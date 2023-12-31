using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class ButtonController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public BeatScroller beatScroller;
    // Start is called before the first frame update
    public Sprite defaultImage;
    public Sprite pressedImage;

    public Animator fishAnimator;

    public UnityEvent whenHit;
    private GameObject note = null;
    public FishermanController fishermanController;

    public KeyCode keycode;
    public KeyCode keycodeAlt;

    void Start()
    {
        this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(this.keycode) || Input.GetKeyDown(this.keycodeAlt))
        {
            this.whenHit.Invoke();
            if (this.note)
            {
                this.beatScroller.NoteHit();
                GameObject.Destroy(this.note);
                this.fishAnimator.SetTrigger("Reel");
                if (this.fishermanController)
                    this.fishermanController.Hit();
            }
            else
            {
                this.beatScroller.NoteMissed();
            }
        }
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        this.note = col.gameObject;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        this.note = null;
    }

    public void OnPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            this.whenHit.Invoke();
            this.spriteRenderer.sprite = this.pressedImage;
            if (this.note)
            {
                this.beatScroller.NoteHit();
                GameObject.Destroy(this.note);
                this.fishAnimator.SetTrigger("Reel");
                if (this.fishermanController)
                    this.fishermanController.Hit();
            }
            else
            {
                this.beatScroller.NoteMissed();
            }

        }
        else if (context.canceled)
        {
            this.spriteRenderer.sprite = this.defaultImage;
        }
    }
}
