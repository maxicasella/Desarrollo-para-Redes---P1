using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SpawnPoints : NetworkBehaviour
{

    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] GameObject _flagPrefab;

    [Networked] int _lastSpawnPoint { get; set; }
    [Networked] int _currentSpawnPoint { get; set; }

    [Networked(OnChanged = nameof(OnCaptured))] public bool isCaptured { get; set; }

    public override void Spawned()
    {
        if (GameManager.Instance.CheckConnectedPlayers()) isCaptured = false;
        else isCaptured = true;
    }

    public void Spawn()
    {
        if (!HasStateAuthority) return;

        _currentSpawnPoint = Random.Range(0, _spawnPoints.Length);

        if (_currentSpawnPoint == _lastSpawnPoint) _currentSpawnPoint = Random.Range(0, _spawnPoints.Length);

        _lastSpawnPoint = _currentSpawnPoint;
        Runner.Spawn(_flagPrefab, _spawnPoints[_currentSpawnPoint].position, transform.rotation);
    }

    void StartSpawn()
    {
        StartCoroutine(SpawnCorroutine());
    }

    IEnumerator SpawnCorroutine()
    {
        Spawn();

        yield return new WaitForSeconds(0.01f);

        if (isCaptured) isCaptured = false;
    }
    static void OnCaptured(Changed<SpawnPoints> changed)
    {
        var updateBool = changed.Behaviour.isCaptured = false;
        changed.LoadOld();

        var oldBool = changed.Behaviour.isCaptured;

        if (oldBool) changed.Behaviour.StartSpawn();
    }
}
