using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTL : MonoBehaviour
{
    [SerializeField] private float timeToLive = 5;
    private float spawnTime;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - spawnTime > timeToLive)
        {
            Destroy(gameObject);
        }
    }
}
