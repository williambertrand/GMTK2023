using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public BeatScroller beatScroller;
    // Start is called before the first frame update
    public Sprite defaultImage;
    public Sprite pressedImage;

    private GameObject note = null;

    void Start()
    {
        this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

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
            this.spriteRenderer.sprite = this.pressedImage;
            if(this.note){
                this.beatScroller.NoteHit();
                GameObject.Destroy(this.note);
            }
            else{
                this.beatScroller.NoteMissed();
            }
        }
        else if (context.canceled)
        {
            this.spriteRenderer.sprite = this.defaultImage;
        }
    }
}
