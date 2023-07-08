using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class ScapeZone : MonoBehaviour
{
    public UnityEvent whenTouching;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        this.boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
        this.rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();

        this.boxCollider2D.isTrigger = true;
        this.rigidbody2D.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col){
        GameObject.Destroy(col.gameObject);
        this.whenTouching.Invoke();
    }
}
