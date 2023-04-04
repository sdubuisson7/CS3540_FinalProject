using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilkJuggernaut : EnemyBehavior {
    public float speed = 2; // Reference to the movement speed of the enemy
    public int damage = 15;
    private bool inCooldown = false;
    public int maxHealth = 15;
    // Start is called before the first frame update
    public override void EnemyStart() {
        currentHealth = maxHealth;
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public override void EnemyUpdate() {
        //Make enemy look at player and move towards player
        transform.LookAt(player.transform);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
    }


    void OnCollisionEnter(Collision c) {
        //Check if Enemy collided with Player
        if (c.gameObject.CompareTag("Player")) {
            if(!inCooldown)
            player.GetComponent<PlayerHealth>().Hit(damage);
            inCooldown = true;
            Invoke("CooldownAttack", 1.5f);
            Debug.Log("PlayerHit");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inCooldown = false;
        }
    }

    void CooldownAttack()
    {
        inCooldown = false;
    }

    public override FoodGroups foodGroup() {
        return FoodGroups.Dairy;
    }
}
