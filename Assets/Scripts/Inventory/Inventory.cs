using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using ApocalipseZ;

public class Inventory : NetworkBehaviour
{
    private UiInventory uiInventory;

    [SyncObject]
    public readonly SyncList<SlotInventoryTemp> inventory = new SyncList<SlotInventoryTemp>();
    public List<ListItemsInspector> ListInspector = new List<ListItemsInspector>();
    [SerializeField] private int maxSlot = 6;

    public bool teste;
    void Awake()
    {

    }
    void FixedUpdate(){
     ListInspector.Clear();
      for (int i = 0; i < inventory.Count; i++)
      {
            ListInspector.Add(new ListItemsInspector(i,inventory[i].Name , inventory[i].Ammo));
      }
    }
    void StartInventory()
    {
        for (int i = 0; i < maxSlot; i++)
        {
            inventory.Add(new SlotInventoryTemp());
        }
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
         StartInventory();
    }
    public override void OnStartClient()
    {

        base.OnStartClient();
        if(base.IsOwner){
            
        uiInventory = GameController.Instance.CanvasFpsPlayer.GetUiInventory();
        uiInventory.SetInventory(this);
        uiInventory.SetWeaponManager(GetComponent<WeaponManager>());
        uiInventory.AddSlots();
        inventory.OnChange += OnInventoryUpdated;
          }

    }

    [ServerRpc]
    public void CmdAddItem(SlotInventoryTemp slot)
    {
        AddItem(slot);
    }
    public SlotInventoryTemp GetSlot(int index)
    {

        return inventory[index];
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
                inventory[i] = slot;
                return true;
            }
        }
        return false;
    }

    public void InsertItem(int slotEnterIndex, int slotIndexselecionado)
    {
        SlotInventoryTemp auxEnter = inventory[slotEnterIndex];
        inventory[slotEnterIndex] = inventory[slotIndexselecionado];
        inventory[slotIndexselecionado] = auxEnter;
    }

    public void RemoveItem(SlotInventoryTemp slot)
    {

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].Compare(slot))
            {
                inventory[i] = new SlotInventoryTemp();
            }
        }
    }
    [ServerRpc]
    public void CmdRemoveItem(int slotIndex)
    {

    }
    private void OnInventoryUpdated(SyncListOperation op, int index, SlotInventoryTemp oldItem, SlotInventoryTemp newItem, bool asServer)
    {
        switch (op)
        {
            case SyncListOperation.Add:
                // index is where it was added into the list
                // newItem is the new item
                uiInventory.UpdateSlot(index, newItem);
                break;
            case SyncListOperation.Insert:
                // index is where it was inserted into the list
                // newItem is the new item
                uiInventory.UpdateSlot(index, newItem);
                break;
            case SyncListOperation.RemoveAt:
                // index is where it was removed from the list
                // oldItem is the item that was removed
                uiInventory.UpdateSlot(index, newItem);
                break;
            case SyncListOperation.Set:
                // index is of the item that was changed
                // oldItem is the previous value for the item at the index
                // newItem is the new value for the item at the index
                uiInventory.UpdateSlot(index, newItem);
                break;
            case SyncListOperation.Clear:
                // list got cleared
                break;
            case SyncListOperation.Complete:
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
    [ServerRpc]
    public void CmdInsertItem(int slotIndex, int slotIndexselecionado)
    {

        InsertItem(slotIndex, slotIndexselecionado);
    }
}
