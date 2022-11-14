using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
public interface IFastItemsManager
{
    void CmdSlotChange(int slotIndex, NetworkConnection sender = null);
    void SetFpsPlayer(IFpsPlayer _player);
}