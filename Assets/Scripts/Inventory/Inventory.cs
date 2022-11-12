using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using ApocalipseZ;

public class Inventory : NetworkBehaviour
{
    private UiInventory uiInventory;
    public SyncList<SlotInventoryTemp> inventory = new SyncList<SlotInventoryTemp>();
    public List<ListItemsInspector> ListInspector = new List<ListItemsInspector>();
    [SerializeField] private int maxSlot = 6;

    public bool teste;
    public override void OnStartClient()
    {
        for (int i = 0; i < maxSlot; i++)
        {
            inventory.Add(new SlotInventoryTemp());
        }
        uiInventory = GameController.Instance.CanvasFpsPlayer.GetUiInventory();
        uiInventory.SetInventory(this);
        uiInventory.AddSlots();
        inventory.Callback += OnInventoryUpdated;
        // Process initial SyncList payload
        for (int index = 0; index < inventory.Count; index++)
            OnInventoryUpdated(SyncList<SlotInventoryTemp>.Operation.OP_ADD, index, new SlotInventoryTemp(), inventory[index]);
    }

    [Command]
    public void CmdAddItem(SlotInventoryTemp slot)
    {
        AddItem(slot);
    }
    public bool AddItem(SlotInventoryTemp slot)
    {
        DataItem item = GameController.Instance.DataManager.GetDataItemById(slot.guidid);
        if (item == null)
        {
            return false;
        }

        for (int i = 0; i < maxSlot; i++)
        {
            if (inventory[i].Compare(new SlotInventoryTemp()))
            {
                inventory.RemoveAt(i);
                inventory.Insert(i, slot);
                return true;
            }
        }
        return false;
    }

    public void InsertItem(int slotEnterIndex, int slotIndexselecionado)
    {
        // 0   5
        print(slotEnterIndex + " " + slotIndexselecionado);
        SlotInventoryTemp auxEnter = inventory[slotEnterIndex];
        SlotInventoryTemp auxSelecionaddo = inventory[slotIndexselecionado];

        inventory.Insert(slotEnterIndex, auxSelecionaddo);
        inventory.Insert(slotIndexselecionado, auxEnter);

        inventory.Remove(inventory[slotEnterIndex]);
        inventory.Remove(inventory[slotIndexselecionado]);


    }


    [Command]
    public void CmdRemoveItem(int slotIndex)
    {

    }
    void OnInventoryUpdated(SyncList<SlotInventoryTemp>.Operation op, int index, SlotInventoryTemp oldItem, SlotInventoryTemp newItem)
    {

        switch (op)
        {
            case SyncList<SlotInventoryTemp>.Operation.OP_ADD:
                // index is where it was added into the list
                // newItem is the new item
                uiInventory.UpdateSlot(index, newItem);
                break;
            case SyncList<SlotInventoryTemp>.Operation.OP_INSERT:
                // index is where it was inserted into the list
                // newItem is the new item
                uiInventory.UpdateSlot(index, newItem);
                break;
            case SyncList<SlotInventoryTemp>.Operation.OP_REMOVEAT:
                // index is where it was removed from the list
                // oldItem is the item that was removed
                uiInventory.UpdateSlot(index, newItem);
                break;
            case SyncList<SlotInventoryTemp>.Operation.OP_SET:
                // index is of the item that was changed
                // oldItem is the previous value for the item at the index
                // newItem is the new value for the item at the index
                uiInventory.UpdateSlot(index, newItem);

                break;
            case SyncList<SlotInventoryTemp>.Operation.OP_CLEAR:
                // list got cleared
                break;
        }
    }
    public void SetUiInventory(UiInventory _uiinventory)
    {
        uiInventory = _uiinventory;
    }
    internal int GetMaxSlots()
    {
        return maxSlot;
    }
    [Command]
    public void CmdInsertItem(int slotIndex, int slotIndexselecionado)
    {

        InsertItem(slotIndex, slotIndexselecionado);
    }
}
