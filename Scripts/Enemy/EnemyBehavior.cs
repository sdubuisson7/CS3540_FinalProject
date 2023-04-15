using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FoodGroups {
    None,
    Sweet,
    Veggie,
    Meat,
    Starch,
    Spice,
    Dairy
}

public abstract class EnemyBehavior : MonoBehaviour {
    protected GameObject player; // Referene to the player so that the enemy can follow the player.
    // public int maxHealth;
    protected int currentHealth;
    Slider healthSlider;
    public bool isDead;
    public GameObject butterDrop;
    private int probabilityOfDrop = 20;

    // Start is called before the first frame update
    void Start() {
        //Get player GameObject with tag
        player = GameObject.FindGameObjectWithTag("Player");
        isDead = false;
        healthSlider = GetComponentInChildren<Slider>();
        EnemyStart();
    }

    // Update is called once per frame
    void Update() {
        if (!LevelManager.isGameOver)
        {
            EnemyUpdate();
        }
        
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            LevelManager.enemiesKilled++;
            Destroy(gameObject);
            isDead = true;
            int chance = Random.Range(0, 101);
            if(chance > (100 - probabilityOfDrop))
            {
                Instantiate(butterDrop, transform.position, Quaternion.identity);
            }

        }
        healthSlider.value = currentHealth;
    }
    public abstract void EnemyStart();

    public abstract void EnemyUpdate();

    public abstract FoodGroups foodGroup();
}
