using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private float angle = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.angle += Time.deltaTime * this.rotationSpeed;
        if(this.angle >= 360) this.angle = 0f;
        this.transform.localRotation = Quaternion.Euler(this.transform.localEulerAngles.x, this.angle, this.transform.localEulerAngles.z);
    }
}
