using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace ApocalipseZ
{
    [RequireComponent(typeof(CharacterController))]
    public class Moviment : MonoBehaviour, IMoviment
    {
        [Header("Moviment,Jump,croush,sprint")]
        public float Walk = 3f;
        public float Run = 5f;
        public float crouchSpeed = 0.4f;
        public float jumpSpeed = 3.5f;
        public float CrouchHeight = 0.5f;
        public bool IsGrounded;

        public Vector3 PlayerVelocity;

        private float currentSpeed;
        private Transform mesh;

        private CharacterController CharacterController;

        private InputManager InputManager;
        private SoundStep SoundStep;


        Transform CameraTransform;
        private void Awake()
        {
            InputManager = GameController.Instance.InputManager;
            mesh = transform.Find("Ch35_nonPBR");
            CharacterController = GetComponent<CharacterController>();
            CameraTransform = transform.Find("Camera & Recoil");
            SoundStep = GetComponent<SoundStep>();
            currentSpeed = Walk;
        }

        public void EnableCharacterController()
        {
            CharacterController.enabled = true;
        }
        public void DisableCharacterController()
        {
            CharacterController.enabled = false;
        }
        public void UpdateMoviment()
        {
            IsGrounded = CharacterController.isGrounded;
            Move();
            Gravity();
            Jump();
            SoundStep.SetIsGround(isGrounded());
            SoundStep.SetIsMoviment(CheckMovement());
        }
        public void Move()
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection = new Vector3(InputManager.GetMoviment().x, 0, InputManager.GetMoviment().y);
            currentSpeed = Walk;
            currentSpeed = InputManager.GetRun() ? Run : currentSpeed;
            currentSpeed = InputManager.GetCrouch() ? crouchSpeed : currentSpeed;
            SetCrouchHeight();
            CharacterController.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);

        }
        public void Gravity()
        {
            PlayerVelocity.y += Physics.gravity.y * Time.deltaTime;
            if (IsGrounded && PlayerVelocity.y < 0)
            {
                PlayerVelocity.y = -2f;
            }
            CharacterController.Move(PlayerVelocity * Time.deltaTime);
        }
        public void Jump()
        {
            if (InputManager.GetIsJump() && isGrounded())
            {
                PlayerVelocity.y = Mathf.Sqrt(jumpSpeed * -3.0f * Physics.gravity.y);
            }
        }
        public void SetCrouchHeight()
        {
            CharacterController.height = InputManager.GetCrouch() ? CrouchHeight : 1.8f;
            mesh.localPosition = InputManager.GetCrouch() ? new Vector3(0, 0.4f, 0) : new Vector3(0, 0, 0);

        }
        public bool CheckMovement()
        {
            if (InputManager.GetMoviment().x > 0 || InputManager.GetMoviment().x < 0 || InputManager.GetMoviment().y > 0 || InputManager.GetMoviment().y < 0)
            {
                return true;
            }
            return false;
        }
        public bool isGrounded()
        {
            return IsGrounded;
        }

        public bool CheckIsRun()
        {
            return InputManager.GetRun();
        }
    }
}