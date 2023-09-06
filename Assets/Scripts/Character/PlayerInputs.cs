using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] float _movementSpeed;
    [SerializeField] float _jumpForce;
    [SerializeField] Animator _myAnim;
    [SerializeField] Rigidbody _myRb;

    float _horizontalInput;
    float _verticalInput;

    bool _isWalking;

    private void Start()
    {
        _isWalking = false;
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        if (_isWalking) _myAnim.SetBool("Run", true);
        else _myAnim.SetBool("Run", false);
        //if (Input.GetKeyDown(KeyCode.Space)) Jump();

    }
    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 movement = new Vector3(-_horizontalInput, 0f, -_verticalInput);
        movement.Normalize();

        _myRb.velocity = movement * _movementSpeed;

        if (movement.magnitude > 0)
        {
            _isWalking = true;
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360f);
        }
        else _isWalking = false;
    }

    void Jump()
    {
        _myRb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }
}
