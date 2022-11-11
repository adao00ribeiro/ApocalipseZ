using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public interface IContainer
{
    public event System.Action OnContainerAltered;
    SSlotInventory GetSlotContainer(int index);
    bool AddItem(int slotIndex, SSlotInventory slot);
    bool AddItem(SSlotInventory item);
    void Clear();
    public bool CheckFreeSpace(ref int posicao);
    public bool CheckIfItemExist(int slotIndex);
    public void RemoveItem(int slotIndex);
    public void UseItem(int slotIndex);
    public int GetMaxSlots();
    public void SetMaxSlots(int maxslot);
    void MoveItem(int id, int idmove);
    InventoryTemp GetContainerTemp();
    void InvokeOnContainer();
    TypeContainer GetTypeContainer();
    void Update(SlotInventoryTemp slot);

}
