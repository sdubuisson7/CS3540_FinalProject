using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonNavBehavior : MonoBehaviour
{
    public enum dragonState
    {
        following,
        goingToEnemy,
        Attack
    }

    public dragonState currentState;
    public float detectionRadius = 5;
    public float attackingRange = 7;
    public float dragonDuration = 30;
    public int damage = 5;

    private bool inCoolDown = false;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private Collider[] detectedEnemies;
    private ParticleSystem flamethrower;
    private float elapsedTimeSinceSpawned;

   
    public AudioClip dragonFruitEndSFX;
    bool dragonFruitEndPlayed;

    // Start is called before the first frame update
    void Start()
    {
        dragonFruitEndPlayed = false;
        agent = GetComponent<NavMeshAgent>();
        currentState = dragonState.following;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        flamethrower = GetComponentInChildren<ParticleSystem>();
        elapsedTimeSinceSpawned = 0;

    }

    // Update is called once per frame
    void Update()
    {
        elapsedTimeSinceSpawned += Time.deltaTime;
        if (elapsedTimeSinceSpawned >= dragonDuration)
        {
            if (!dragonFruitEndPlayed)
            {
                AudioSource.PlayClipAtPoint(dragonFruitEndSFX, transform.position);
                dragonFruitEndPlayed = true;
                Destroy(gameObject);
                return;
            }
        }

        detectedEnemies = Physics.OverlapSphere(transform.position, detectionRadius, 1 << 3); // 1 << 3 for Enemy layer only
        switch (currentState)
        {
            case dragonState.following:
                UpdateFollowing();
                break;
            case dragonState.goingToEnemy:
                UpdateGoingToEnemy();
                break;
            case dragonState.Attack:
                UpdateAttack();
                break;

        }

    }


    void UpdateFollowing()
    {
        agent.SetDestination(player.position);
        agent.stoppingDistance = 3;
        agent.autoBraking = true;
        agent.speed = 7.5f;
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (detectedEnemies.Length > 0)
        {
            currentState = dragonState.goingToEnemy;
        }

    }

    void UpdateGoingToEnemy()
    {
        if (AllEnemiesDead())
        {
            currentState = dragonState.following;
            return;
        }
        agent.SetDestination(detectedEnemies[0].transform.position);
        agent.speed = 9f;
        agent.autoBraking = false;
        agent.stoppingDistance = attackingRange;
        if (agent.remainingDistance <= attackingRange)
        {
            currentState = dragonState.Attack;
        }
    }

    void UpdateAttack()
    {
        if (AllEnemiesDead())
        {
            animator.SetBool("Attack", false);
            currentState = dragonState.following;
            return;
        }
        if (agent.remainingDistance > attackingRange)
        {
            GetComponentInChildren<FlamethrowerControlls>().attacking = false;
            animator.SetBool("Attack", false);
            currentState = dragonState.goingToEnemy;
            return;
        }
        if (Vector3.Distance(detectedEnemies[0].transform.position, transform.position) > attackingRange)
        {
            GetComponentInChildren<FlamethrowerControlls>().attacking = false;
            animator.SetBool("Attack", false);
            currentState = dragonState.goingToEnemy;
            return;
        }
        Vector3 direction = detectedEnemies[0].transform.position - transform.position;
        direction.y = 0;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 180 * Time.deltaTime);
        animator.SetBool("Attack", true);
        
        if (GetComponentInChildren<FlamethrowerControlls>().attacking && !inCoolDown)
        {
            //Damage Enemy
            //detectedEnemies[0].gameObject.takeDamage()
            //For now this will only destroy the enemy, Delete this after implementing enemy health system!!!!
            //Also Remember to do LevelManager.enemiesKilled++ in the enemyDestroy function and remove it from swordattack. 
            //Currently we are counting the enemies killed in the SwordAttack script. 
            //animator.SetBool("Attack", false);
          
            if(detectedEnemies[0].gameObject.GetComponent<EnemyBehavior>() != null)
            {

                detectedEnemies[0].gameObject.GetComponent<EnemyBehavior>().Hit(damage);
                inCoolDown = true;
                Invoke("CooldownAttack", 2.0f);
                
            }
            
        }

    }
    
    private bool AllEnemiesDead()
    {
        if (detectedEnemies.Length <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

    }

    private void CooldownAttack()
    {
        inCoolDown = false;
    }
}

