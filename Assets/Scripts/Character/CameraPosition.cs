using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _distance;
    [SerializeField] Vector2 _rotationAngle = new Vector2(90 * Mathf.Deg2Rad,0);
    [SerializeField] Vector2 _sensitivity;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float hor = Input.GetAxis("Mouse X");

        if (hor != 0) _rotationAngle.x += hor * Mathf.Deg2Rad * _sensitivity.x;

        float ver = Input.GetAxis("Mouse Y");

        if (hor != 0)
        {
            _rotationAngle.y += ver * Mathf.Deg2Rad * _sensitivity.y;
            _rotationAngle.y = Mathf.Clamp(_rotationAngle.y, -50 * Mathf.Deg2Rad, 50 * Mathf.Deg2Rad); //Limito Y
        }
    }

    void LateUpdate()
    {
        Vector3 direction = new Vector3(Mathf.Cos(_rotationAngle.x) * -Mathf.Cos(_rotationAngle.y), -Mathf.Sin(_rotationAngle.y), Mathf.Sin(_rotationAngle.x) * Mathf.Cos(_rotationAngle.y));

        RaycastHit hit;
        var newDistance = _distance;
        if(Physics.Raycast(_target.position, direction, out hit, _distance))
        {
            newDistance = (hit.point - _target.position).magnitude;
        }

        transform.position = _target.position + direction * _distance;
        transform.rotation = Quaternion.LookRotation(_target.position - transform.position);
    }
}
