using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] float _movementSpeed;
    [SerializeField] Animator _myAnim;
    [SerializeField] Rigidbody _myRb;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(-horizontalInput, 0f, -verticalInput);
        movement.Normalize();

        _myRb.velocity = movement * _movementSpeed;

        if (movement.magnitude > 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360f);
        }

    }
}
