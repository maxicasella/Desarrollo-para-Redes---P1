using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class LocalAmmo : NetworkBehaviour
{
    WeaponController _weapons;
    [SerializeField] Text _myTxt;

    public override void Spawned()
    {
        foreach (var weapons in GameManager.Instance.Weapons())
        {
            if (weapons.HasInputAuthority) _weapons = weapons;
        }
    }

    public override void FixedUpdateNetwork()
    {
        _myTxt.text = _weapons.currentAmmo.ToString();
    }
}
