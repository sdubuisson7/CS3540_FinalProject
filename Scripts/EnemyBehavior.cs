using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBehavior : MonoBehaviour
{
    public float speed = 4; // Reference to the movement speed of the enemy
    Transform player; // Referene to the transfrom of the player so that the enemy can follow the player.

    // Start is called before the first frame update
    void Start()
    {
        //Get player GameObject with tag
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    // Update is called once per frame
    void Update()
    {
        //Make enemy look at player and move towards player
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if Enemy collided with Player
        if (other.CompareTag("Player"))
        {
            //Player is hit(Take Damage/Die/GameOver)
            //Destroy(other);
            Debug.Log("PlayerHit");
        }
    }
}
