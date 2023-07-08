using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
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

    private bool _musicStarted;

    void Start()
    {
        if(role == MenuRole.SCORE && scoreText != null)
        {
            scoreText.text = string.Format("You Caught {0} Humans!", GameStats.score);
        }
        else if (role == MenuRole.MAIN)
        {
            StartCoroutine(PlayMainMenuMusic());
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnStartPress()
    {
        GameStats.score = 0;
        AudioManager.Instance.PlayOneShot(AudioEvent.START_GAME);
        SceneManager.LoadScene("HumanTesting");
    }

    public void OnMenuPress()
    {
        GameStats.score = 0;
        AudioManager.Instance.PlayOneShot(AudioEvent.BUTTON_CLICK);
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator PlayMainMenuMusic()
    {
        while (!AudioManager.Instance.hasLoaded)
            yield return new WaitForSeconds(0.25f);
        AudioManager.Instance.PlayMusic(MusicType.MENU);
    }

    public void OnMouseHoverButton()
    {
        AudioManager.Instance.PlayOneShot(AudioEvent.BUTTON_HOVER);
    }
}
