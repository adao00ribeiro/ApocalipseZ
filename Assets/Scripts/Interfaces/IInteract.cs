using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public interface IInteract
{

    void CmdInteract ( NetworkConnectionToClient sender = null );
    
    void OnInteract ( IFpsPlayer player );

    void StartFocus ( );

    void EndFocus ( );
    string GetTitle ( );
}
