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

    NetworkRigidbody _rb;

    void Start()
    {
        _rb = GetComponent<NetworkRigidbody>();
        Destroy(gameObject, _lifetime);
    }

    public override void FixedUpdateNetwork()
    {
        Vector3 movement = transform.forward * _speed;
        _rb.Rigidbody.velocity = movement;
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
