using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Aura : NetworkBehaviour
{
    public NetworkMecanimAnimator myAnim;
    [SerializeField] float _auraTimer;
    [SerializeField] float _auracooldown;

    public override void FixedUpdateNetwork()
    {
        _auraTimer += Time.deltaTime;
        if (_auraTimer >= _auracooldown)
        {
            AuraOff();
            Runner.Despawn(Object);
        }
    }
    public void AuraOff()
    {
        myAnim.Animator.SetBool("AuraOff", true);
    }
}
