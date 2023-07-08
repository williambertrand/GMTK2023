using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeatScroller : MonoBehaviour
{
    public float BPM = 128f;
    public SongLevel songLevel = SongLevel.Easy;
    public float startDelay = 2f;
    public float fadeInDuration = 1f;

    public AudioClip song;
    public AudioClip humanCatch;
    public AudioClip lost;
    public AudioClip reel;

    [Header("Notes")]
    public float noteSpeed = 1f;
    public float notesPerSecond = 1f;

    [Header("Tugging")]
    public GameObject fisherman;
    public float pullForce = 2f;
    public float scapeForce = 1f;

    [Header("Prefabs")]
    public GameObject leftArrow;
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject rightArrow;

    [Header("Spawn initial position")]
    public Transform leftArrowPosition;
    public Transform upArrowPosition;
    public Transform downArrowPosition;
    public Transform rightArrowPosition;

    [Header("Buttons")]
    public GameObject leftArrowButton;
    public GameObject upArrowButton;
    public GameObject downArrowButton;
    public GameObject rightArrowButton;

    private AudioSource source;
    private AudioSource reelSource;

    private float startTime = 0f;
    private float lastSpawn = 0f;

    private bool letFinish = false; 
    

    // Start is called before the first frame update
    void Start()
    {
        this.startTime = Time.time;
        this.source = this.gameObject.GetComponent<AudioSource>();
        this.source.clip = this.song;
        this.source.playOnAwake = false;
        this.source.volume = 0;


        switch (this.songLevel)
        {
            case SongLevel.Easy:
                this.downArrowButton.SetActive(false);
                this.upArrowButton.SetActive(false);
            break;
            case SongLevel.Normal:
                this.downArrowButton.SetActive(false);
                break;
        }
    }

    public void NoteMissed()
    {
        this.fisherman.transform.position += new Vector3(1f * this.scapeForce, 0f, 0f);
    }

    public void NoteHit()
    {
        this.fisherman.transform.position -= new Vector3(1f * this.pullForce, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.lastSpawn + 1f/this.notesPerSecond < Time.time)
        {
            this.lastSpawn = Time.time;

            var arrow = SpawnArrow();
            arrow.transform.parent = this.transform.parent;
            
            arrow.GetComponent<NoteController>().beatTempo = this.BPM / 60f;
        }

        if (Time.time > this.startTime + this.startDelay)
        {
            StartCoroutine(StartFade(this.source, this.fadeInDuration, 1));
        }
    }

    private GameObject SpawnArrow()
    {
        GameObject arrow = null;
        

        int max = this.songLevel == SongLevel.Hard ? 4 : (this.songLevel == SongLevel.Normal ? 3 : 2);

        var i = UnityEngine.Random.Range(0, max);
        
        if (i == 0)
        {
            arrow = GameObject.Instantiate(this.leftArrow);
            arrow.transform.position = this.leftArrowPosition.position;
        }
        else if (i == 1)
        {
            
            arrow = GameObject.Instantiate(this.rightArrow);
            arrow.transform.position = this.rightArrowPosition.position;
        }
        else if (i == 2)
        {
            arrow = GameObject.Instantiate(this.downArrow);
            arrow.transform.position = this.downArrowPosition.position;
        }
        else if (i == 3)
        {
            arrow = GameObject.Instantiate(this.upArrow);
            arrow.transform.position = this.upArrowPosition.position;
        }
        return arrow;
    }

    public void Won()
    {
        this.source.clip = this.humanCatch;
        this.letFinish = true;
    }

    public void Lost()
    {
        this.source.clip = this.humanCatch;
        this.letFinish = true;
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}

public enum SongLevel
{
    Easy = 3,
    Normal = 2,
    Hard = 1
}

