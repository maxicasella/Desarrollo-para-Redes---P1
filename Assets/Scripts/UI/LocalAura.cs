using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class LocalAura : NetworkBehaviour
{
    PlayerInputs _player;
    [SerializeField] Image _auraFill;

    public override void Spawned()
    {
        foreach (var players in GameManager.Instance.Players())
        {
            if (players.HasInputAuthority) _player = players;
        }
    }

    public override void FixedUpdateNetwork()
    {
        UpdateAura();
    }

    void UpdateAura()
    {
        var amount = _player.AuraFill();
        _auraFill.fillAmount = amount;
    }

}
