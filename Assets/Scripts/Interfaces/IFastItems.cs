using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApocalipseZ;
using System;
using Mirror;
public interface IFastItems {

    public event Action OnFastItemsAltered;
    void RemoveSlot ( SSlotInventory slot );
    bool AddItem ( int slotIndex , SSlotInventory slot );
    bool AddItem ( SSlotInventory slot );
    void SlotChange ( int switchSlotIndex );
    SSlotInventory GetFastItems ( int id );
    void SetInventory ( IContainer inventory);
    void MoveItem ( int id , int v );
    InventoryTemp GetFastItemsTemp ( );
    void Clear ( );
   
    // fast ITEMS
    [Command]
    public void CmdGetFastItems ( NetworkConnectionToClient sender = null );
    [Command]
    public void CmdMoveSlotFastItems ( int idselecionado , int identer , NetworkConnectionToClient sender = null );
    [Command]
    public void CmdAddSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );
    [Command]
    public void CmdRemoveSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );

}
