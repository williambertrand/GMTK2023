using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class NoteController : MonoBehaviour
{
    
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    private Animator animator;

    public float beatTempo = 1;
    void Start()
    {
        this.boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
        this.rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();

        this.rigidbody2D.isKinematic = true;
        this.boxCollider2D.isTrigger = true; 

        this.animator = this.gameObject.GetComponent<Animator>();
    }

    void Update(){
        this.transform.position += new Vector3(0f, this.beatTempo * Time.deltaTime, 0f);
    }
}