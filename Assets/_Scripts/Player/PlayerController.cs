using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTutorial.Manager;

namespace UnityTutorial.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float AnimBlendSpeed = 0.5f;
        [SerializeField] private float MouseSensitivity = 10f;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float Bottomlimit = 70f;
        [SerializeField] private Transform CamHolder;
        [SerializeField] private Transform Camera;

        //Essential Constants
        private const float _runSpeed = 6f;
        private const float _walkSpeed = 2f;

        //Variables
        private Vector2 _currrentVelocity;
        private float _xRotation;
        private float _yRotation;
        private bool _hasAnimator;
        private Animator _animator;
        private int _xVelHash;
        private int _yVelHash;

        //Components
        private Rigidbody _playerRigidbody;
        private InputManager _inputManager;



        private void Start() {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigidbody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();


            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
        }


        private void FixedUpdate() 
        {
            Move();
        }

        private void LateUpdate() 
        {
            CamMovements();
        }

        private void Move()
        {
            if(!_hasAnimator) return;
            
            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if(_inputManager.Move == Vector2.zero) targetSpeed = 0.1f;

            _currrentVelocity.x = Mathf.Lerp(_currrentVelocity.x , _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            _currrentVelocity.y = Mathf.Lerp(_currrentVelocity.y , _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

            var xVelDifference = _currrentVelocity.x - _playerRigidbody.velocity.x;
            var zVelDifference = _currrentVelocity.y - _playerRigidbody.velocity.z;
            _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0 , zVelDifference)), ForceMode.VelocityChange);
            
            _animator.SetFloat(_xVelHash, _currrentVelocity.x);
            _animator.SetFloat(_yVelHash, _currrentVelocity.y);
        }

        private void CamMovements()
        {
            if(!_hasAnimator) return;

            var Mouse_X = _inputManager.Look.x;
            var Mouse_Y = _inputManager.Look.y;
            Camera.position = CamHolder.position;

            _xRotation -= Mouse_Y * MouseSensitivity * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit , Bottomlimit);

            Camera.localRotation = Quaternion.Euler(_xRotation, 0 , 0 );
            transform.Rotate(Vector3.up, Mouse_X * MouseSensitivity * Time.deltaTime);
        }

    }
}
