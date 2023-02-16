using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordAttack : MonoBehaviour
{
    public GameObject tip; // Reference to the empty GameObject located at the tip of the sword
    public Image dot; //Reference to the UI Image at the center of the screen
    public Color enemyDotColor = Color.red; //Reference to the color that the dot will change when aiming at an enemy
    public float swordRange = 1.5f; // Reference to the range at which the sword can cause damage to enemies
    GameObject player; // The player game object
    Color neutralDotColor; //The neutral color of the Dot
    bool attacked; // Has the player attacked?
    TrailRenderer trail; //The TrailRenderer at the tip of the sword

    // Start is called before the first frame update
    void Start()
    {
        attacked = false; //Set attacked to false
        trail = tip.GetComponent<TrailRenderer>(); //get reference to trail component
        trail.enabled = false; // disable the trail renderer
        neutralDotColor = dot.color; // assign the dot's neutral color
        player = GameObject.FindGameObjectWithTag("Player"); //get a reference to the player

    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if the player pressed the fire 1 button and has not attacked
        if (Input.GetButtonDown("Fire1") && !attacked)
        {
            Attack(); // Run the Attack method
        }
        

    }

    void Attack()
    {
        StartCoroutine(AttackAnimation()); //Start the AttackAnimation coroutine
        RaycastHit hit;

        //Check to see if Raycast hit a collider
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            //Check to see if hit collider is an enemy
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                
                float distance = Vector3.Distance(player.transform.position, hit.transform.position);//The distance between the enemy and the player
                //Check to see if the enemy is within range to get hit by sword
                if (distance <= swordRange)
                {
                    //Kill/Damage Enemy
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    void FixedUpdate()
    {
        dotUpdate();//Runs the DotUpdate method
    }

    void dotUpdate()
    {
        RaycastHit hit;

        //Check to see if Raycast hit a collider
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            //Check to see if Raycast hit an enemy
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                dot.color = Color.Lerp(dot.color, enemyDotColor, Time.deltaTime * 10); // If raycast is looking at an enemy change the dot color

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
        GetComponent<Animator>().SetTrigger("Attack"); // sets attack trigger to run the sword attack animation

        yield return new WaitForSeconds(0.6f); //Wait for 0.6 seconds
        attacked = false; // set attacked back to false
        trail.enabled = false; // disabled the trail renderer at the tip of the sword
    }
}
