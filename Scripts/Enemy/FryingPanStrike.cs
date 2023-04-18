using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPanStrike : MonoBehaviour
{
    public GameObject player;
    int damage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        damage = GameObject.FindGameObjectWithTag("Boss").GetComponent<EvilChefBehavior>().damage;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //donde esta el health component del player y add health. also destroy after collection.
            print("PlayerDamaged!");
            other.gameObject.GetComponent<PlayerHealth>().Hit(damage);
        }
    }
}
