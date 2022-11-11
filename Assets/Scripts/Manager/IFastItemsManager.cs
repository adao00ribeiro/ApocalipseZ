using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public interface IFastItemsManager
{
    void CmdSlotChange ( int slotIndex , NetworkConnectionToClient sender = null );
    void SetFpsPlayer ( IFpsPlayer _player );
}