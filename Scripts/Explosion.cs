using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public float countdown = 2.0f;
    public int dpt = 1;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        countdown -= Time.deltaTime;
        if (countdown <= 0.0f) {
            Destroy(gameObject);
        }
    }
}
