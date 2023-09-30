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
        if (Object.HasStateAuthority) _weapons = FindObjectOfType<WeaponController>();
    }

    public override void FixedUpdateNetwork()
    {
        _myTxt.text = _weapons.currentAmmo.ToString();
    }
}
