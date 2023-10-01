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

    void Start()
    {
        _runner = GetComponent<NetworkRunner>();

        //Crear sala
        var clientTask = InitializeGame(GameMode.Shared, SceneManager.GetActiveScene().buildIndex);
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
