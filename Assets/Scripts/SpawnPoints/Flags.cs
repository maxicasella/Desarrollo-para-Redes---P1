using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Flags : NetworkBehaviour
{
    [SerializeField] int _score;
    [SerializeField] GameObject _captureParticles;
    SpawnPoints _spawner;

    public override void Spawned()
    {
        _spawner = FindObjectOfType<SpawnPoints>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        //Sumar puntos al GM

        Runner.Spawn(_captureParticles, this.transform.position,transform.rotation);

        _spawner.isCaptured = true;

        Runner.Despawn(Object);
    }
}
