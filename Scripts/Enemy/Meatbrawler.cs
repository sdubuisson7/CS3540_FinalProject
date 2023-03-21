using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meatbrawler : EnemyBehavior {
    public float speed = 2.0f; // Reference to the movement speed of the enemy
    public float minDistance = 12.5f;
    public float maxDistance = 15.0f;
    public int damage = 20;
    public float throwingInterval = 3.0f;
    //public GameObject scuttlePortion;
    public GameObject throwingArm;
    public GameObject meatball;
    public GameObject meatballSpawn;
    private float throwingTracker;
    private bool thrown;

    // Start is called before the first frame update
    public override void EnemyStart() {
        throwingTracker = throwingInterval + 2.0f;
        thrown = false;
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
        //scuttlePortion.transform.rotation = Quaternion.Euler(0.0f, Mathf.Sin(Time.time) * 3.0f, 0.0f);
        throwingTracker -= Time.deltaTime;
        if (throwingTracker <= 2.0f) {
            throwingArm.transform.localRotation = Quaternion.Slerp(throwingArm.transform.localRotation, Quaternion.AngleAxis(60f,Vector3.right), Time.deltaTime * 20);
            if (throwingTracker <= 0.0f) {
                throwingTracker = throwingInterval + 2.0f;
                thrown = false;
            } else if (throwingTracker <= 1.9f && !thrown) {
                MeatbrawlerProjectile proj = Instantiate(meatball, meatballSpawn.transform.position + meatballSpawn.transform.forward, transform.rotation).GetComponent<MeatbrawlerProjectile>();
                proj.Activate(damage);
                thrown = true;
            }
        } else {
            throwingArm.transform.localRotation = Quaternion.Euler(20.0f, 0.0f, 0.0f);
        }

        //print(Vector3.Distance(player.transform.position, transform.position));
    }

    public override FoodGroups foodGroup() {
        return FoodGroups.Meat;
    }
}
