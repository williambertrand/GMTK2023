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
    [SerializeField] private BoxCollider _movementBounds;

    [SerializeField] private int _maxHumanCount;
    private int _currentHumanCount;

    GamePlayManager _gameplayManager;

    void Start()
    {
        isActive = true;
        _gameplayManager = FindObjectOfType<GamePlayManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        if (
            _currentHumanCount < _maxHumanCount
            && Time.time - _lastSpawn >= _currentSpawnDelay
        ) {
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
        
        // TODO: TESTING - remove
        h.preferredBait = BaitType.HAMBURGER;
        h.RequiredCaptureCount = Random.Range(8, 15);
        h.movementBounds = _movementBounds;
        h.Spawner = this;

        h.transform.localScale = new Vector3(
            Random.Range(1.05f, 1.85f),
            Random.Range(1.05f, 1.85f),
            Random.Range(1.05f, 1.85f)
        );

        h.speed = GetCurrentHumanSpeed();
        if (_gameplayManager != null)
        {
            // Don't need this now that mini game transitions back to scene w/ score
            // h.onCaught += _gameplayManager.OnHumanCaught;
        }

        _currentHumanCount++;
        
    }

    public void OnHumanHooked(Human h)
    {
        _currentHumanCount -= 1;
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
            2.5f,
            Random.Range(_spawnBounds.bounds.min.z, _spawnBounds.bounds.max.z)
        );
    }
}
