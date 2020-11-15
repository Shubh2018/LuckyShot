using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField]
    Text text;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text scorePageText;
    [SerializeField]
    Text backText;
    [SerializeField]
    Animator anim;
    [SerializeField]
    Canvas gameOver;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        gameOver.enabled = false;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Help()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void HelpNextPage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void DisplayText(string textToDisplay)
    {
        text.text = textToDisplay;
    }

    public void DisplayScore(int score)
    {
        scoreText.text = score.ToString();
        scorePageText.text = "Your Score: " + score.ToString();
    }

    public void EnableGameOverScreen()
    {
        gameOver.enabled = true;
        anim.SetTrigger("End");
    }

}
