using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }

    //Funciona como el Awake, solo que seria al instanciarse en la red
    public override void Spawned()
    {
        if (Object.HasInputAuthority) Local = this; //Solo es true en el equipo que ejecuta el juego
    }
}
