using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class LocalLifeBar : NetworkBehaviour
{
    PlayerInputs _player;
    [SerializeField] Text _myTxt;

    public override void Spawned()
    {
        if(Object.HasStateAuthority) _player = FindObjectOfType<PlayerInputs>();
    }

    public override void FixedUpdateNetwork()
    {
        _myTxt.text = _player.LocalLife().ToString("00");
    }

}
