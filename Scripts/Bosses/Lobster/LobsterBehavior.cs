using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

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

    public Transform lobsterAttackUpPosition;

    public ParticleSystem steamNormal;
    public ParticleSystem steamFast;

    public AudioClip boilingSFX;

    public Color heatingWaterColor;

    public float spawnCooldown;
    public float attackCooldown;
    public float stoppingDistance = 15;
    public float stunnedForce;
    public float stunnedTime;
    public float attackForce;


    public int maxHealth;
    

    private NavMeshAgent agent;
    private float timeTillSpawn;
    private float timeTillAttack;
    private bool isStunned = false;
    private bool isBeingLaunched = false;

    private GameObject stunnedLobster;

    private Color startingButtonColor;
    private Color startingWaterColor;
    private Color startingHeatingColor;

    private Color currentButtonColor;
    private Color currentWaterColor;
    private Color currentHeatingColor;
    

    private bool deadEffectsFinished = false;

    public override void BossStart()
    {
        
        currentState = LobsterState.Potted;
        bossName.text = "THE LOBSTER";
        currentHealth = maxHealth;
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
            currentState = LobsterState.Attacking;
            timeTillAttack = 0;
        }

        if (agent.remainingDistance <= agent.stoppingDistance && distance >= 8)
        {
            
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
        print("AttackingUpdate");
        if (!isBeingLaunched)
        {
            lobster.transform.position = Vector3.Lerp(lobster.transform.position, lobsterAttackUpPosition.transform.position, Time.deltaTime * 3);
            lobster.transform.LookAt(player.transform.position);
        }
        
        if(Vector3.Distance(lobster.transform.position, lobsterAttackUpPosition.transform.position) <= 0.2f && !isBeingLaunched)
        {
            isBeingLaunched = true;
            StartCoroutine(AttackLaunch());
        }
        currentButtonColor = startingButtonColor;
        currentHeatingColor = startingHeatingColor;
        currentWaterColor = startingWaterColor;
        UpdateColors();
    }

    private IEnumerator AttackLaunch()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        lobster.GetComponent<Rigidbody>().isKinematic = false;
        lobster.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(0.1f);
        lobster.GetComponent<Rigidbody>().AddForce(lobster.transform.forward * attackForce, ForceMode.Impulse);
        yield return new WaitForSeconds(1.5f);
        lobster.GetComponent<Rigidbody>().isKinematic = true;
        lobster.GetComponent<Rigidbody>().useGravity = false;
        Instantiate(lobsterPuffEffect, lobster.transform.position, Quaternion.identity);
        MeshRenderer[] lobsterParts = lobster.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mesh in lobsterParts)
        {
            mesh.enabled = false;
        }
        yield return new WaitForSeconds(1f);
        lobster.transform.localPosition = new Vector3(-0.046f, 0.54f, 0.472f);
        lobster.transform.localRotation = Quaternion.Euler(new Vector3(-4.349f, 0, 0));
        Instantiate(lobsterPuffEffect, lobster.transform.position, Quaternion.identity);
        foreach (MeshRenderer mesh in lobsterParts)
        {
            mesh.enabled = true;
        }
        isBeingLaunched = false;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        currentState = LobsterState.Potted;
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
        AudioSource.PlayClipAtPoint(boilingSFX, Camera.main.transform.position);
        yield return new WaitForSeconds(4);
        
        lobster.SetActive(false);
        stunnedLobster =  Instantiate(lobsterWithRigidbodies, lobster.transform.position, lobster.transform.rotation,gameObject.transform);
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

    public override void BossDeadEffects()
    {
        if (!deadEffectsFinished)
        {
            deadEffectsFinished = true;
            StopCoroutine(LobsterIsStunned());
            Instantiate(lobsterPuffEffect, stunnedLobster.transform.position, Quaternion.identity);
            Instantiate(lobsterPuffEffect, potWater.transform.position, Quaternion.identity);
            Instantiate(lobsterPuffEffect, heatingElement.transform.position, Quaternion.identity);
            Destroy(gameObject, 0.2f);
           
        }    
    }

    public void HeatOn()
    {
        if(currentState == LobsterState.Potted) //Can only stun lobster while potted to avoid glitches
        {
            currentState = LobsterState.Stunned;
            steamNormal.Stop();
            steamFast.Play();
        }
    }



    
}
