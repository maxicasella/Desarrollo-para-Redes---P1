using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents: MonoBehaviour
{
    [SerializeField] Animator _myAnim;
    [SerializeField] WeaponController _weapons;
    public void ShootFinish()
    {
        _myAnim.SetBool("Shoot", false);
    }

    public void Reload()
    {
        _weapons.ReloadAmmo();
    }
}
