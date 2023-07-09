using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class DrummerController : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    public BeatScroller beatScroller;
    void Start(){
        this.animator = this.gameObject.GetComponent<Animator>();
        this.boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
    }

    public void Left(){
        this.animator.SetTrigger("LeftDrumm");
    }

    public void Right(){
        this.animator.SetTrigger("RightDrumm");
    }

    void OnTriggerEnter2D(Collider2D col){
        this.beatScroller.canMiss = true;
        this.boxCollider2D.enabled = false;
    }
}
