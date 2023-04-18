using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilChefBehavior : BossBehavior
{
    public int maxHealth = 100;
    public AudioClip evilLaughSFX;
    public AudioClip angrySFX;

    public enum ChefState
    {
        Spawn,
        Idle,
        Swinging,
        Throwing,
        Stunned,
    }

    // cooldown between swinging attacks
    public float idleTimer = 3.0f;

    // cooldown for when enemy is stunned
    public float stunnedTimer = 5.0f;

    // flourbomb prefab
    public GameObject flourBomb;
    // empty game object at bombing hand
    public GameObject bombHand;
    public GameObject pan;

    // time to play angry animation
    public float angryTimer = 7.0f;

    bool angryAnimationPlayed;
    public bool isAngry;

    // for throwing state
    float elapsedTime = 0;
    public float shootRate = 2.0f;
    public float attackRate = 3.0f;



    // Swing 3 times before we go into throwing state
    int rounds = 3;
    
    // spawn enemies
    public GameObject[] enemiesToSpawn;

    Animator anim;

    public ChefState currentState;


    public override void BossStart()
    {
        currentState = ChefState.Spawn;
        bossName.text = "THE EVIL CHEF";
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        angryAnimationPlayed = false;
        anim = gameObject.GetComponent<Animator>();
        pan = GameObject.FindGameObjectWithTag("Pan");
        pan.SetActive(false);
        isAngry = false;


    }

    public override void BossUpdate()
    {
        /* Vector3 directionToTarget = (gameObject.transform.position - player.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, 10 * Time.deltaTime); */

        switch (currentState)
        {
            case ChefState.Spawn:
                EvilUpdate();
                break;
            case ChefState.Idle:
                IdleUpdate();
                break;
            case ChefState.Swinging:
                elapsedTime += Time.deltaTime;

                SwingingUpdate();

                break;
            case ChefState.Throwing:
                elapsedTime += Time.deltaTime;

                ThrowingUpdate();

                break;
            case ChefState.Stunned:
                StunnedUpdate();
                break;
        }
        /* if (currentHealth <= 50) {
            isAngry = true;
        }*/

        
    }

    private void EvilUpdate()
    {

        print("EvilUpdate");
        anim.SetInteger("animState", 1);

        angryTimer -= Time.deltaTime;
        if (angryTimer <= 0) {
            currentState = ChefState.Idle;
            angryTimer = 3.0f;

        }
        // currentState = ChefState.Idle;
    }

    private void IdleUpdate()
    {
        //Chef Idle
        print("IdleUpdate");
        
        pan.SetActive(true);
        anim.SetInteger("animState", 0);

        
        currentState = ChefState.Swinging;
        
    }

    private void SwingingUpdate()
    {
        //Chef Attacks
        print("SwingingUpdate");
        Debug.Log("Angry: " + isAngry.ToString());

        if (!isAngry) {
            print("Phase 1");
            anim.SetInteger("animState", 2);
            /* if (elapsedTime >= attackRate) {
                // Get the length of attack animation (3)
                var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;

                // delay the spellcasting to the end of the animation duration
                Invoke("ShootPlayer", animDuration);
                anim.SetInteger("animState", 0);

                elapsedTime = 0.0f;
            } */
            // Invoke("AttackPlayer", animDuration);
                
            
        }
        else {
            print("Phase 2");
            anim.SetInteger("animState", 3);

        }
        
    }

    private void AttackPlayer() {

        if (!isAngry) {
            anim.SetInteger("animState", 2);
        }

    }

    private void ThrowingUpdate()
    {
        //Chef Throws Flour Bomb
        anim.SetInteger("animState", 3);

        print("ThrowingUpdate");
        
        if (elapsedTime >= shootRate) {
            // Get the length of attack animation (3)
            var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;

            // delay the spellcasting to the end of the animation duration
            Invoke("ThrowBombs", animDuration);
            elapsedTime = 0.0f;

        }
        
        
    }

    private void ThrowBombs() {
        print("Throwing Flour Bombs");
        Instantiate(flourBomb, bombHand.transform.position, bombHand.transform.rotation);
        
        Vector3 directionToTarget = (flourBomb.transform.position - player.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(flourBomb.transform.rotation, lookRotation, 10 * Time.deltaTime);

        // TODO: Add MoveTowards script w/ particle effects to flourBomb prefab

    }

    private void StunnedUpdate()
    {
        //Chef Stunned
        print("StunnedUpdate");
        
        
    }

    public override void BossDeadEffects()
    {
        anim.SetInteger("animState", 4);
        print("Evil Chef Defeated!");
        Destroy(gameObject, 2.0f);
        
    }

}
