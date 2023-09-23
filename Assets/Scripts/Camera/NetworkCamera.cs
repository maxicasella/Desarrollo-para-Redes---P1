using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkCamera : NetworkBehaviour
{
    [SerializeField] NetworkTransform _cameraAxis;
    [SerializeField] NetworkTransform _cameraTrack;
    [SerializeField] NetworkTransform _camera;

    [SerializeField] float _rotY;
    [SerializeField] float _rotX;
    [SerializeField] float _camRotSpeed;
    [SerializeField] float _minangle;
    [SerializeField] float _maxAngle;
    [SerializeField] float _cameraSpeed;

    Transform _playerTransform;

    public override void Spawned()
    {
        _playerTransform = this.transform;
        _camera = Camera.main.GetComponent<NetworkTransform>();
    }

    public override void FixedUpdateNetwork()
    {
        CameraLogic();
    }

    void CameraLogic()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float time = Time.deltaTime;

        _rotY = mouseY * time * _camRotSpeed;
        _rotX = mouseX * time * _camRotSpeed;

        _playerTransform.Rotate(0, _rotX, 0);

        _rotY = Mathf.Clamp(_rotY, _minangle, _maxAngle);

        Quaternion localRotation = Quaternion.Euler(-_rotY, 0, 0);
        _cameraAxis.Transform.localRotation = localRotation;

        _camera.Transform.position = Vector3.Lerp(_camera.Transform.position, _cameraTrack.Transform.position, _cameraSpeed * time);
        _camera.Transform.rotation = Quaternion.Lerp(_camera.Transform.rotation, _cameraTrack.Transform.rotation, _cameraSpeed * time);
    }
}
