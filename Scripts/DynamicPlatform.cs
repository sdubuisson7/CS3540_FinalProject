using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatform : MonoBehaviour
{
    public float speed = 1;
    public float distance = 6;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Sin function allows for oscillation
        Vector3 newPos = transform.position;

        newPos.z = startPos.z + (Mathf.Sin(Time.time * speed) * distance);
        transform.position = newPos;
    }


    private void OnTriggerEnter(Collider other){
        Debug.Log("Touching!");
        if (other.CompareTag("Player")) {
            other.transform.SetParent(transform);
        }
    }


    private void OnTriggerExit(Collider other){
        Debug.Log("Not Touching!");
        if (other.CompareTag("Player")) {
            other.transform.SetParent(null);
        }
    }
}
