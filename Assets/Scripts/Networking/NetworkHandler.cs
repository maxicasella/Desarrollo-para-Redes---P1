using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(NetworkRunner))]
[RequireComponent (typeof(NetworkSceneManagerDefault))]
public class NetworkHandler : MonoBehaviour
{
    NetworkRunner _runner;
    bool _startGame = false;

    void Start()
    {
        _runner = GetComponent<NetworkRunner>();

        //Crear sala
        var clientTask = InitializeGame(GameMode.Shared, SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        if (_startGame) return;
        StartGameOk();
    }

    bool StartGameOk()
    {
        if (GameManager.Instance.CheckConnectedPlayers())
        {
            _runner.ProvideInput = true; //Avisamos al runner que comienza a tomar Inputs
            return _startGame = true;
        }
        else return _runner.ProvideInput = false;
    }

    Task InitializeGame(GameMode gameMode, SceneRef sceneToLoad) //Creamos la sesion
    {
        var sceneManager = GetComponent<NetworkSceneManagerDefault>();
   
        return _runner.StartGame(new StartGameArgs() //Le podemos indicar parametros que posee StartGameArgs
        {
            GameMode = gameMode,
            Scene = sceneToLoad,
            SessionName = "SessionName",
            SceneManager = sceneManager,
            PlayerCount = 2
        });
    }


}
