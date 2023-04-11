using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LobsterBehavior : BossBehavior
{
    public enum LobsterState
    {
        Potted,
        Attacking,
        Stunned,
    }
    public LobsterState currentState;
    
    public GameObject[] enemiesToSpawn;

    public GameObject onButton;
    public GameObject heatingElement;
    public GameObject potWater;
    public GameObject lobster;
    public GameObject lobsterWithRigidbodies;
    public GameObject lobsterPuffEffect;

    public ParticleSystem steamNormal;
    public ParticleSystem steamFast;

    public Color heatingWaterColor;

    public float spawnCooldown;
    public float attackCooldown;
    public float stoppingDistance = 15;
    public float stunnedForce;
    public float stunnedTime;


    public int maxHealth;
    

    private NavMeshAgent agent;
    private float timeTillSpawn;
    private float timeTillAttack;
    private bool isStunned = false;


    private Color startingButtonColor;
    private Color startingWaterColor;
    private Color startingHeatingColor;

    private Color currentButtonColor;
    private Color currentWaterColor;
    private Color currentHeatingColor;
    

public override void BossStart()
    {
        currentHealth = maxHealth;
        currentState = LobsterState.Potted;
        bossName.text = "THE LOBSTER";
        healthBar.maxValue = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        steamFast.Stop();
        steamNormal.Play();

        startingButtonColor = onButton.GetComponent<MeshRenderer>().material.color;
        startingWaterColor = potWater.GetComponent<MeshRenderer>().material.color;
        startingHeatingColor = heatingElement.GetComponent<MeshRenderer>().material.color;


    }

    public override void BossUpdate()
    {
        switch (currentState)
        {
            case LobsterState.Potted:
                PottedUpdate();
                break;
            case LobsterState.Attacking:
                AttackingUpdate();
                break;
            case LobsterState.Stunned:
                StunnedUpdate();
                break;
        }
        
    }

    private void PottedUpdate()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        agent.SetDestination(player.transform.position);
        agent.stoppingDistance = stoppingDistance;
        timeTillAttack += Time.deltaTime;
        timeTillSpawn += Time.deltaTime;

        if(timeTillSpawn >= spawnCooldown)
        {
            foreach(GameObject enemy in enemiesToSpawn)
            {
                Instantiate(enemy, transform.position + Vector3.forward * Random.Range(1.0f, 10.0f), Quaternion.identity);
            }
            timeTillSpawn = 0;
        }

        if(timeTillAttack >= attackCooldown)
        {
            //currentState = LobsterState.Attacking;
            print("Attacked");
            timeTillAttack = 0;
        }

        if (agent.remainingDistance <= agent.stoppingDistance && distance >= 8)
        {
            print("made it");
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 180 * Time.deltaTime);
        }
        currentButtonColor = startingButtonColor;
        currentHeatingColor = startingHeatingColor;
        currentWaterColor = startingWaterColor;
        UpdateColors();
    }

    private void AttackingUpdate()
    {
        //Lobster Attacks
        print("Attacked");
        currentButtonColor = startingButtonColor;
        currentHeatingColor = startingHeatingColor;
        currentWaterColor = startingWaterColor;
        UpdateColors();
    }

    private void StunnedUpdate()
    {
        
        //Add SFX for boiling water here
        currentWaterColor = heatingWaterColor;
        currentHeatingColor = Color.red;
        currentButtonColor = Color.green;
        UpdateColors();
        
        if (!isStunned)
        {
            StartCoroutine(LobsterIsStunned());
        }
    }

    private IEnumerator LobsterIsStunned()
    {
        
        isStunned = true;
        yield return new WaitForSeconds(4);
        //Vector3 lobsterOriginalPosition = lobster.transform.position;
        lobster.SetActive(false);
        GameObject stunnedLobster =  Instantiate(lobsterWithRigidbodies, lobster.transform.position, lobster.transform.rotation,gameObject.transform);
        Rigidbody[] lobsterRigidbodies = stunnedLobster.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in lobsterRigidbodies)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.up * stunnedForce, ForceMode.Impulse);
           
        }

        yield return new WaitForSeconds(stunnedTime);
        Instantiate(lobsterPuffEffect, stunnedLobster.GetComponentInChildren<Rigidbody>().transform.position, Quaternion.identity);
        Destroy(stunnedLobster);
        yield return new WaitForSeconds(1f);
        //Particle System poof
        Instantiate(lobsterPuffEffect, lobster.transform.position, Quaternion.identity);
        lobster.SetActive(true);
        isStunned = false;
        currentState = LobsterState.Potted;
        steamNormal.Play();
        steamFast.Stop();
    }

    private void UpdateColors()
    {
        Color previousButtonColor = onButton.GetComponent<MeshRenderer>().material.color;
        Color previousWaterColor = potWater.GetComponent<MeshRenderer>().material.color;
        Color previousHeatinElementcolor = heatingElement.GetComponent<MeshRenderer>().material.color;

        onButton.GetComponent<MeshRenderer>().material.color = Color.Lerp(previousButtonColor, currentButtonColor, Time.deltaTime * 5f);
        potWater.GetComponent<MeshRenderer>().material.color = Color.Lerp(previousWaterColor, currentWaterColor, Time.deltaTime * 1.5f);
        heatingElement.GetComponent<MeshRenderer>().material.color = Color.Lerp(previousHeatinElementcolor, currentHeatingColor, Time.deltaTime * 2.4f);
    }

    public void HeatOn()
    {
        if(currentState != LobsterState.Stunned)
        {
            currentState = LobsterState.Stunned;
            steamNormal.Stop();
            steamFast.Play();
        }
    }

    
}
