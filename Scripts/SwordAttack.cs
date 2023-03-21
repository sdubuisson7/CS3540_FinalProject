using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SwordAttack : MonoBehaviour
{
    public GameObject tip; // Reference to the empty GameObject located at the tip of the sword
    public Image dot; //Reference to the UI Image at the center of the screen
    public Color enemyDotColorNear = Color.red; //Reference to the color that the dot will change when aiming at an enemy
    public Color enemyDotColorFar = Color.yellow; //Reference to the color that the dot will change when aiming at an enemy
    public float colorRange = 3f; // Reference to the range at which the sword can cause damage to enemies
    public float colorSpeed = 8; //Reference to the speed the dot color changes
    
    public float attackRange;
    public static bool attacked; // Has the player attacked?
    GameObject player; // The player game object
    Color neutralDotColor; //The neutral color of the Dot
    
    public enum FoodGroups {
        None,
        Sweet,
        Veggie,
        Meat,
        Starch,
        Spice,
        Dairy
    }

    // TODO: can switch to enum later once we figure out recipe names
    private string currentRecipe;

    TrailRenderer trail; //The TrailRenderer at the tip of the sword
    private FoodGroups[] ingredientsList;
    public Image ingredient1;
    public Image ingredient2;
    public Image ingredient3;
    private Image[] ingredientImages;
    // Used to show what recipe we have
    public Text recipeEffectsText;

    // sword swing audio
    public AudioClip swordSFX;
    // Recipe FX audio
    public AudioClip eatSFX;
    // powerup SFX
    public AudioClip powerupSFX;

    // for powerup SFX
    bool powerupPlayed;
    

    public Transform attackPoint;

    private float speedBoostTimer;
    private float attackBoostTimer;
    GameObject playerAnimator;


    // Start is called before the first frame update
    void Start()
    {
        // no recipe set
        currentRecipe = null;
        powerupPlayed = false;
        ingredientImages = new Image[3]{ingredient1, ingredient2, ingredient3};
        attacked = false; 
        // get reference to trail component
        trail = tip.GetComponent<TrailRenderer>(); 
        // disable the trail renderer
        trail.enabled = false; 
        // assign the dot's neutral color
        neutralDotColor = dot.color; 
        player = GameObject.FindGameObjectWithTag("Player");
        ingredientsList = new FoodGroups[3]{FoodGroups.None, FoodGroups.None, FoodGroups.None};

        for (int i = 0; i < ingredientImages.Length; i++) {
            ingredientImages[i].color = Color.gray;
        }
        
        speedBoostTimer = 10.0f;
        attackBoostTimer = 10.0f;
        playerAnimator = GameObject.FindGameObjectWithTag("PlayerAnimator");
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if the player pressed the fire 1 button and has not attacked
        if (Input.GetButtonDown("Fire1") && !attacked)
        {
            Attack(); 
        }

        // Update the recipe loadout in the UI
        UpdateRecipeLoadout();

        // If we've filled the ingredientsList, apply the power up
        if (ingredientsList[ingredientsList.Length - 1] != FoodGroups.None)
        {
            UpdateCurrentRecipe();
            if (currentRecipe != null) {
                // TODO: Add some kind of noise here
                // Reset our recipe loadout
                ApplyPowerup();
            }
            else {
                ResetRecipeLoadout();

            }
        }
        if (LevelManager.isGameOver)
        {
            gameObject.SetActive(false);
        }

    }

    void Attack()
    {
        if(!LevelManager.isGameOver)
        {
            trail.enabled = true; // enables the trail renderer at the tip of the sword
            attacked = true; // attacked is set to true

            // Sets animator int to 2
            Animator anim = playerAnimator.GetComponent<Animator>();
            anim.SetInteger("animInt", 2);

            AudioSource.PlayClipAtPoint(swordSFX, transform.position);

            // go back to neutral position
            if (player.GetComponent<PlayerMovement>().isMoving)
            {
                Invoke("ResetAnimation", 0.9f);
            }
            else
            {
                Invoke("ResetAnimation", 0.95f);
            }



            Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange);

            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    LevelManager.enemiesKilled++;

                    // Update the food name in the ingredientsList array
                    for (int i = 0; i < ingredientsList.Length; i++)
                    {
                        if (ingredientsList[i] == FoodGroups.None)
                        {
                            AudioSource.PlayClipAtPoint(eatSFX, transform.position);

                            FoodGroups group = (FoodGroups) hit.gameObject.GetComponent<EnemyBehaviour>().foodGroup();
                            ingredientsList[i] = group;
                            break;
                        }
                    }

                    Destroy(hit.gameObject);

                }
            }
            
        }
        
    }

    //To Visualize the attack point in the inspector
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void FixedUpdate()
    {
        // Update the dot reticle at screen center
        DotUpdate();
    }

    void DotUpdate()
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
    
    void ResetAnimation()
    {        
        attacked = false; // set attacked back to false
        trail.enabled = false; // disabled the trail renderer at the tip of the sword
        playerAnimator.GetComponent<Animator>().SetInteger("animInt", 0); //Resets animator
    }


    void UpdateRecipeLoadout() 
    {
        for (int i = 0; i < ingredientsList.Length; i++) 
        {
            if (ingredientsList[i] != FoodGroups.None) 
            {
                switch (ingredientsList[i]) 
                {
                    case FoodGroups.Sweet:
                        ingredientImages[i].color = Color.magenta;
                        break;
                    case FoodGroups.Veggie:
                        ingredientImages[i].color = Color.green;
                        break;
                    case FoodGroups.Meat:
                        ingredientImages[i].color = new Color(1.0f, 0.6f, 0.0f, 1f);
                        break;
                    case FoodGroups.Starch:
                        ingredientImages[i].color = Color.yellow;
                        break;
                    case FoodGroups.Spice:
                        ingredientImages[i].color = Color.red;
                        break;
                    case FoodGroups.Dairy:
                        ingredientImages[i].color = Color.white;
                        break;
                    default:
                        break;
                } 
                
            }
        }
    }

    void ApplyPowerup() 
    {
        if (!powerupPlayed) 
        {
            AudioSource.PlayClipAtPoint(eatSFX, transform.position);
            AudioSource.PlayClipAtPoint(powerupSFX, transform.position);
            powerupPlayed = true;
        }

        switch (currentRecipe) {
            case "Sugar Cube":
                ApplySugarRush();
                break;
            case "Meat Skewer":
                ApplyProteinPunch();
                break;
            default:
                // We didn't make a recipe
                break;
            
        }
        
    }

    void ApplyProteinPunch() {
        if (speedBoostTimer > 0.0f)
        {
            Debug.Log("Meat Skewer created! Attack Boost for 10 seconds");
            attackBoostTimer -= Time.deltaTime;

            // TODO: Attack Boost for player

            recipeEffectsText.text = "Recipe: Meat Skewer\nEffect: Protein Punch\nTime: " + attackBoostTimer.ToString("f2");
        }
        else
        {
            // reset the effects of the power up
            ResetPowerUp();

            // Reset our recipe loadout
            ResetRecipeLoadout();
            
        }

        
    }



    void ApplySugarRush() {
        
        if (speedBoostTimer > 0.0f)
        {

            player.GetComponent<PlayerMovement>().moveSpeed = 20;
            speedBoostTimer -= Time.deltaTime;
            Debug.Log("Sugar Cube created! Speed Boost for 10 seconds");
            recipeEffectsText.text = "Recipe: Sugar Cube\nEffect: Sugar Rush\nTime: " + speedBoostTimer.ToString("f2");

        }
        else
        {
            // reset the effects of the power up
            ResetPowerUp();

            // Reset our recipe loadout
            ResetRecipeLoadout();
            
        }
    }

    // return the recipe that 
    void UpdateCurrentRecipe() 
    {
        
        if (Array.TrueForAll(ingredientsList, element => element == FoodGroups.Sweet)) {
            currentRecipe = "Sugar Cube";
        }
        else if (Array.TrueForAll(ingredientsList, element => element == FoodGroups.Meat))
        {
            currentRecipe = "Meat Skewer";
        }
        // TODO: More cases, based on recipe specs
    }

    void ResetRecipeLoadout() 
    {
        recipeEffectsText.text = "Recipe: -";
        for(int i = 0; i < ingredientsList.Length; i++) 
        {
            ingredientsList[i] = FoodGroups.None;
            ingredientImages[i].color = Color.gray;
        }
    }

    void ResetPowerUp() {
        powerupPlayed = false;

        switch (currentRecipe) {
            case "Sugar Cube":
                player.GetComponent<PlayerMovement>().moveSpeed = 10;
                speedBoostTimer = 10.0f;
                break;
            case "Meat Skewer":
                // TODO: Attack stuff
                speedBoostTimer = 10.0f;
                break;
            default:
                break;
        }

        currentRecipe = null;

    }
}
