using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffStage : MonoBehaviour
{
    // Start is called before the first frame update
    bool levelLost = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < -5) {
            if (gameObject.tag == "Player" && !levelLost) {
                levelLost = true;
                FindObjectOfType<LevelManager>().LevelLost();
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}
