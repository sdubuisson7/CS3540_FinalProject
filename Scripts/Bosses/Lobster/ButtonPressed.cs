using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressed : MonoBehaviour
{
    public GameObject lobsterBoss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lobsterBoss.GetComponent<LobsterBehavior>().HeatOn();
        }
    }

}
