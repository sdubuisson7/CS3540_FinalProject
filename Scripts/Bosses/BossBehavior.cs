using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BossBehavior : MonoBehaviour
{
    protected GameObject player;
    public int currentHealth;
    public GameObject bossCanvas;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bossCanvas.SetActive(true);
        healthBar.value = currentHealth;
        BossStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.isGameOver)
        {
            BossUpdate();
        }
    }

    void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //PlayDeathAniamtion
            BossDefeated();
            
        }
    }

    void BossDefeated()
    {
        FindObjectOfType<LevelManager>().LevelBeat();
    }

    public abstract void BossStart();

    public abstract void BossUpdate();
}
