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
        uiInventory = GameController.Instance.CanvasFpsPlayer.GetUiInventory();
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
        if (maxSlot == inventory.Count)
        {
            print("cheio");
            return false;
        }
        int posicao = -1;
        if (CheckFreeSpace(ref posicao))
        {
            slot.slotindex = posicao;
        }
        inventory.Add(slot);
        return true;
    }
    public void MoveItem(SlotInventoryTemp slotselecionado, SlotInventoryTemp slotenter)
    {
        inventory.Insert(slotenter.slotindex,slotselecionado);
    }
    public bool CheckFreeSpace(ref int posicao)
    {
        bool isfreespace = false;

        for (int i = 0; i < maxSlot; i++)
        {
            SlotInventoryTemp item = GetSlotContainer(i);
            if (item == null)
            {
                posicao = i;
                isfreespace = true;
                break;
            }
        }
        return isfreespace;
    }
    public SlotInventoryTemp GetSlotContainer(int slotindex)
    {
        SlotInventoryTemp temp = null;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].slotindex == slotindex)
            {
                temp = inventory[i];
            }
        }
        return temp;
    }
    [Command]
    public void CmdRemoveItem(int slotIndex)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].slotindex == slotIndex)
            {
                // Debug.Log ( "Removed item: " + Items[i].GetSItem ( ).name + "  " + type.ToString ( ) );
                inventory.Remove(inventory[i]);
                break;
            }
        }
    }
    void OnInventoryUpdated(SyncList<SlotInventoryTemp>.Operation op, int index, SlotInventoryTemp oldItem, SlotInventoryTemp newItem)
    {


        DataItem item = GameController.Instance.DataManager.GetDataItemById(newItem.guidid);
        switch (op)
        {
            case SyncList<SlotInventoryTemp>.Operation.OP_ADD:
                // index is where it was added into the list
                // newItem is the new item

                uiInventory.UpdateSlot(newItem);
                break;
            case SyncList<SlotInventoryTemp>.Operation.OP_INSERT:
                // index is where it was inserted into the list
                // newItem is the new item

                break;
            case SyncList<SlotInventoryTemp>.Operation.OP_REMOVEAT:
                // index is where it was removed from the list
                // oldItem is the item that was removed

                break;
            case SyncList<SlotInventoryTemp>.Operation.OP_SET:
                // index is of the item that was changed
                // oldItem is the previous value for the item at the index
                // newItem is the new value for the item at the index

                uiInventory.UpdateSlot(newItem);
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
    public void CmdMoveItem(SlotInventoryTemp slotselecionado, SlotInventoryTemp slotenter)
    {
        MoveItem( slotselecionado,  slotenter);
    }
}
