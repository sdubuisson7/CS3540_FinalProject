using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinnamonRoller : EnemyBehavior {
    public float speed = 4; // Reference to the movement speed of the enemy
    public int damage = 10;
    public float damageInterval = 2.0f;
    private float damageCountdown;

    // Start is called before the first frame update
    public override void EnemyStart() {
        damageCountdown == 0.0f;
    }

    // Update is called once per frame
    public override void EnemyUpdate() {
        damageCountdown -= Time.deltaTime;
        //Make enemy look at player and move towards player
        transform.LookAt(player.transform);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
    }

    void OnCollisionEnter(Collision c) {
        //Check if Enemy collided with Player
        if (c.gameObject.CompareTag("Player") && damageCountdown > 0.0f) {
            player.GetComponent<PlayerHealth>().Hit(damage);
            Debug.Log("PlayerHit");
            damageCountdown = damageInterval;
        }
    }

    public override FoodGroups foodGroup() {
        return FoodGroups.Sweet;
    }
}
