using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
    [RequireComponent(typeof(CharacterController))]
    public class Moviment : MonoBehaviour
    {
        [Header("Moviment,Jump,croush,sprint")]
        public float Walk = 3f;
        public float Run = 5f;
        public float crouchSpeed = 0.4f;
        public float jumpSpeed = 3.5f;
        public float CrouchHeight = 0.5f;
        public bool IsGrounded;

        private float _terminalVelocity = 53.0f;
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
            mesh = transform.Find("Mesh/Ch35_nonPBR");
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
        public void MoveTick(MoveData md, float delta)
        {

            Move(md, delta);
            SoundStep.SetIsGround(isGrounded());
            SoundStep.SetIsMoviment(CheckMovement());
        }
        public void GravityJumpUpdate(bool IsJump, float delta)
        {
            CheckGround();

            if (IsGrounded)
            {

                if (PlayerVelocity.y < 0)
                {
                    PlayerVelocity.y = -2f;
                }

                if (IsJump)
                {
                    PlayerVelocity.y = Mathf.Sqrt(jumpSpeed * -2.0f * Physics.gravity.y);
                }
            }

            if (PlayerVelocity.y < _terminalVelocity)
            {
                PlayerVelocity.y += Physics.gravity.y * delta;
            }


        }
        public float GroundedRadius = 0.28f;
        public float GroundedOffset = -0.14f;
        public LayerMask GroundLayers;


        public void CheckGround()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            IsGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
        }
        public void Move(MoveData md, float delta)
        {
            if (!CharacterController.enabled)
            {
                return;
            }
            Vector3 moveDirection = Vector3.zero;
            moveDirection = new Vector3(md.Horizontal, 0, md.Forward);
            currentSpeed = Walk;
            currentSpeed = md.IsRun ? Run : currentSpeed;
            currentSpeed = md.IsCrouch ? crouchSpeed : currentSpeed;
            //SetCrouchHeight();
            transform.localRotation = Quaternion.Euler(0, md.RotationX, 0);
            CharacterController.Move(transform.TransformDirection(moveDirection) * currentSpeed * delta + new Vector3(0.0f, PlayerVelocity.y, 0.0f) * delta);

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
        public void SetIsGround(bool isgrounded)
        {
            IsGrounded = isgrounded;
        }
        public bool CheckIsRun()
        {
            return InputManager.GetRun();
        }
    }
}