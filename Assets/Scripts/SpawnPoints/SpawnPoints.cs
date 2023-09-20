using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SpawnPoints : NetworkBehaviour
{
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] GameObject _flagPrefab;

    int _lastSpawnPoint;

    public override void Spawned()
    {
        var currentSpawn = Random.Range(0, _spawnPoints.Length);

        _lastSpawnPoint = currentSpawn;

        Debug.Log(_lastSpawnPoint);
        Runner.Spawn(_flagPrefab, _spawnPoints[currentSpawn].position, transform.rotation);
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
    }
}
