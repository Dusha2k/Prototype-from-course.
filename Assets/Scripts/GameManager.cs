using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float _waitAfterDying = 2f;

    [HideInInspector]
    public bool ending;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PlayerDied()
    {
        StartCoroutine(delayOfDie());
    }

    public IEnumerator delayOfDie()
    {
        yield return new WaitForSeconds(_waitAfterDying);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseUnpause()
    {
        if (UIController.instance.pauseScreen.activeInHierarchy)
        {
            UIController.instance.pauseScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            Cursor.visible = false;

            PlayerController.instance.soundWalk.Play();
            PlayerController.instance.soundRun.Play();
        }
        else
        {
            UIController.instance.pauseScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            Cursor.visible = true;

            PlayerController.instance.soundWalk.Stop();
            PlayerController.instance.soundRun.Stop();
        }
    }
}
