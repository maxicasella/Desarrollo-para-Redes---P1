using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Aura : NetworkBehaviour
{
    public NetworkMecanimAnimator myAnim;

    public override void Spawned()
    {
        this.gameObject.SetActive(false);
    }
    public void AuraOff()
    {
        myAnim.Animator.SetBool("AuraOff", true);
        this.gameObject.SetActive(false);
    }
}
