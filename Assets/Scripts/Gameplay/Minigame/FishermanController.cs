using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LineRenderer))]
public class FishermanController : MonoBehaviour
{
    public Transform victoryPosition;
    public Transform baitPosition;
    public Transform sensorPosition;
    public Transform fishingPolePosition;
    private AudioSource audioSource;
    private LineRenderer lineRenderer;

    public AudioClip farOff;

    [Range(0, 100)]
    public float gettingNearPct;
    public AudioClip gettingNear;

    [Range(0, 100)]
    public float almostCatchPct;
    public AudioClip almostCatch;
    private float startingDistance;

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        this.audioSource.clip = this.farOff;
        this.audioSource.Stop();
        this.audioSource.loop = false;

        this.startingDistance = Vector3.Distance(this.sensorPosition.position, this.victoryPosition.position);
        
        this.lineRenderer = this.gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.lineRenderer.SetPosition(0, this.baitPosition.position);
        this.lineRenderer.SetPosition(1, this.fishingPolePosition.position);
    }

    public void Hit()
    {
        float currentPct = Vector3.Distance(this.victoryPosition.position, this.sensorPosition.position) * 100 / this.startingDistance;
        
        if (currentPct < this.gettingNearPct)
        {
            if (currentPct < this.almostCatchPct)
            {
                this.audioSource.clip = this.almostCatch;
                this.audioSource.Play();
            }
            else
            {
                this.audioSource.clip = this.gettingNear;
                this.audioSource.Play();
            }
        }
        else
        {
            this.audioSource.clip = this.farOff;
            this.audioSource.Play();
        }
    }
}
