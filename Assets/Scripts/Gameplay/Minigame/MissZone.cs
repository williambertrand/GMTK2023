using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MissZone : MonoBehaviour
{
    public BeatScroller beatScroller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col){
        beatScroller.NoteMissed();
        GameObject.Destroy(col.gameObject);
    }
}
