using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    
    private float _currentSpawnDelay = 2.5f; // Used to track spawn spee/freq increasing over time
    private float _currentBaseSpeed = 2.0f;

    [SerializeField] private List<Human> spawnPrefabs;

    private float _lastSpawn;
    private bool isActive;

    [SerializeField] private BoxCollider _spawnBounds;

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
        Vector3 position = RandomPointInBounds();
        Human h = Instantiate(toSpawn, position, Quaternion.identity);

        h.preferredBait = Bait.GetRandomBait();
        h.speed = GetCurrentHumanSpeed();
    }

    private float GetCurrentHumanSpeed()
    {
        
        float offset = Random.Range(-0.5f, 0.5f);

        return _currentBaseSpeed + offset;
    }

    private Vector3 RandomPointInBounds()
    {
        return new Vector3(
            Random.Range(_spawnBounds.bounds.min.x, _spawnBounds.bounds.max.x),
            1.5f,
            Random.Range(_spawnBounds.bounds.min.z, _spawnBounds.bounds.max.z)
        );
    }
}
