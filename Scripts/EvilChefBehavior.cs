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

    // time to play angry animation
    public float angryTimer = 3.0f;

    bool angryAnimationPlayed;

    // for throwing state
    float elapsedTime = 0;
    public float shootRate = 2.0f;



    // Swing 3 times before we go into throwing state
    int swingingRounds = 3;
    
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


    }

    public override void BossUpdate()
    {
        switch (currentState)
        {
            case ChefState.Spawn:
                EvilUpdate();
                break;
            case ChefState.Idle:
                IdleUpdate();
                break;
            case ChefState.Swinging:
                SwingingUpdate();
                break;
            case ChefState.Throwing:
                ThrowingUpdate();
                break;
            case ChefState.Stunned:
                StunnedUpdate();
                break;
        }
        elapsedTime += Time.deltaTime;

        
    }

    private void EvilUpdate()
    {
        anim.SetInteger("animState", 1);

        print("AngryUpdate");
    }

    private void IdleUpdate()
    {
        anim.SetInteger("animState", 0);

        //Chef Attacks
        print("IdleUpdate");
        
    }

    private void SwingingUpdate()
    {

        //Chef Attacks
        print("SwingingUpdate");
        
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
