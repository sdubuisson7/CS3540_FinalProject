using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static int enemiesKilled;
    public int enemiesToKillToBeatLevel = 10;
    public Text enemiesLeft;

    GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        enemiesKilled = 0;
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner");
        enemiesLeft.text = "Enemies Left: 10";
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesToKillToBeatLevel > enemiesKilled)
        {
            setEnemiesLeft();
        }
        else
        {
            enemiesLeft.text = "Congrats! You win";
        }
        Destroy(spawner);

    }

    void setEnemiesLeft()
    {
        enemiesLeft.text = "Enemies Left: " + (enemiesToKillToBeatLevel - enemiesKilled).ToString();
    }
}
