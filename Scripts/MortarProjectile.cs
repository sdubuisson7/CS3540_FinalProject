using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarProjectile : MonoBehaviour {
    public GameObject warningPrefab;
    public int damage;
    public bool canHitPlayer;
    public bool canHitEnemies;
    private GameObject target;

    // Start is called before the first frame update
    void Start() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity)) {
            target = Instantiate(warningPrefab, hit.point, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        } else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.tag == "Player" && canHitPlayer) {
            c.gameObject.GetComponent<PlayerHealth>().Hit(damage);
        }
        if (canHitEnemies) {
            // TODO: Make Mortar Damage Enemies
        }
        Destroy(target);
        Destroy(gameObject);
    }
}
