using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    public int maxHealth = 100;
    int currentHealth;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Hit(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            currentHealth = 0;
            // PLAYER DIES
        }
    }

    public void Heal(int amount) {
        currentHealth += amount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

    public int getHealth() {
        return currentHealth;
    }

    public float getPrcHealth() {
        return (float) currentHealth / (float) maxHealth;
    }
}
