using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{

    [SerializeField] private List<Transform> spawnLocs;
    
    private float _currentSpawnDelay = 2.5f; // Used to track spawn spee/freq increasing over time
    private float _currentBaseSpeed = 5.0f;

    [SerializeField] private List<Human> spawnPrefabs;

    private float _lastSpawn;
    private bool isActive;

    void Start()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        if (Time.time - _lastSpawn >= _currentSpawnDelay)
        {
            SpawnHuman();
        }
    }

    private void SpawnHuman()
    {
        _lastSpawn = Time.time;

        Human toSpawn = spawnPrefabs[Random.Range(0, spawnPrefabs.Count)];
        Vector3 position = spawnLocs[Random.Range(0, spawnLocs.Count)].position;
        Human h = Instantiate(toSpawn, position, Quaternion.identity);

        h.preferredBait = Bait.GetRandomBait();
        h.speed = GetCurrentHumanSpeed();
    }

    private float GetCurrentHumanSpeed()
    {
        
        float offset = Random.Range(-0.5f, 0.5f);

        return _currentBaseSpeed + offset;
    }
}
