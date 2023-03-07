using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwordAttack : MonoBehaviour
{
    public GameObject tip; // Reference to the empty GameObject located at the tip of the sword
    public Image dot; //Reference to the UI Image at the center of the screen
    public Color enemyDotColorNear = Color.red; //Reference to the color that the dot will change when aiming at an enemy
    public Color enemyDotColorFar = Color.yellow; //Reference to the color that the dot will change when aiming at an enemy
    public float colorRange = 3f; // Reference to the range at which the sword can cause damage to enemies
    public float colorSpeed = 8;//Reference to the speed the dot color changes
    public float attackRange;
    public static bool attacked; // Has the player attacked?
    GameObject player; // The player game object
    Color neutralDotColor; //The neutral color of the Dot
    
    TrailRenderer trail; //The TrailRenderer at the tip of the sword
    private string[] ingredients;
    public Image ingredient1;
    public Image ingredient2;
    public Image ingredient3;

    public Transform attackPoint;

    private float speedBoost;
    GameObject playerAnimator;


    // Start is called before the first frame update
    void Start()
    {
        attacked = false; //Set attacked to false
        trail = tip.GetComponent<TrailRenderer>(); //get reference to trail component
        trail.enabled = false; // disable the trail renderer
        neutralDotColor = dot.color; // assign the dot's neutral color
        player = GameObject.FindGameObjectWithTag("Player"); //get a reference to the player
        ingredients = new string[3];
        ingredient1.color = Color.gray;
        ingredient2.color = Color.gray;
        ingredient3.color = Color.gray;
        speedBoost = 10.0f;
        playerAnimator = GameObject.FindGameObjectWithTag("PlayerAnimator");
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if the player pressed the fire 1 button and has not attacked
        if (Input.GetButtonDown("Fire1") && !attacked)
        {
            Attack(); // Run the Attack method
        }
        if (ingredients[0] == "Sweet")
        {
            ingredient1.color = Color.magenta;
        }
        if (ingredients[1] == "Sweet")
        {
            ingredient2.color = Color.magenta;
        }
        if (ingredients[2] == "Sweet")
        {
            ingredient3.color = Color.magenta;
        }
        if (ingredients[2] != null)
        {
            if (speedBoost > 0.0f)
            {
                player.GetComponent<PlayerMovement>().moveSpeed = 12;
                speedBoost -= Time.deltaTime;
                Debug.Log("Sugar Cube created! Speed Boost for 10 seconds");
            }
            else
            {
                player.GetComponent<PlayerMovement>().moveSpeed = 10;
                ingredients[0] = null;
                ingredients[1] = null;
                ingredients[2] = null;
                ingredient1.color = Color.gray;
                ingredient2.color = Color.gray;
                ingredient3.color = Color.gray;
                speedBoost = 10.0f;
            }
        }

    }

    void Attack()
    {
        StartCoroutine(AttackAnimation()); //Start the AttackAnimation coroutine
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange);

        foreach(Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                LevelManager.enemiesKilled++;
                for (int i = 0; i < 3; i++)
                {
                    if (ingredients[i] == null)
                    {
                        ingredients[i] = hit.gameObject.GetComponent<EnemyBehavior>().foodGroup();
                        i = 2;
                    }
                }
                Destroy(hit.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void FixedUpdate()
    {
        dotUpdate();//Runs the DotUpdate method
    }

    void dotUpdate()
    {
        RaycastHit hit;

        //Check to see if Raycast hit a collider
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, Mathf.Infinity))
        {
            //Check to see if Raycast hit an enemy
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(player.transform.position, hit.transform.position);//The distance between the enemy and the player
                //Check to see if the enemy is within close range
                if (distance <= colorRange)
                {
                    dot.color = Color.Lerp(dot.color, enemyDotColorNear, Time.deltaTime * colorSpeed); // If raycast is looking at a nearby enemy change the dot color
                }
                else
                {
                    dot.color = Color.Lerp(dot.color, enemyDotColorFar, Time.deltaTime * colorSpeed); // If raycast is looking at a far enemy change the dot color
                }
                

            }
            else
            {
                dot.color = neutralDotColor; // if it is not looking at an enemy, leave the color neutral
            }
        }
    }



    
    IEnumerator AttackAnimation()
    {
        
        trail.enabled = true; // enables the trail renderer at the tip of the sword
        attacked = true; // attacked is set to true
        Animator anim = playerAnimator.GetComponent<Animator>(); //Sets animator int to 2
        anim.SetInteger("animInt", 2);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length); //Wait for 0.6 seconds
        attacked = false; // set attacked back to false
        trail.enabled = false; // disabled the trail renderer at the tip of the sword
        playerAnimator.GetComponent<Animator>().SetInteger("animInt", 0); //Resets animator
    }
}
