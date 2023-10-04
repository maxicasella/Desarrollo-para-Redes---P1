using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fusion;
using Fusion.Sockets;
using Cinemachine;

public class SpawnNetworkPlayer : MonoBehaviour, INetworkRunnerCallbacks
{
    NetworkRunner _runner;

    [SerializeField] NetworkPlayer _playerPrefab;

    [SerializeField] Transform[] spawnPoints;

    bool _startGame = false;
    [Networked] int _spawn {get; set;}

    SharedMode.NetworkCharacterController _characterController;

    void Start()
    {
       _runner = GetComponent<NetworkRunner>();
    }

    void Update()
    {
        if (_startGame) return;
        StartGameOk();
    }

    public void OnConnectedToServer(NetworkRunner runner) //Spawn del Player
    {
        Debug.Log(_spawn);
        if (runner.Topology == SimulationConfig.Topologies.Shared)
        {
            if(_spawn == 1)
            {
                var localPlayer = runner.Spawn(_playerPrefab, spawnPoints[0].position, spawnPoints[0].rotation, runner.LocalPlayer);
                _characterController = localPlayer.GetComponent<SharedMode.NetworkCharacterController>();
                var localCamera = _characterController.GetComponentInChildren<CinemachineFreeLook>();
                localCamera.Priority = 2;
      
            }
            else
            {
                var localPlayer = runner.Spawn(_playerPrefab, spawnPoints[1].position, spawnPoints[1].rotation, runner.LocalPlayer);
                _characterController = localPlayer.GetComponent<SharedMode.NetworkCharacterController>();
                var localCamera = _characterController.GetComponentInChildren<CinemachineFreeLook>();
                localCamera.Priority = 2;
                _spawn = 1;
            }
        }
        Debug.Log(_spawn);
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) //Inputs
    {
        if (!NetworkPlayer.Local || !_characterController) return;

        input.Set(_characterController.GetLocalInputs()); //Si lo tengo, llamo al metodo para obtener los inputs
    }

    bool StartGameOk()
    {
        if (GameManager.Instance.CheckConnectedPlayers())
        {
            _runner.ProvideInput = true;
            
            return _startGame = true; 
        }
        else return _runner.ProvideInput = false;
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

