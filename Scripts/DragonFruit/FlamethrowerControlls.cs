using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerControlls : MonoBehaviour
{
    public bool attacking;
    private ParticleSystem flamethrower;
    public AudioClip flameBreathSFX;
    public GameObject dragonFruit;


    private void Start()
    {
        flamethrower = GetComponentInChildren<ParticleSystem>();
        dragonFruit = GameObject.FindGameObjectWithTag("DragonFruit");
        
    }
    
    public void FlamethrowerOn()
    {
        flamethrower.Play();
        attacking = true;
        AudioSource.PlayClipAtPoint(flameBreathSFX, transform.position);
        
    }

    public void FlamethrowerOff()
    {
        flamethrower.Stop();
        attacking = false;
    }
}
