using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApocalipseZ;
using System;
using FishNet.Object;
using FishNet.Connection;

public interface IFastItems
{

    public event Action OnFastItemsAltered;
    void RemoveSlot(SSlotInventory slot);
    bool AddItem(int slotIndex, SSlotInventory slot);
    bool AddItem(SSlotInventory slot);
    void SlotChange(int switchSlotIndex);
    SSlotInventory GetFastItems(int id);
    void SetInventory(IContainer inventory);
    void MoveItem(int id, int v);

    void Clear();

    // fast ITEMS

    public void CmdGetFastItems(NetworkConnection sender = null);

    public void CmdMoveSlotFastItems(int idselecionado, int identer, NetworkConnection sender = null);

    public void CmdAddSlotFastItems(UISlotItemTemp slot, NetworkConnection sender = null);

    public void CmdRemoveSlotFastItems(UISlotItemTemp slot, NetworkConnection sender = null);

}
