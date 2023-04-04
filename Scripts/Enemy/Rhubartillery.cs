using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rhubartillery : EnemyBehavior {
    public float speed = 1.0f; // Reference to the movement speed of the enemy
    public float minDistance = 10.0f;
    public float maxDistance = 25.0f;
    public int damage = 20;
    public float throwingInterval = 3.0f;
    public int inaccuracy = 5;
    public List<ParticleSystem> fire;
    public GameObject projectile;
    private float throwingTracker;
    public int maxHealth;

    // Start is called before the first frame update
    public override void EnemyStart() {
        throwingTracker = throwingInterval;
        currentHealth = maxHealth;
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public override void EnemyUpdate() {
        //Make enemy look at player and move towards player
        Vector3 directionToTarget = (player.transform.position - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 100 * Time.deltaTime);
        
        if (Vector3.Distance(this.player.transform.position, transform.position) > maxDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < minDistance) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * -1.0f * speed);
        }
        throwingTracker -= Time.deltaTime;
        if (throwingTracker <= 0.0f) {
            throwingTracker = throwingInterval;
            Instantiate(projectile, new Vector3(player.transform.position.x + ((float) Random.Range(inaccuracy * -100, inaccuracy * 100) / 100.0f), player.transform.position.y + 30.0f, player.transform.position.z + ((float) Random.Range(inaccuracy * -100, inaccuracy * 100) / 100.0f)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
            Instantiate(projectile, new Vector3(player.transform.position.x + ((float) Random.Range(inaccuracy * -100, inaccuracy * 100) / 100.0f), player.transform.position.y + 30.0f, player.transform.position.z + ((float) Random.Range(inaccuracy * -100, inaccuracy * 100) / 100.0f)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
            Instantiate(projectile, new Vector3(player.transform.position.x + ((float) Random.Range(inaccuracy * -100, inaccuracy * 100) / 100.0f), player.transform.position.y + 30.0f, player.transform.position.z + ((float) Random.Range(inaccuracy * -100, inaccuracy * 100) / 100.0f)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
            foreach (ParticleSystem ps in fire) {
                ps.Play();
            }
        }
    }

    public override FoodGroups foodGroup() {
        return FoodGroups.Veggie;
    }
}
