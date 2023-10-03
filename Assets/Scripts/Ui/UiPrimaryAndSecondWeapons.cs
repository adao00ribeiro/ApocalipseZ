using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class UiPrimaryAndSecondWeapons : UIAbstract
    {
        [SerializeField] private UISlotItem UiPrimaryWeapon;
        [SerializeField] private UISlotItem UiSecondWeapon;
        WeaponManager weaponManager;
        public WeaponManager WeaponManager
        {
            get
            {
                if (weaponManager == null)
                {
                    weaponManager = GameController.Instance.playerController.GetPlayer().GetWeaponManager();
                }
                return weaponManager;
            }

        }

        private void Awake()
        {
            UiPrimaryWeapon = transform.Find("Container/Primary Weapon Slot").GetComponent<UISlotItem>();
            UiSecondWeapon = transform.Find("Container/Second Weapon Slot").GetComponent<UISlotItem>();
            WeaponManager.OnPrimaryWeapon += UpdatePrimaryWeapon; ;
            WeaponManager.OnSecondWeapon += UpdateSecondWeapon; ;
        }
        private void FixedUpdate()
        {

        }
        public void UpdatePrimaryWeapon(Weapon newItem)
        {
            DataItem dataItem = GameController.Instance.DataManager.GetDataItemWeaponByName(newItem.WeaponName);

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
        public void UpdateSecondWeapon(Weapon newItem)
        {

            DataItem dataItem = GameController.Instance.DataManager.GetDataItemWeaponByName(newItem.WeaponName);
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