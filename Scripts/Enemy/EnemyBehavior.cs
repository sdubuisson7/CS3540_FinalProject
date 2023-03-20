using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start() {
        //Get player GameObject with tag
        player = GameObject.FindGameObjectWithTag("Player"); 
        EnemyStart();
    }

    // Update is called once per frame
    void Update() {
        EnemyUpdate();
    }

    public abstract void EnemyStart();

    public abstract void EnemyUpdate();

    public abstract FoodGroups foodGroup();
}