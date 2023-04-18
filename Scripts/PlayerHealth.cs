using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    public int maxHealth = 100;
    int currentHealth;
    public Slider healthSlider;
    Animator animator;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        healthSlider.value = currentHealth;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Hit(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            currentHealth = 0;
            animator.SetBool("isDead", true);
            FindObjectOfType<LevelManager>().LevelLost();
        }
        healthSlider.value = currentHealth;
    }

    public void Heal(int amount) {
        currentHealth += amount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        healthSlider.value = currentHealth;
    }

    public int getHealth() {
        return currentHealth;
    }

    public float getPrcHealth() {
        return (float) currentHealth / (float) maxHealth;
    }
}
