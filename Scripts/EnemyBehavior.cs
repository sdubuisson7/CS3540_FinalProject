using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 4; // Reference to the movement speed of the enemy
    public int damage = 20;
    GameObject player; // Referene to the player so that the enemy can follow the player.

    
    // Start is called before the first frame update
    void Start()
    {
        //Get player GameObject with tag
        player = GameObject.FindGameObjectWithTag("Player"); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.isGameOver)
        {
            //Make enemy look at player and move towards player
            transform.LookAt(player.transform);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.Hit(damage);
            Debug.Log("Player Hit");
        }
    }

    public string foodGroup()
    {
        if (gameObject.name == "CinnamonRoller(Clone)")
        {
            return "Sweet";
        }
        else
        {
            return gameObject.name;
        }
    }
}
