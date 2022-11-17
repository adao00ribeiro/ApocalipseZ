using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public interface IMoviment
    {

        void Move(MoveData moveData, float delta);
        void Jump(MoveData moveData, float delta);
        bool CheckMovement();
        bool isGrounded();

        void SetIsGround(bool isgrounded);
        void CheckGround();
        bool CheckIsRun();
        void MoveTick(MoveData moveData, float delta);
        void GravityJumpUpdate(MoveData moveData, float delta);

        public void EnableCharacterController();
        public void DisableCharacterController();
    }
}