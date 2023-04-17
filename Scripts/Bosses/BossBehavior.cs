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
    public bool isDead = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        bossCanvas = GameObject.FindGameObjectWithTag("BossCanvas");
        for (int i = 0; i < bossCanvas.transform.childCount; i++)
        {
            bossCanvas.transform.GetChild(i).gameObject.SetActive(true);
        }
        bossName = GameObject.FindGameObjectWithTag("BossName").GetComponent<TMP_Text>();
        healthBar = GameObject.FindGameObjectWithTag("BossHealth").GetComponent<Slider>();
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
        else if(isDead)
        {
            BossDeadEffects();
        }
    }

    public virtual void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        print(currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            BossDefeated();
            print(currentHealth);
        }
    }

    protected void BossDefeated() {
        isDead = true;
        FindObjectOfType<LevelManager>().LevelBeat();
    }

    public abstract void BossStart();

    public abstract void BossUpdate();

    public abstract void BossDeadEffects();
}
