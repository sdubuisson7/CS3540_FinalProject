using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatbrawlerProjectile : MonoBehaviour {
    private bool active = false;
    private float lifespan;
    private int damage;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (active) {
                       
            lifespan -= Time.deltaTime;
            if (lifespan <= 0.0f) {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision c) {
        //Check if Enemy collided with Player
        if (c.gameObject.CompareTag("Player")) {
            c.gameObject.GetComponent<PlayerHealth>().Hit(damage);
            Debug.Log("PlayerHit");
            Destroy(gameObject);
        }
    }

    public void Activate(int dmg) {
        damage = dmg;
        active = true;
        lifespan = 5.0f;
        print("Activate work");
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 10, ForceMode.VelocityChange);
    }
}
