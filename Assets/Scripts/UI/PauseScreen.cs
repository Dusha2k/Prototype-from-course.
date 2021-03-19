using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public string mainMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        GameManager.instance.PauseUnpause();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);

        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QuitGame");
    }
}
