using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetworkInputsData : INetworkInput
{
    public float xMovement;
    public float yMovement;
    public NetworkBool isFiring;
    public NetworkBool isJumping;
    public NetworkBool isReloading;
    public NetworkBool auraOn;
}
