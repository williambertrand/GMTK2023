using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    public Sprite defaultImage;
    public Sprite pressedImage;
    void Start()
    {
       this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPressed(InputAction.CallbackContext context){
       if(context.performed){
           this.spriteRenderer.sprite = this.pressedImage;
       }
       else if(context.canceled){
           this.spriteRenderer.sprite = this.defaultImage;
       }
    }
}
