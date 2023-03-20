using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatbrawlerProjectile : MonoBehaviour {
    private bool active;
    private float lifespan;
    private int damage;

    // Start is called before the first frame update
    void Start() {
        active = false;
    }

    // Update is called once per frame
    void Update() {
        if (active) {
            transform.Translate(transform.forward * 100.0f * Time.deltaTime);
            lifespan -= Time.deltaTime;
            if (lifespan <= 0.0f) {
                Destroy(this);
            }
        }
    }

    void OnCollisionEnter(Collision c) {
        //Check if Enemy collided with Player
        if (c.gameObject.CompareTag("Player")) {
            c.gameObject.GetComponent<PlayerHealth>().Hit(damage);
            Debug.Log("PlayerHit");
            Destroy(this);
        }
    }

    public void Activate(int dmg) {
        damage = dmg;
        active = true;
        lifespan = 5.0f;
    }
}