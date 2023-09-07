using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents: MonoBehaviour
{
    [SerializeField] Animator _myAnim;
    public void ShootFinish()
    {
        _myAnim.SetBool("Shoot", false);
    }
}
