using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoviment
{
    void Move ( );
    void Jump ( );
    bool CheckMovement ( );
    bool isGrounded ( );
    bool CheckIsRun ( );
    void UpdateMoviment ( );

    public void EnableCharacterController ( );
    public void DisableCharacterController ( );
}
