using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarolinaReaper : EnemyBehavior {
    public float speed = 6.0f; // Reference to the movement speed of the enemy
    public int damage = 20;
    public int maxHealth = 8;
    public float explosionRange = 2.0f;
    public float explosionFuse = 3.0f;
    public GameObject explosion;

    private float countdown = -1.987f;

    // Start is called before the first frame update
    public override void EnemyStart() {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    public override void EnemyUpdate() {
        //Make enemy look at player and move towards player
        Vector3 directionToTarget = (player.transform.position - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 100 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        
        if (countdown == -1.987f) {
            if (Vector3.Distance(player.transform.position, transform.position) < 2.0f) {
                countdown = explosionFuse;
            }
        } else if (countdown <= 0.0f) {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        } else {
            countdown -= Time.deltaTime;
        }
    }

    public override FoodGroups foodGroup() {
        return FoodGroups.Spice;
    }
}
