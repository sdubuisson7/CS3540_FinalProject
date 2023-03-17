using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream:Scripts/EnemyBehavior.cs
public class EnemyBehaviorChase : MonoBehaviour
{
=======
public class CinnamonRoller : EnemyBehaviour {
>>>>>>> Stashed changes:Scripts/Enemy/CinnamonRoller.cs
    public float speed = 4; // Reference to the movement speed of the enemy
    public int damage = 20;

    // Start is called before the first frame update
    public override void EnemyStart() {

    }

    // Update is called once per frame
    public override void EnemyUpdate() {
        //Make enemy look at player and move towards player
        transform.LookAt(player.transform);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
    }
<<<<<<< Updated upstream:Scripts/EnemyBehavior.cs

    private void OnTriggerEnter(Collider other)
    {
        //Check if Enemy collided with Player
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerHealth>().Hit(damage);
            Debug.Log("PlayerHit");
        }
=======
    
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.Hit(damage);
            Debug.Log("Player Hit");
        }
    }

    public override FoodGroup foodGroup() {
        return FoodGroup.Sweet;
>>>>>>> Stashed changes:Scripts/Enemy/CinnamonRoller.cs
    }
}
