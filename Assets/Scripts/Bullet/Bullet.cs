using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _lifetime;
    [SerializeField] float _dmg;

    [SerializeField] GameObject _explotion;

    float _time;
    NetworkRigidbody _rb;


    public override void Spawned()
    {
        _rb = GetComponent<NetworkRigidbody>();

    }
    public override void FixedUpdateNetwork()
    {
        _time += Time.deltaTime;

        Vector3 movement = transform.forward * _speed;
        _rb.Rigidbody.velocity = movement;

        if (_time >= _lifetime) Runner.Despawn(Object);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!Object || !Object.HasStateAuthority) return;

        if(other.TryGetComponent(out PlayerInputs enemy))
        {
            enemy.TakeDamage(_dmg);

            Runner.Despawn(Object);
        }

        Runner.Despawn(Object);
    }
}
