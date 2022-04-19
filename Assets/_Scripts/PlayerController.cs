using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTutorial.Manager;

namespace UnityTutorial.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        private float RunSpeed = 6f;
        private float WalkSpeed = 2f;
        private Vector2 currrentVelocity;
        [SerializeField] private float AnimBlendSpeed = 0.5f;
        [SerializeField] private float MouseSensitivity = 10f;
        private float xRotation;
        private float yRotation;
        [SerializeField] private float upperLimit;
        [SerializeField] private float bottomlimit;
        [SerializeField] private Transform CamHolder;
        [SerializeField] private Transform FPV;


        private bool hasAnimator;
        private Animator animator;

        private int X_VelHash;
        private int Y_VelHash;

        
        private Rigidbody playerRigidbody;
        private InputManager inputManager;



        private void Start() {
            hasAnimator = TryGetComponent<Animator>(out animator);
            playerRigidbody = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();


            X_VelHash = Animator.StringToHash("X_Velocity");
            Y_VelHash = Animator.StringToHash("Y_Velocity");
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
            if(!hasAnimator) return;
            
            float targetSpeed = inputManager.run ? RunSpeed : WalkSpeed;
            if(inputManager.move == Vector2.zero) targetSpeed = 0.1f;

            currrentVelocity.x = Mathf.Lerp(currrentVelocity.x , inputManager.move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            currrentVelocity.y = Mathf.Lerp(currrentVelocity.y , inputManager.move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

            
            var xVelDifference = currrentVelocity.x - playerRigidbody.velocity.x;
            var zVelDifference = currrentVelocity.y - playerRigidbody.velocity.z;

            playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0 , zVelDifference)), ForceMode.VelocityChange);
            

            animator.SetFloat(X_VelHash, currrentVelocity.x);
            animator.SetFloat(Y_VelHash, currrentVelocity.y);
        }

        private void CamMovements()
        {
            var Mouse_X = inputManager.look.x;
            var Mouse_Y = inputManager.look.y;
            FPV.position = CamHolder.position;


            xRotation -= Mouse_Y * MouseSensitivity * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, upperLimit , bottomlimit);

            FPV.localRotation = Quaternion.Euler(xRotation, 0 , 0 );
            transform.Rotate(Vector3.up, Mouse_X * MouseSensitivity * Time.deltaTime);
        }

    }
}
