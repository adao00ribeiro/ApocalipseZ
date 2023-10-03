using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ApocalipseZ
{
    public class UiInventory : UIAbstract
    {
        public UISlotItem PrefabSlot;
        [SerializeField] private List<UISlotItem> UIItems = new List<UISlotItem>();
        private Transform slotPanel;
        private FpsPlayer player;
        void Awake()
        {
            slotPanel = transform.Find("SlotPanel").transform;
            AddSlots();

        }
        void Start()
        {
            player.GetInventory().OnUpdateInventory += UpdateSlot; ;
        }
        void OnDisable()
        {
            player.GetInventory().OnUpdateInventory -= UpdateSlot; ;
        }
        public void AddSlots()
        {
            foreach (UISlotItem item in UIItems)
            {
                Destroy(item.gameObject);
            }
            UIItems.Clear();
            for (int i = 0; i < Player.GetInventory().GetMaxSlots(); i++)
            {
                UISlotItem instance = Instantiate(PrefabSlot, slotPanel);
                instance.HUD = transform.parent;
                instance.SetSlotIndex(i);
                instance.SetInventory(Player.GetInventory());
                instance.SetWeaponManager(Player.GetWeaponManager());
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
        internal void UpdateSlot(int index, SlotInventoryTemp newItem)
        {

            DataItem dataItem = GameController.Instance.DataManager.GetDataItemById(newItem.guidid);
            if (dataItem == null)
            {
                UIItems[index].SetIsEmpty(true);
                UIItems[index].SetImage(null);
                UIItems[index].SetTextQuantidade("");
            }
            else
            {
                UIItems[index].SetIsEmpty(false);
                UIItems[index].SetImage(dataItem.Thumbnail);
                UIItems[index].SetTextQuantidade(newItem.Quantity.ToString());
            }
        }


        public FpsPlayer Player
        {
            get
            {
                if (player == null)
                {
                    player = GameController.Instance.playerController.GetPlayer();
                }
                return player;
            }
        }
    }
}