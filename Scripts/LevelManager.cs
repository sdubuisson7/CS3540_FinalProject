using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static int enemiesKilled;
    public int enemiesToKillToBeatLevel = 10;
    public Text enemiesLeft;
    public Text gameText;
    public static bool isGameOver = false;

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
    }

    void setEnemiesLeft()
    {
        enemiesLeft.text = "Enemies Left: " + (enemiesToKillToBeatLevel - enemiesKilled).ToString();
    }

    public void LevelLost()
    {
        isGameOver = true;
        gameText.text = "GAME OVER, YOU LOSE";
        gameText.gameObject.SetActive(true);
        Destroy(spawner);
    }
}
