using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerControlls : MonoBehaviour
{
    public bool attacking;
    private ParticleSystem flamethrower;

    private void Start()
    {
        flamethrower = GetComponentInChildren<ParticleSystem>();
        
    }
    
    public void FlamethrowerOn()
    {
        flamethrower.Play();
        attacking = true;
    }

    public void FlamethrowerOff()
    {
        flamethrower.Stop();
        attacking = false;
    }
}
