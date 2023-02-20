using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour {
    public float speed = 1.0f;
    public float xAxis = 0.0f;
    public float yAxis = 0.0f;
    public float zAxis = 0.0f;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(new Vector3(speed * xAxis * Time.deltaTime, speed * yAxis * Time.deltaTime, speed * zAxis * Time.deltaTime));
    }
}
