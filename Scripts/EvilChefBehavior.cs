using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

    // cooldown between swinging attacks
    public float idleTimer = 3.0f;
    private float timeTillSpawn;


    // cooldown for when enemy is stunned
    public float spawnTimer = 5.0f;
    public float spawnCooldown;
    public GameObject enemySpawnPoint;


    // public float stunnedTimer = 5.0f;
    public float survivalTimer = 60.0f;
    public Text enemiesLeft;

    // flourbomb prefab
    // public GameObject flourBomb;
    // empty game object at bombing hand
    // public GameObject bombHand;
    public GameObject pan;

    // time to play angry animation
    // public float attackTimer = 10.0f;

    bool angryAnimationPlayed;
    bool tauntAnimationPlayed;
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
    public int damage;


    public override void BossStart()
    {
        spawnTimer = 5.0f;
        // attackTimer = 10.0f;
        // stunnedTimer = 5.0f;
        currentState = ChefState.Spawn;
        bossName.text = "THE EVIL CHEF";
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        angryAnimationPlayed = false;
        tauntAnimationPlayed = false;
        anim = gameObject.GetComponent<Animator>();
        pan = GameObject.FindGameObjectWithTag("Pan");
        pan.SetActive(false);
        isAngry = false;
        Invoke("spawnEnemies", 10.0f);


    }

    private void spawnEnemies() {
        foreach(GameObject enemy in enemiesToSpawn)
        {
            Instantiate(enemy, enemySpawnPoint.transform.position * Random.Range(1.0f, 10.0f), Quaternion.identity);
        }
    }

    public override void BossUpdate()
    {
        
        survivalTimer -= Time.deltaTime;
        enemiesLeft.text = "Survival Time: " + survivalTimer.ToString("f2");

        if (survivalTimer > 0) {
            switch (currentState)
            {
                case ChefState.Spawn:
                    TauntUpdate();
                    break;
                case ChefState.Idle:
                    IdleUpdate();
                    break;
                case ChefState.Swinging:
                    elapsedTime += Time.deltaTime;

                    SwingingUpdate();

                    break;
                
            }
            if (survivalTimer <= 30.0f) {
                isAngry = true;
            }
        }
        else {
            takeDamage(100);
        }

        
    }

    private void TauntUpdate()
    {

        print("TauntUpdate");
        if (!tauntAnimationPlayed) {
            AudioSource.PlayClipAtPoint(evilLaughSFX, transform.position);
            anim.SetInteger("animState", 1);
            tauntAnimationPlayed = true;
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0) {
            currentState = ChefState.Idle;
            spawnTimer = 3.0f;

        }
    }

    private void IdleUpdate()
    {
        //Chef Idle
        print("IdleUpdate");
        timeTillSpawn += Time.deltaTime;
        pan.SetActive(true);
        anim.SetInteger("animState", 0);
        
        currentState = ChefState.Swinging;
        
    }

    private void SwingingUpdate()
    {
        
        //Chef Attacks
        print("SwingingUpdate");
        // Debug.Log("Angry: " + isAngry.ToString());

        // attackTimer -= Time.deltaTime;
        if (!isAngry) {
            print("Phase 1");
            damage = 20;
            anim.SetInteger("animState", 2);
            
        }
        else {
            print("Phase 2");
            damage = 20;
            anim.SetInteger("animState", 3);

        }
        
    }

    public override void BossDeadEffects()
    {
        if (!angryAnimationPlayed) {
            AudioSource.PlayClipAtPoint(angrySFX, transform.position);
            anim.SetInteger("animState", 6);
            print("Evil Chef Defeated!");
            angryAnimationPlayed = true;

            Destroy(gameObject, 2.0f);
        }
        
    }

}
