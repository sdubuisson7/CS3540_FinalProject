using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoMasher : EnemyBehavior {
    public float speed = 6.0f; // Reference to the movement speed of the enemy
    public float minDistance = 12.5f;
    public float maxDistance = 15.0f;
    public int damage = 20;
    public int maxHealth = 8;

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
        
        if (Vector3.Distance(this.player.transform.position, transform.position) > maxDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < minDistance) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * -1.0f * (speed / 2));
        }
    }

    public override FoodGroups foodGroup() {
        return FoodGroups.Starch;
    }
}
