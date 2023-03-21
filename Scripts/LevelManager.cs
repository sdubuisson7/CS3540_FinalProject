using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int enemiesKilled;
    public int enemiesToKillToBeatLevel = 10;
    public Text enemiesLeft;
    public Text gameText;
    public static bool isGameOver = false;
    float reload = 5.0f;


    GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        enemiesKilled = 0;
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner");
        enemiesLeft.text = "Enemies Left: 10";
        gameText.gameObject.SetActive(false);
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
            else
            {
                isGameOver = true;
                gameText.text = "Congrats! You win";
                gameText.gameObject.SetActive(true);
                enemiesLeft.gameObject.SetActive(false);
                Destroy(spawner);
            }
        }
        else if(enemiesToKillToBeatLevel > enemiesKilled)
        {
            reload -= Time.deltaTime;
            gameText.text = "Game Over\nRestarting in " + reload.ToString("f2");
        }
    }

    void setEnemiesLeft()
    {
        enemiesLeft.text = "Enemies Left: " + (enemiesToKillToBeatLevel - enemiesKilled).ToString();
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
