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

    public bool canMiss = false;

    public FishReelSFX fishReelSFX;

    public GameObject bait;

    [Header("Songs")]
    public AudioClip song;
    public AudioClip victory;
    public AudioClip lost;

    [Header("Notes")]
    public float noteSpeed = 1f;
    public float notesPerSecond = 1f;

    [Header("Tugging")]
    public GameObject fisherman;
    public float pullForce = 2f;
    public float scapeForce = 1f;

    [Header("Prefabs")]
    public GameObject leftArrow;
    public GameObject rightArrow;

    [Header("Spawn initial position")]
    public Transform leftArrowPosition;
    public Transform rightArrowPosition;

    [Header("Buttons")]
    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    private AudioSource source;
    private AudioSource reelSource;

    private float startTime = 0f;
    private float lastSpawn = 0f;

    private bool letFinish = false;
    private bool hasWon = false;

    [Header("Transitions")]
    public GameObject lostTransition;
    public GameObject victoryTransition;


    // Start is called before the first frame update
    void Start()
    {
        this.startTime = Time.time;
        this.source = this.gameObject.GetComponent<AudioSource>();
        this.source.clip = this.song;
        this.source.playOnAwake = false;
        this.source.volume = 0;


    }

    public void NoteMissed()
    {
        if (this.fisherman && this.canMiss){
            this.fisherman.transform.position += new Vector3(1f * this.scapeForce, 0f, 0f);
            this.fishReelSFX.ReelOut();
        }
    }

    public void NoteHit()
    {
        if (this.fisherman){
            this.fisherman.transform.position -= new Vector3(1f * this.pullForce, 0f, 0f);
            this.fishReelSFX.ReelIn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (letFinish)
        {
            if (!this.source.isPlaying)
            {
                GameObject.Find("Minigame scene manager").GetComponent<MinigameSceneController>().Finish(this.hasWon);
            }
        }
        else
        {


            if (Time.time > this.startTime + this.startDelay)
            {
                StartCoroutine(StartFade(this.source, this.fadeInDuration, 1));

                if (this.lastSpawn + 1f / (this.notesPerSecond * ((int)this.songLevel) / 2f) < Time.time)
                {
                    this.lastSpawn = Time.time;

                    var arrow = SpawnArrow();
                    if (arrow)
                    {
                        arrow.transform.parent = this.transform.parent;

                        arrow.GetComponent<NoteController>().beatTempo = this.BPM / 60f * this.noteSpeed * (int)this.songLevel;
                    }
                }
            }
        }
    }

    private GameObject SpawnArrow()
    {
        GameObject arrow = null;

        var i = UnityEngine.Random.Range(0, 3);

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
        return arrow;
    }

    public void Won()
    {
        this.source.clip = this.victory;
        this.victoryTransition.SetActive(true);
        this.source.Play();
        this.letFinish = true;
        this.hasWon = true;
    }

    public void Lost()
    {
        this.source.clip = this.lost;
        this.lostTransition.SetActive(true);
        this.source.Play();
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
    Easy = 1,
    Normal = 2,
    Hard = 5
}

