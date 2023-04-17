using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuBehavior : MonoBehaviour
{
    public TextMeshProUGUI defeatedEnemiesText;
    public static int defeatedEnemies;

    public void LoadFirstLevel(string scene)
    {

        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        defeatedEnemies = PlayerPrefs.GetInt("defeatedEnemies", 0);
        defeatedEnemiesText.text = "ENEMIES DEFEATED: " + defeatedEnemies.ToString();
    }
}
