using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SharedMode
{
    public class NetworkCharacterController : MonoBehaviour
    {
        NetworkInputsData _networkInputs;

        bool _isFirePressed;
        bool _isJumpPressed;
        bool _isReloadPressed;

        void Start()
        {
            _networkInputs = new NetworkInputsData();
        }

        void Update()
        {
            _networkInputs.xMovement = Input.GetAxis("Vertical");
            _networkInputs.yMovement = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Mouse0)) _isFirePressed = true;
            if (Input.GetKeyDown(KeyCode.Space)) _isJumpPressed = true;
            if (Input.GetKeyDown(KeyCode.R)) _isReloadPressed = true;
        }

        public NetworkInputsData GetLocalInputs()
        {
            _networkInputs.isFiring = _isFirePressed;
            _isFirePressed = false;

            _networkInputs.isJumping = _isJumpPressed;
            _isJumpPressed = false;

            _networkInputs.isReloading = _isReloadPressed;
            _isReloadPressed = false;

            return _networkInputs;
        }
    }
}
