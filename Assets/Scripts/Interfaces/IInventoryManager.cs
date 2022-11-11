using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryManager
{
    void InventoryOpen();

    void InventoryClose();

    void SetFpsPlayer(IFpsPlayer player);
}