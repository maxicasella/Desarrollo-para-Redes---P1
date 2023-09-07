using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _lifetime;
    [SerializeField] GameObject _explotion;

    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject, _lifetime);
    }

    private void FixedUpdate()
    {
        Vector3 movement = transform.forward * _speed;
        _rb.velocity = movement;
    }
}
