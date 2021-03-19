using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    public string mainMenu;
    public float timeBetweenShowing;
    public GameObject textBox, returnButton;

    public Image blackScreen;
    public float blackScreenFade;
    void Start()
    {
        StartCoroutine(ShowObjectsCo());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, blackScreenFade * Time.deltaTime));
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public IEnumerator ShowObjectsCo()
    {
        yield return new WaitForSeconds(timeBetweenShowing);
        textBox.SetActive(true);

        yield return new WaitForSeconds(timeBetweenShowing);
        returnButton.SetActive(true);
    }
}
