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

    public NetworkBool isCaptured;

    public override void Spawned()
    {
        _currentSpawnPoint = Random.Range(0, _spawnPoints.Length);
        _lastSpawnPoint = _currentSpawnPoint;
        if (GameManager.Instance.CheckConnectedPlayers())
        {
            Runner.Spawn(_flagPrefab, _spawnPoints[_currentSpawnPoint].position, transform.rotation);
        }
        else isCaptured = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (!GameManager.Instance.CheckConnectedPlayers()) return;
        if(isCaptured) StartCoroutine(SpawnCorroutine());
    }

    public void Spawn()
    {
        RPC_Spawn();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_Spawn()
    {
        _currentSpawnPoint = Random.Range(0, _spawnPoints.Length);

        if (_currentSpawnPoint == _lastSpawnPoint) _currentSpawnPoint = Random.Range(0, _spawnPoints.Length);

        _lastSpawnPoint = _currentSpawnPoint;
        Runner.Spawn(_flagPrefab, _spawnPoints[_currentSpawnPoint].position, transform.rotation);
    }

    IEnumerator SpawnCorroutine()
    {
        Spawn();

        yield return new WaitForSeconds(0.05f);

        if(isCaptured) isCaptured = false;
    }
}
