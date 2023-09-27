using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using Unity.Mathematics;
using System;

namespace ApocalipseZ
{
    public class WeaponManager : NetworkBehaviour
    {

        UiPrimaryAndSecondWeapons UiPrimaryAndSecondWeapons;

        public Weapon activeSlot;
        public Weapon[] twoWeapon = new Weapon[2];
        [Tooltip("Animator that contain pickup animation")]
        public Animator weaponHolderAnimator;
        [SerializeField] FpsPlayer fpsplayer;
        //Transform where weapons will dropped on Drop()
        private Transform playerTransform;
        [SerializeField] private Transform swayTransform;
        private InputManager InputManager;
        public static bool IsChekInventory;
        [SerializeField] private bool loadingWeapon;
        private void Awake()
        {
            InputManager = GameController.Instance.InputManager;

        }
        private void OnValidate()
        {
            // Certifique-se de que o tamanho do array não seja maior que o valor máximo.
            if (twoWeapon.Length > 2)
            {
                System.Array.Resize(ref twoWeapon, 2);
            }
        }
        void Start()
        {
            UiPrimaryAndSecondWeapons = GameController.Instance.CanvasFpsPlayer.GetUiPrimaryandSecundaryWeapons();
            swayTransform = transform.Find("Recoil/Camera & Recoil/Weapon holder/Sway").transform;
            weaponHolderAnimator = transform.Find("Recoil/Camera & Recoil/Weapon holder").GetComponent<Animator>();

        }
        public void SetFpsPlayer(FpsPlayer fps)
        {
            fpsplayer = fps;
        }
        // Update is called once per frame
        void Update()
        {
            if (!IsOwner || fpsplayer.GetPlayerStats().IsDead())
            {
                return;
            }

            if (IsChekInventory)
            {
                IsChekInventory = false;
            }
            if (InputManager.GetAlpha1() && !InputManager.GetFire() && !loadingWeapon)
            {
                CmdSlotChange(0);
            }
            else if (InputManager.GetAlpha2() && !InputManager.GetFire() && !loadingWeapon)
            {
                CmdSlotChange(1);
            }
            if (activeSlot == null)
            {
                return;
            }
            if (InputManager.GetFire() && !loadingWeapon && !fpsplayer.GetMoviment().CheckIsRun() && !CanvasFpsPlayer.IsInventoryOpen)
            {
                activeSlot.CmdFire();
            }
            if (InputManager.GetReload())
            {
                activeSlot.CmdReloadBegin();

            }
            if (InputManager.GetAim() && !fpsplayer.GetMoviment().CheckIsRun())
            {

                activeSlot.Aim(true);
                weaponHolderAnimator.SetBool("Walk", false);
                weaponHolderAnimator.SetBool("Run", false);
            }
            else
            {
                activeSlot.Aim(false);
            }

            if (InputManager.GetDropWeapon())
            {
                CmdDropWeapon();
            }

            // if (Input.GetKeyDown(KeyCode.H))
            // {
            //     DropAllWeapons();
            // }
        }

        [ServerRpc]
        public void CmdSlotChange(int currentSlot)
        {

            if (activeSlot == twoWeapon[currentSlot])
            {
                return;
            }
            if (activeSlot != null)
            {
                if (activeSlot.reloading)
                {
                    return;
                }
            }
            if (twoWeapon[currentSlot] == null)
            {
                return;
            }

            StartCoroutine(Hide(currentSlot));

            ObserverRpcSlotChange(currentSlot);
        }

        [ObserversRpc]
        public void ObserverRpcSlotChange(int currentSlot)
        {
            if (base.IsHost)
            {
                return;
            }

            StartCoroutine(Hide(currentSlot));
        }

        private IEnumerator Hide(int currentSlot)
        {
            loadingWeapon = true;
            weaponHolderAnimator.SetBool("HideWeapon", true);
            yield return new WaitForSeconds(0.4f);
            if (activeSlot != null)
            {
                activeSlot.transform.GetChild(0).gameObject.SetActive(false);
                activeSlot = null;
            }
            activeSlot = twoWeapon[currentSlot];
            activeSlot.Cam = fpsplayer.GetFirstPersonCamera();
            activeSlot.transform.localPosition = Vector3.zero;
            activeSlot.transform.localRotation = quaternion.identity;
            activeSlot.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            weaponHolderAnimator.SetBool("HideWeapon", false);
            PersonalizeArmsWeapon personalize = activeSlot.GetComponent<PersonalizeArmsWeapon>();
            personalize.ActiveArms("Yasmim");
            if (!base.IsOwner)
            {
                FpsPlayer.SetLayerRecursively(activeSlot.gameObject, 8);
            }
            yield return new WaitForSeconds(0.2f);
            loadingWeapon = false;
        }
        private void SelecionaWeapon()
        {
            if (CanvasFpsPlayer.IsInventoryOpen)
            {
                return;
            }
        }

        /*
        public void DropAllWeapons ( )
        {
            weaponHolderAnimator.SetLayerWeight ( 1 , 0 );
            weaponHolderAnimator.SetBool ( "HideWeapon" , true );

            foreach ( Slot slot in slots )
            {
                if ( !slot.IsFree ( ) )
                {
                    if ( slot.storedWeapon.weaponType != WeaponType.Melee && haveMeleeWeaponByDefault )
                    {
                        if ( slot.storedWeapon == activeSlot.storedWeapon )
                        {
                            DropWeapon ( );
                        }
                        else
                        {
                            slot.storedWeapon.gameObject.SetActive ( false );
                            if ( slot.storedDropObject )
                            {
                                slot.storedDropObject.SetActive ( true );
                                slot.storedDropObject.transform.position = playerTransform.transform.position + playerTransform.forward * 0.5f;
                                slot.storedDropObject = null;
                                slot.storedWeapon = null;
                            }
                        }
                    }
                }
            }

            if ( haveMeleeWeaponByDefault )
            {
                activeSlot = slots[0];
                activeSlot.storedWeapon.gameObject.SetActive ( true );
            }
        }
        */

        [ServerRpc]
        public void CmdDropWeapon(NetworkConnection sender = null)
        {
            DropWeapons(activeSlot);
        }


        public IWeapon GetActiveWeapon()
        {
            return activeSlot;
        }


        [ServerRpc]
        public void CmdAddWeaponRemoveInventory(int slotenter, int SlotSelecionado)
        {
            SlotInventoryTemp slot = fpsplayer.GetInventory().GetSlot(SlotSelecionado);
            DataArmsWeapon armsWeapon = GameController.Instance.DataManager.GetArmsWeapon(slot.Name);
            if (armsWeapon == null)
            {
                return;
            }
            Weapon tempWeapon = Instantiate(armsWeapon.PrefabArmsWeapon, swayTransform).GetComponent<Weapon>();
            tempWeapon.transform.localPosition = Vector3.zero;
            tempWeapon.transform.localRotation = quaternion.identity;
            tempWeapon.Cam = fpsplayer.GetFirstPersonCamera();
            tempWeapon.transform.GetChild(0).gameObject.SetActive(false);
            base.Spawn(tempWeapon.gameObject);
            twoWeapon[slotenter] = tempWeapon;
            fpsplayer.GetInventory().RemoveItem(slot);
            ObserverAddWeaponRemoveInventory(slotenter, tempWeapon.gameObject);
        }
        [ObserversRpc]
        public void ObserverAddWeaponRemoveInventory(int slotenter, GameObject weapon)
        {
            weapon.transform.SetParent(swayTransform);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = quaternion.identity;
            weapon.transform.GetChild(0).gameObject.SetActive(false);
            twoWeapon[slotenter] = weapon.GetComponent<Weapon>();
        }
        [ServerRpc]
        internal void CmdMove(int slotIndex1, int slotIndex2)
        {

        }

        internal void DropAllWeapons()
        {
            foreach (var item in twoWeapon)
            {
                if (item != null)
                {
                    DropWeapons(item);
                }
            }
        }

        private void DropWeapons(Weapon weapon)
        {
            //procurar o item 
            DataItem tempWeapon = GameController.Instance.DataManager.GetDataItemWeaponByName(weapon.WeaponName);
            if (tempWeapon == null)
            {
                return;
            }
            GameObject dropItemTemp = Instantiate(tempWeapon.Prefab);
            dropItemTemp.GetComponent<Item>().SetAmmo(activeSlot.CurrentAmmo);
            dropItemTemp.transform.position = fpsplayer.GetFirstPersonCamera().transform.position + fpsplayer.GetFirstPersonCamera().transform.forward * 0.5f;
            base.Spawn(dropItemTemp);
            base.Despawn(weapon.gameObject);
        }
    }

}
