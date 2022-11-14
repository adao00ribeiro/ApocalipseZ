using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class UiPrimaryAndSecondWeapons : MonoBehaviour
    {
        [SerializeField] private UISlotItem UiPrimaryWeapon;
        [SerializeField] private UISlotItem UiSecondWeapon;

        IFpsPlayer player;

        private void Awake()
        {
            UiPrimaryWeapon = transform.Find("Container/Primary Weapon Slot").GetComponent<UISlotItem>();
            UiSecondWeapon = transform.Find("Container/Second Weapon Slot").GetComponent<UISlotItem>();
            UiPrimaryWeapon.HUD = transform.parent;
            UiSecondWeapon.HUD = transform.parent;
            UpdatePrimaryWeapon(new SlotInventoryTemp());
        }

        public void UpdatePrimaryWeapon( SlotInventoryTemp newItem)
        {
           
            DataItem dataItem = GameController.Instance.DataManager.GetDataItemById(newItem.guidid);
             
            if (dataItem == null)
            {
                UiPrimaryWeapon.SetIsEmpty(true);
                UiPrimaryWeapon.SetImage(null);
                UiPrimaryWeapon.SetTextQuantidade("");
            }
            else
            {
            
                UiPrimaryWeapon.SetIsEmpty(false);
                UiPrimaryWeapon.SetImage(dataItem.Thumbnail);
                UiPrimaryWeapon.SetTextQuantidade("1");
            }
        }
        public void UpdateSecundaryWeapon(SlotInventoryTemp newItem)
        {
            if(newItem.Compare(new SlotInventoryTemp())){
                UiSecondWeapon.SetIsEmpty(true);
                UiSecondWeapon.SetImage(null);
                UiSecondWeapon.SetTextQuantidade("");
                return;
            }
            DataItem dataItem = GameController.Instance.DataManager.GetDataItem(newItem.guidid);
            if (dataItem == null)
            {
                UiSecondWeapon.SetIsEmpty(true);
                UiSecondWeapon.SetImage(null);
                UiSecondWeapon.SetTextQuantidade("");
            }
            else
            {
                UiSecondWeapon.SetIsEmpty(false);
                UiSecondWeapon.SetImage(dataItem.Thumbnail);
                UiSecondWeapon.SetTextQuantidade("1");
            }
        }

    }
}