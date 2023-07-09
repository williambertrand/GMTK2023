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

    [SerializeField] private ParticleSystem _bubbles;

    [SerializeField] private GameObject _fish;

    private bool _bubblesOn;
    private float _bubbleSpeed;

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

        _bubbleSpeed = 20.0f;
        _bubbles.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(_bubblesOn)
        {
            _bubbleSpeed += 1.5f * Time.deltaTime;
            var main = _bubbles.main;
            main.startSpeed = _bubbleSpeed;
            _fish.transform.Translate(new Vector3(_bubbleSpeed * 0.25f * Time.deltaTime * -1, 0.0f, 0.0f));
        }
    }

    public void OnStartPress()
    {
        GameStats.score = 0;
        AudioManager.Instance.PlayOneShot(AudioEvent.START_GAME);

        if(role == MenuRole.MAIN)
        {
            _bubbles.gameObject.SetActive(true);
            _bubblesOn = true;
            StartCoroutine(LoadSceneWithTransition());
        }
    }

    private IEnumerator LoadSceneWithTransition()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlayMusic(MusicType.RELAXED);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("GamePlayFinal");
    }

    public void OnMenuPress()
    {
        GameStats.score = 0;
        AudioManager.Instance.PlayOneShot(AudioEvent.BUTTON_CLICK);
        SceneManager.LoadScene("MenuScene");
    }

    private IEnumerator PlayMainMenuMusic()
    {
        while (!AudioManager.Instance.hasLoaded)
            yield return new WaitForSeconds(0.25f);
        AudioManager.Instance.PlayMusic(MusicType.RELAXED);
    }

    public void OnMouseHoverButton()
    {
        AudioManager.Instance.PlayOneShot(AudioEvent.BUTTON_HOVER);
    }

    public void OnMouseClickButton()
    {
        AudioManager.Instance.PlayOneShot(AudioEvent.BUTTON_CLICK);
    }
}
