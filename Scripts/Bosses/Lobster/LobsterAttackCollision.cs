using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobsterAttackCollision : MonoBehaviour
{
    public GameObject collideEffect;
    private float timeSinceLastCollision;
    private float timeSinceItDidDamage;

    private void Update()
    {
        timeSinceLastCollision += Time.deltaTime;
        timeSinceItDidDamage += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (timeSinceItDidDamage >= 1f)
            {
                collision.gameObject.GetComponent<PlayerHealth>().Hit(35);
                timeSinceItDidDamage = 0;
            }
                
        }
        else if (!collision.gameObject.CompareTag("Enemy"))
        {
            
            {
                Instantiate(collideEffect, collision.contacts[0].point, Quaternion.identity);
                timeSinceLastCollision = 0;
            }
            
        }
    }


}

