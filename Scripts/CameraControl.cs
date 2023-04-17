using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player; // The player transform reference to keep the player looking forward
    public Transform cameraTarget; // The camera target transfrom reference where the camera will look at
    float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        //Hide Cursor
        rotationSpeed = PlayerPrefs.GetFloat("rotationSpeed", 30);
        print("rotationSpeed = " + rotationSpeed);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.isGameOver)
        {
            //Calculates the forward vector from the camera and keeps the player looking forward
            Vector3 lookAt = cameraTarget.position - new Vector3(transform.position.x, cameraTarget.position.y, transform.position.z);
            player.forward = Vector3.Lerp(player.forward, lookAt.normalized, Time.deltaTime * rotationSpeed);
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
