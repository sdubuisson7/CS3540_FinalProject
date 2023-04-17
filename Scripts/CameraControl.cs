using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public Transform player; // The player transform reference to keep the player looking forward
    public Transform cameraTarget; // The camera target transfrom reference where the camera will look at
    public CinemachineFreeLook brains;
    float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        //Hide Cursor
        rotationSpeed = PlayerPrefs.GetFloat("rotationSpeed", 300);
        brains = GetComponentInChildren<CinemachineFreeLook>();
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
            brains.m_XAxis.m_MaxSpeed = rotationSpeed;
            brains.m_YAxis.m_MaxSpeed = rotationSpeed * 0.00416667f;//ratio for camera control
            Vector3 lookAt = cameraTarget.position - new Vector3(transform.position.x, cameraTarget.position.y, transform.position.z);
            player.forward = Vector3.Lerp(player.forward, lookAt.normalized, Time.deltaTime * 30);
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
