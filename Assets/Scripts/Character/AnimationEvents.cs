using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents: MonoBehaviour
{
    [SerializeField] Animator _myAnim;
    [SerializeField] WeaponController _weapons;
    [SerializeField] PlayerInputs _player;
    public void ShootFinish()
    {
        _myAnim.SetBool("Shoot", false);
        _player.shootParticles.SetActive(false);
    }

    public void Reload()
    {
        _weapons.ReloadAmmo();
    }

}
