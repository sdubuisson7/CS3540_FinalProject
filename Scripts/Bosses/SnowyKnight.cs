using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnowyKnight : BossBehavior {
    public int maxHealth;
    public AudioSource clang;
    public GameObject attackArea;
    public GameObject snowAttack;
    public float snowFrequency;
    public List<TrailRenderer> trails;

    public enum SnowyPhase { Phase1, Phase1Attacking, Transition1, Phase2, Transition2, Waiting, Phase3, Phase3Attacking }

    private SnowyPhase phase;
    private NavMeshAgent agent;
    private bool deadEffectsFinished;
    private Animator anim;
    private float hailCountdown;

    // Start is called before the first frame update
    public override void BossStart() {
        phase = SnowyPhase.Phase1;
        currentHealth = maxHealth;
        bossName.text = "SNOWY KNIGHT";
        healthBar.maxValue = maxHealth;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        hailCountdown = 0.0f;
    }

    // Update is called once per frame
    public override void BossUpdate() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Normal")) {
            anim.ResetTrigger("BlockAttack");
        }

        foreach (TrailRenderer t in trails) {
            t.emitting = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        }

        switch (phase) {
            case SnowyPhase.Phase1:
                //Debug.Log("Phase 1; Walk");
                if (currentHealth <= maxHealth / 2) {
                    phase = SnowyPhase.Transition1;
                    break;
                }
                if (transform.Find(attackArea.name) != null) {
                    Destroy(transform.Find(attackArea.name).gameObject);
                }
                agent.SetDestination(player.transform.position);
                agent.stoppingDistance = 2.0f;
                if (agent.remainingDistance > 1.0f) {
                    Vector3 directionToTarget = (player.transform.position - transform.position).normalized;
                    directionToTarget.y = 0;
                    Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 100 * Time.deltaTime);
                } else {
                    anim.SetTrigger("Attack");
                    phase = SnowyPhase.Phase1Attacking;
                }
                break;
            case SnowyPhase.Phase1Attacking:
                Debug.Log("Phase 1; Attack!");
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Normal")) {
                    phase = SnowyPhase.Phase1;
                    anim.ResetTrigger("Attack");
                    if (transform.Find(attackArea.name) == null) {
                        Invoke("CreateAttackArea", 1.0f);
                    }
                }
                break;
            case SnowyPhase.Transition1:
                anim.SetTrigger("JumpAway");
                StartCoroutine(JumpAway());
                phase = SnowyPhase.Waiting;
                break;
            case SnowyPhase.Phase2:
                StartCoroutine(Volley());
                break;
            case SnowyPhase.Transition2:
                anim.SetTrigger("JumpBack");
                StartCoroutine(JumpBack());
                phase = SnowyPhase.Waiting;
                break;
            case SnowyPhase.Phase3:
                if (transform.Find(attackArea.name) != null) {
                    Destroy(transform.Find(attackArea.name).gameObject);
                }
                if (hailCountdown <= 0.0f) {
                    hailCountdown = snowFrequency;
                    Instantiate(snowAttack, transform.position + (Vector3.up * 30.0f) + (Vector3.forward * Random.Range(-10.0f, 10.0f)) + (Vector3.left * Random.Range(-10.0f, 10.0f)), transform.rotation);
                } else {
                    hailCountdown -= Time.deltaTime;
                }
                agent.SetDestination(player.transform.position);
                agent.stoppingDistance = 2.0f;
                if (agent.remainingDistance > 1.0f) {
                    Vector3 directionToTarget = (player.transform.position - transform.position).normalized;
                    directionToTarget.y = 0;
                    Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 100 * Time.deltaTime);
                } else {
                    anim.SetTrigger("Attack");
                    phase = SnowyPhase.Phase1Attacking;
                }
                break;
            case SnowyPhase.Phase3Attacking:
                if (hailCountdown <= 0.0f) {
                    hailCountdown = snowFrequency;
                    Instantiate(snowAttack, transform.position + (Vector3.up * 30.0f) + (Vector3.forward * Random.Range(-10.0f, 10.0f)) + (Vector3.left * Random.Range(-10.0f, 10.0f)), transform.rotation);
                } else {
                    hailCountdown -= Time.deltaTime;
                }
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Normal")) {
                    phase = SnowyPhase.Phase3;
                    anim.ResetTrigger("Attack");
                    if (transform.Find(attackArea.name) == null) {
                        Invoke("CreateAttackArea", 1.0f);
                    }
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator JumpAway() {
        yield return new WaitForSeconds(0.2f);
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        phase = SnowyPhase.Phase2;
    }

    private IEnumerator JumpBack() {
        yield return new WaitForSeconds(0.2f);
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        transform.Translate(Vector3.up * 5.0f);
        yield return null;
        phase = SnowyPhase.Phase3;

    }

    private IEnumerator Volley() {
        Instantiate(snowAttack, transform.position + (Vector3.forward * -10.0f), transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.forward * -8.0f), transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.forward * -6.0f), transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.forward * -4.0f), transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.forward * -2.0f), transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.forward * 2.0f), transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.forward * 4.0f), transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.forward * 6.0f), transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.forward * 8.0f), transform.rotation);
        Instantiate(snowAttack, transform.position + (Vector3.left * -10.0f) + Vector3.forward, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.forward * 10.0f), transform.rotation);
        Instantiate(snowAttack, transform.position + (Vector3.left * -8.0f) + Vector3.forward, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.left * -6.0f) + Vector3.forward, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.left * -4.0f) + Vector3.forward, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.left * -2.0f) + Vector3.forward, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + Vector3.forward, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.left * 2.0f) + Vector3.forward, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.left * 4.0f) + Vector3.forward, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.left * 6.0f) + Vector3.forward, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.left * 8.0f) + Vector3.forward, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(snowAttack, transform.position + (Vector3.left * 10.0f) + Vector3.forward, transform.rotation);
        yield return new WaitForSeconds(0.1f);
        phase = SnowyPhase.Transition2;
    }

    public override void takeDamage(int damage) {
        if (phase == SnowyPhase.Phase1Attacking || phase == SnowyPhase.Phase3Attacking) {
            currentHealth -= damage;
            healthBar.value = currentHealth;
            if (currentHealth <= 0) {
                currentHealth = 0;
                BossDefeated();
                print(currentHealth);
            }
        } else {
            anim.SetTrigger("BlockAttack");
            clang.Play();
        }
    }

    private void CreateAttackArea() {
        Instantiate(attackArea, transform.position + transform.forward, transform.rotation, transform);
    }

    public override void BossDeadEffects() {
        if (!deadEffectsFinished) {
            deadEffectsFinished = true;
            //StopCoroutine(LobsterIsStunned());
            //Instantiate(lobsterPuffEffect, stunnedLobster.transform.position, Quaternion.identity);
            //Instantiate(lobsterPuffEffect, potWater.transform.position, Quaternion.identity);
            //Instantiate(lobsterPuffEffect, heatingElement.transform.position, Quaternion.identity);
            Destroy(gameObject, 0.2f);
           
        }
    }
}
