using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static int enemiesKilled;
    public int enemiesToKillToBeatLevel = 10;
    public Text enemiesLeft;
    public Text gameText;
    public GameObject boss;
    public AudioClip gameMusic;
    public AudioClip bossMusic;
    public float bossMusicVolume;
    public static bool isGameOver = false;
    float reload = 5.0f;
    public string nextLevel;

    public Vector3 bossOrigin = new Vector3(0, 3, 0);


    bool bossStarted = false;
    GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        enemiesKilled = 0;
        
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner");
        enemiesLeft.text = "Enemies Left: " + enemiesToKillToBeatLevel.ToString();
        gameText.gameObject.SetActive(false);
        Camera.main.GetComponent<AudioSource>().clip = gameMusic;
        Camera.main.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            if (enemiesToKillToBeatLevel > enemiesKilled)
            {
                setEnemiesLeft();
            }
            else if(!bossStarted)
            {
                if(gameObject.scene.name == "Level1")
                {
                    LevelBeat();
                }
                else
                {
                    StartBossFight();
                }
            }
        }
        else if(enemiesToKillToBeatLevel > enemiesKilled)
        {
            if (reload > 0.0f)
            {
                reload -= Time.deltaTime;
            }
            
            gameText.text = "Game Over\nRestarting in " + reload.ToString("f2");
        }
        
    }

    void StartBossFight()
    {
        //Play Boss Music
        Camera.main.GetComponent<AudioSource>().clip = bossMusic;
        Camera.main.GetComponent<AudioSource>().Play();
        Camera.main.GetComponent<AudioSource>().volume = bossMusicVolume;
        
        bossStarted = true;
        //Destroy Current live enemies
        GameObject[] enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject aliveEnemy in enemiesAlive)
        {
            Destroy(aliveEnemy);
        }
        //Instantiate this levels boss
        GameObject currentBoss = Instantiate(boss, bossOrigin, Quaternion.identity);
        if(gameObject.scene.name != "Level4")
        {
            enemiesLeft.gameObject.SetActive(false);
        }
        
        
        //Disable enemySpawner
        FindObjectOfType<EnemySpawner>().enabled = false;
       
    }

    void setEnemiesLeft()
    {
        enemiesLeft.text = "Enemies Left: " + (enemiesToKillToBeatLevel - enemiesKilled).ToString();
    }

    public void LevelBeat() 
    {
        isGameOver = true;
        gameText.text = "Congrats! You win";
        gameText.gameObject.SetActive(true);
        enemiesLeft.gameObject.SetActive(false);
        FindObjectOfType<PlayerMovement>().wonAnimation();
        if (!string.IsNullOrEmpty(nextLevel)) {
            Invoke("LoadNextLevel", 3);
        }

    }

    void LoadNextLevel() {
        SceneManager.LoadScene(nextLevel);
    }
    

    public void LevelLost()
    {
        isGameOver = true;
        gameText.text = "GAME OVER";
        gameText.gameObject.SetActive(true);
        Invoke("ReloadLevel", 5);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
