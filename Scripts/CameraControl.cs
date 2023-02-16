using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public Transform cameraTarget;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookAt = cameraTarget.position - new Vector3(transform.position.x, cameraTarget.position.y, transform.position.z);
        player.forward = lookAt.normalized;
    }
}
