using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class SpawnNetworkPlayer : MonoBehaviour, INetworkRunnerCallbacks
{
    NetworkRunner _runner;

    [SerializeField] NetworkPlayer _playerPrefab;

    [SerializeField] Transform[] spawnPoints;

    bool _startGame = false;

    SharedMode.NetworkCharacterController _characterController;

    void Start()
    {
        _runner = GetComponent<NetworkRunner>();
    }

    //void Update()
    //{
    //    if (_startGame) return;
    //    StartGameOk();
    //}

    public void OnConnectedToServer(NetworkRunner runner) //Spawn del Player
    {
        if (runner.Topology == SimulationConfig.Topologies.Shared)
        {
            Transform spawnPoint = GetRandomSpawnPoint();

            var localPlayer = runner.Spawn(_playerPrefab, spawnPoint.position, spawnPoint.rotation, runner.LocalPlayer);
            _characterController = localPlayer.GetComponent<SharedMode.NetworkCharacterController>();
        }
                
    }

    bool StartGameOk()
    {
        if (GameManager.Instance.CheckConnectedPlayers())
        {
            _runner.ProvideInput = true;
            
            return _startGame = true; //Avisamos al runner que comienza a tomar Inputs
        }
        else return _runner.ProvideInput = false;
    }

    Transform GetRandomSpawnPoint()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex];
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) //Inputs
    {
        if (!NetworkPlayer.Local || !_characterController) return;

        input.Set(_characterController.GetLocalInputs()); //Si lo tengo, llamo al metodo para obtener los inputs
    }

    #region Callbacks sin usar

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    #endregion
}

