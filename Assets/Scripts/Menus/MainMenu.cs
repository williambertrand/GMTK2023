using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public enum MenuRole
{
    MAIN,
    SCORE
}
public class MainMenu : MonoBehaviour
{
    [SerializeField] private MenuRole role;
    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        if(role == MenuRole.SCORE && scoreText != null)
        {
            scoreText.text = string.Format("You Caught {0} Humans!", GameStats.score);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartPress()
    {
        GameStats.score = 0;
        SceneManager.LoadScene("HumanTesting");
    }

    public void OnMenuPress()
    {
        GameStats.score = 0;
        SceneManager.LoadScene("Menu");
    }
}
