using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DrummerController : MonoBehaviour
{
    private Animator animator;

    void Start(){
        this.animator = this.gameObject.GetComponent<Animator>();
    }

    public void Left(){
        this.animator.SetTrigger("LeftDrumm");
    }

    public void Right(){
        this.animator.SetTrigger("RightDrumm");
    }
}
