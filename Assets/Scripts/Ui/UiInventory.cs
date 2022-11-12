using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
    public class UiInventory : MonoBehaviour
    {

        public UISlotItem PrefabSlot;
        [SerializeField] private List<UISlotItem> UIItems = new List<UISlotItem>();
        private Transform slotPanel;
        Inventory inventory;


        void Awake()
        {
            slotPanel = transform.Find("SlotPanel").transform;

        }

        public void SetInventory(Inventory _inventory)
        {
            inventory = _inventory;
            AddSlots();
        }

        public void AddSlots()
        {
            foreach (UISlotItem item in UIItems)
            {
                Destroy(item.gameObject);
            }
            UIItems.Clear();
            for (int i = 0; i < inventory.GetMaxSlots(); i++)
            {
                UISlotItem instance = Instantiate(PrefabSlot, slotPanel);
                instance.HUD = transform.parent;
                instance.SetSlotIndex(i);
                UIItems.Add(instance);
            }
        }
        public void ClearSlot(int index)
        {
            foreach (UISlotItem item in UIItems)
            {
                if (item.GetSlotIndex() == index)
                {
                    item.SetImage(null);
                    item.SetTextQuantidade("");
                }
            }
        }
        internal void UpdateSlot(SlotInventoryTemp newItem)
        {
          
            foreach (UISlotItem item in UIItems)
            {
                if (item.GetSlotIndex() == newItem.slotindex)
                {
                    DataItem dataItem = GameController.Instance.DataManager.GetDataItemById(newItem.guidid);
                    item.SetInventorySlot(newItem);
                    item.SetImage(dataItem.Thumbnail);
                    item.SetTextQuantidade(newItem.Quantity.ToString());
                }
            }
        }
    }
}