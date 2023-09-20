using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SpawnPoints : NetworkBehaviour
{
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] GameObject _flagPrefab;

    int _lastSpawnPoint;
    int _currentSpawnPoint;

    public NetworkBool isCaptured;
    
    public override void Spawned()
    {
        _currentSpawnPoint = Random.Range(0, _spawnPoints.Length);
        _lastSpawnPoint = _currentSpawnPoint;

        Runner.Spawn(_flagPrefab, _spawnPoints[_currentSpawnPoint].position, transform.rotation);
    }

    public override void FixedUpdateNetwork()
    {
        if(isCaptured) Spawn();
    }

    void Spawn()
    {
        _currentSpawnPoint = Random.Range(0, _spawnPoints.Length);

        if (_currentSpawnPoint == _lastSpawnPoint) return;

        _lastSpawnPoint = _currentSpawnPoint;
        Runner.Spawn(_flagPrefab, _spawnPoints[_currentSpawnPoint].position, transform.rotation);
    }
}