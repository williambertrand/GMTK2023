using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(AudioSource))]
public class FishReelSFX : MonoBehaviour
{
    public AudioClip reelIn;
    public AudioClip reelOut;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void ReelIn(){
        this.audioSource.Stop();
        this.audioSource.PlayOneShot(this.reelIn);
    }

    public void ReelOut(){
        this.audioSource.Stop();
        this.audioSource.PlayOneShot(this.reelOut);
    }
}
