using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    public int currentScore;
    private GameObject mainCamera;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private float timeToPlay;
    private float _currentTimeLeft;
    private bool _isGameOver;

    private bool _timerRunning = true;

    void Start()
    {
        currentScore = 0;
        _isGameOver = false;
        _currentTimeLeft = timeToPlay;
        this.mainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameOver) return;

        if (_timerRunning)
        {
            _currentTimeLeft -= Time.deltaTime;
            if (_currentTimeLeft > 0)
            {
                timerText.text = string.Format("{0:00}", _currentTimeLeft);
            }
            else
            {
                _isGameOver = true;
                timerText.text = "Times Up!";

                // Update score and head to score screen after fading out
                GameStats.score = currentScore;
                StartCoroutine(TransitionToScoreScreen());
            }
        }
    }

    public void OnHumanCaught(Human h)
    {
        // TODO: Just counting right now, but if we want to do points we can update that here
        currentScore += h.scoreValue;
        UpdateScoreDisplay();
    }

    public void GoToMinigame()
    {
        this._timerRunning = false;
        this.mainCamera.SetActive(false);
    }
    public void ReturnFromMinigame(bool hasWon)
    {
        Debug.Log("returning from minigame won:" + hasWon.ToString());
        this._timerRunning = true;
        this.mainCamera.SetActive(true);
        SceneManager.UnloadSceneAsync("RhythmMinigame");

        PlayerController.Instance.GetComponent<FishingPoleController>().ResetBait();
        PlayerController.Instance.UnlockPlayer();

        if(hasWon)
        {
            GameStats.score += 1;
            UpdateScoreDisplay();
        }

    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = string.Format("{0}", currentScore);
    }

    private IEnumerator TransitionToScoreScreen()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("ScoreScreen");
    }
}
