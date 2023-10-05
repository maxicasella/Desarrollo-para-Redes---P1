using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Flags : NetworkBehaviour
{
    [SerializeField] int _score;
    [SerializeField] GameObject _captureParticles;
    SpawnPoints _spawner;

    [Networked(OnChanged = nameof(CapturedChanged))]

    bool _isCaptured { get; set; }

    public override void Spawned()
    {
        _spawner = FindObjectOfType<SpawnPoints>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        var player = other.GetComponent<PlayerInputs>();

        GameManager.Instance.AddScore(player, _score);
     
        _spawner.isCaptured = true;
        _isCaptured = true;
        Runner.Despawn(Object);
    }

    static void CapturedChanged(Changed<Flags> changed)
    {
        var update = changed.Behaviour._isCaptured = true;
        changed.LoadOld(); //Carga el valor anterior de la variable

        var old = changed.Behaviour._isCaptured;

        if (!old && update) changed.Behaviour.Runner.Spawn(changed.Behaviour._captureParticles, changed.Behaviour.transform.position, changed.Behaviour.transform.rotation); ;
    }



}
