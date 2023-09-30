using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class WeaponController : NetworkBehaviour
{

    [SerializeField] float _maxAmmo;
    public float currentAmmo;
    [SerializeField] NetworkMecanimAnimator _myAnim;
    [SerializeField] AudioSource _reloadAudio;

    public override void Spawned()
    {
        GameManager.Instance.AddWeapons(this);
        currentAmmo = _maxAmmo;
    }

    public override void FixedUpdateNetwork()
    {
        if (currentAmmo <= 0)
        {
            _myAnim.Animator.SetTrigger("Reload");
        }
    }

    public void ReloadAmmo()
    {
        _reloadAudio.Play();
        currentAmmo = _maxAmmo;
    }

    public void UpdateAmo()
    {
        currentAmmo--;
    }
}
