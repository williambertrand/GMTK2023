using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioEvent
{
    BUTTON_HOVER,
    BUTTON_CLICK,
    START_GAME,

    PLAYER_CAST,
    PLAYER_CATCH
}

public enum MusicType
{
    MENU,
    RELAXED,
    ACTIVE
}

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    public bool hasLoaded;

    Dictionary<AudioEvent, AudioClip> sfxClips;
    Dictionary<MusicType, AudioClip> musicClips;

    private AudioSource _audioSFX;
    private AudioSource _audioMusic;

    // TODO: smooth music transitions
    // https://www.youtube.com/watch?v=c3NdUYDyRhE&ab_channel=PawelMakesGames

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(gameObject);

        _audioSFX = gameObject.AddComponent<AudioSource>();
        _audioMusic = gameObject.AddComponent<AudioSource>();

        sfxClips = new Dictionary<AudioEvent, AudioClip>();

        sfxClips.Add(AudioEvent.BUTTON_HOVER, loadClip("UI/UI_Button_hover_01"));
        sfxClips.Add(AudioEvent.BUTTON_CLICK, loadClip("UI/UI_Button_click_01"));
        sfxClips.Add(AudioEvent.START_GAME, loadClip("UI/UI_Button_startgame_01"));

        sfxClips.Add(AudioEvent.PLAYER_CAST, loadClip("SFX/plr_fishingrod_throw_01"));
        sfxClips.Add(AudioEvent.PLAYER_CATCH, loadClip("SFX/sfx_fishing_catch_01"));

        musicClips = new Dictionary<MusicType, AudioClip>();

        musicClips.Add(MusicType.MENU, loadClip("Music/just_ocean_noises"));

        Debug.Log("~~ Audio files loaded! ~~");
        hasLoaded = true;

    }

    public void PlayOneShot(AudioEvent ev)
    {
        AudioClip clip;
        if (!sfxClips.TryGetValue(ev, out clip))
        {
            Debug.Log("Clip not loaded for event: " + ev.ToString());
            return;
        }

        _audioSFX.PlayOneShot(clip);
    }

    public void PlayMusic(MusicType music)
    {
        AudioClip clip;
        if (!musicClips.TryGetValue(music, out clip))
        {
            Debug.Log("Clip not loaded for music: " + music.ToString());
            return;
        }

        _audioMusic.clip = clip;
        _audioMusic.loop = true;
        _audioMusic.Play();
    }

    private AudioClip loadClip(string name)
    {
        return (AudioClip)Resources.Load("Audio/" + name);
    }
}
