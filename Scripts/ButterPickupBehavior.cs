using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterPickupBehavior : MonoBehaviour
{
    public float duration = 5;
    public int healthPoints = 20;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //donde esta el health component del player y add health. also destroy after collection.
            other.gameObject.GetComponent<PlayerHealth>().Heal(healthPoints);
            Destroy(gameObject);
        }
    }
}
