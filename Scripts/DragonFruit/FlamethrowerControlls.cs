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
<<<<<<< HEAD
        if (attacking == true) {
            AudioSource.PlayClipAtPoint(flameBreathSFX, transform.position);
        }
=======
        AudioSource.PlayClipAtPoint(flameBreathSFX, transform.position);
        
>>>>>>> 7e35fdae63b905475f87603ca622823a083e1c94
    }

    public void FlamethrowerOff()
    {
        flamethrower.Stop();
        attacking = false;
    }
}
