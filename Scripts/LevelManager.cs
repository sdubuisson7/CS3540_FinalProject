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
    public Slider bossHealthBar;
    public static bool isGameOver = false;
    float reload = 5.0f;
    public string nextLevel;


    bool bossStarted = false;
    GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        enemiesKilled = 0;
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner");
        enemiesLeft.text = "Enemies Left: 10";
        gameText.gameObject.SetActive(false);
        //bossFightCanvas.SetActive(false);
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
                StartBossFight();
                //LevelBeat();
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
        //Play some Boss Music??

        //Activates Boss Canvas
        //bossFightCanvas.SetActive(true);
        //Instantiate this levels boss
        bossStarted = true;
        GameObject[] enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject aliveEnemy in enemiesAlive)
        {
            Destroy(aliveEnemy);
        }
        GameObject currentBoss = Instantiate(boss, new Vector3(0, 3, 0), Quaternion.identity);
        enemiesLeft.gameObject.SetActive(false);
        
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
        Destroy(spawner);
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
        Destroy(spawner);
        Invoke("ReloadLevel", 5);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
