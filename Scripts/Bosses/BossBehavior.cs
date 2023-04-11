using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BossBehavior : MonoBehaviour
{
    protected GameObject player;
    protected int currentHealth;
    public TMP_Text bossName;
    public GameObject bossCanvas;
    public Slider healthBar;
    
    
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        bossCanvas.SetActive(true);
        BossStart();
        healthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.isGameOver)
        {
            BossUpdate();
        }
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //PlayDeathAniamtion
            BossDefeated();
            print(currentHealth);
        }
    }

    void BossDefeated()
    {
        FindObjectOfType<LevelManager>().LevelBeat();
    }

    public abstract void BossStart();

    public abstract void BossUpdate();
}
