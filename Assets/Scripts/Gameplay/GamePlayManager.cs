using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    public int currentScore;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private float timeToPlay;
    private float _currentTimeLeft;
    private bool _isGameOver;


    void Start()
    {
        currentScore = 0;
        _isGameOver = false;
        _currentTimeLeft = timeToPlay;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameOver) return;

        _currentTimeLeft -= Time.deltaTime;
        if (_currentTimeLeft > 0)
        {
            timerText.text = string.Format("{0:00}", _currentTimeLeft);
        } else
        {
            _isGameOver = true;
            timerText.text = "Times Up!";

            // Update score and head to score screen after fading out
            GameStats.score = currentScore;
            StartCoroutine(TransitionToScoreScreen());
        }
       
    }

    public void OnHumanCaught(Human h)
    {
        // TODO: Just counting right now, but if we want to do points we can update that here
        currentScore += h.scoreValue;
        scoreText.text = string.Format("{0} Humans Caught", currentScore);
    }

    private IEnumerator TransitionToScoreScreen()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("ScoreScreen");
    }
}