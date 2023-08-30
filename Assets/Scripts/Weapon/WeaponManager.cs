using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using Unity.Mathematics;

namespace ApocalipseZ
{
    public class WeaponManager : NetworkBehaviour
    {

        UiPrimaryAndSecondWeapons UiPrimaryAndSecondWeapons;

        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(SetAmmoNetwork))]
        public int AmmoNetwork;
        public Weapon activeSlot;

        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(SetPrimaryWeapon))]
        public SlotInventoryTemp PrimaryWeapon;
        public SlotInventoryTemp SecundaryWeapon;

        [Tooltip("Animator that contain pickup animation")]
        public Animator weaponHolderAnimator;
        IFpsPlayer fpsplayer;
        //Transform where weapons will dropped on Drop()
        private Transform playerTransform;
        [SerializeField] private Transform swayTransform;
        private InputManager InputManager;
        public static bool IsChekInventory;
        public void SetAmmoNetwork(int oldSlot, int newSlot, bool asServer)
        {
            AmmoNetwork = newSlot;

        }
        private void Awake()
        {
            InputManager = GameController.Instance.InputManager;

        }


        void Start()
        {
            UiPrimaryAndSecondWeapons = GameController.Instance.CanvasFpsPlayer.GetUiPrimaryandSecundaryWeapons();
            swayTransform = transform.Find("Camera & Recoil/Weapon holder/Sway").transform;
            weaponHolderAnimator = transform.Find("Camera & Recoil/Weapon holder").GetComponent<Animator>();

        }
        public void SetFpsPlayer(IFpsPlayer fps)
        {
            fpsplayer = fps;
        }
        // Update is called once per frame
        void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            if (IsChekInventory)
            {
                IsChekInventory = false;
            }
            if (InputManager.GetAlpha1())
            {
                CmdSlotChange(0);
            }
            else if (InputManager.GetAlpha2())
            {
                CmdSlotChange(1);
            }
            if (activeSlot == null)
            {
                return;
            }


            if (InputManager.GetFire() && !fpsplayer.GetMoviment().CheckIsRun() && !CanvasFpsPlayer.IsInventoryOpen)
            {
                if (activeSlot.Fire())
                {
                    //  RpcFire(base.Owner);
                }
                AmmoNetwork = activeSlot.CurrentAmmo;

            }
            if (InputManager.GetReload())
            {
                activeSlot.ReloadBegin();

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
                DropWeapon();
            }

            // if (Input.GetKeyDown(KeyCode.H))
            // {
            //     DropAllWeapons();
            // }
        }
        [ServerRpc]
        public void CmdFire()
        {
            if (activeSlot.Fire())
            {
                RpcFire(base.Owner);
            }
            AmmoNetwork = activeSlot.CurrentAmmo;
        }
        [TargetRpc]
        public void RpcFire(NetworkConnection conn)
        {
            activeSlot.PlayFX();
        }


        [ServerRpc]
        public void CmdSlotChange(int currentSlot)
        {
            DataArmsWeapon tempArms = null;
            int ammo = 0;
            if (currentSlot == 0)
            {
                DesEquipWeapon();
                tempArms = GameController.Instance.DataManager.GetArmsWeapon(PrimaryWeapon.Name);
                ammo = PrimaryWeapon.Ammo;
            }
            else
            {
                DesEquipWeapon();
                tempArms = GameController.Instance.DataManager.GetArmsWeapon(SecundaryWeapon.Name);
                ammo = SecundaryWeapon.Ammo;
            }
            if (tempArms == null)
            {
                print("aki");
                return;
            }
            activeSlot = Instantiate(tempArms.PrefabArmsWeapon, swayTransform).GetComponent<Weapon>();
            activeSlot.Cam = fpsplayer.GetFirstPersonCamera();
            activeSlot.CurrentAmmo = ammo;
            activeSlot.gameObject.SetActive(true);
            base.Spawn(activeSlot.gameObject);
            weaponHolderAnimator.Play("Unhide");
            ObserverRpcSlotChange(activeSlot.gameObject);
        }

        [TargetRpc]
        public void TargetRpcSlotChange(NetworkConnection target, GameObject weapon, Transform sway)
        {
            activeSlot = weapon.GetComponent<Weapon>();
        }
        [ObserversRpc]
        public void ObserverRpcSlotChange(GameObject weapon)
        {
            activeSlot = weapon.GetComponent<Weapon>();
            activeSlot.Cam = fpsplayer.GetFirstPersonCamera();
            activeSlot.transform.SetParent(swayTransform);
            activeSlot.transform.localPosition = Vector3.zero;
            activeSlot.transform.localRotation = quaternion.identity;
        }
        private void SelecionaWeapon()
        {
            if (CanvasFpsPlayer.IsInventoryOpen)
            {
                return;
            }
        }
        private void DropWeapon()
        {
            if (activeSlot != null)
            {
                //CmdDropWeapon(container.GetSlotContainer(switchSlotIndex).GetSlotTemp());
                weaponHolderAnimator.Play("Unhide");
                DesEquipWeapon();
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


        public void DesEquipWeapon()
        {
            if (activeSlot != null)
            {
                weaponHolderAnimator.Play("Hide");
                base.Despawn(activeSlot.gameObject);
                activeSlot = null;
            }

        }

        public IWeapon GetActiveWeapon()
        {
            return activeSlot;
        }
        public void SetPrimaryWeapon(SlotInventoryTemp oldSlot, SlotInventoryTemp newSlot, bool asServer)
        {

            PrimaryWeapon = newSlot;
            UiPrimaryAndSecondWeapons.UpdatePrimaryWeapon(newSlot);
        }

        [ServerRpc]
        public void CmdAddWeaponRemoveInventory(int slotenter, int SlotSelecionado)
        {
            SlotInventoryTemp slot = fpsplayer.GetInventory().GetSlot(SlotSelecionado);
            if (slotenter == 0)
            {
                PrimaryWeapon = slot;
            }
            else
            {
                SecundaryWeapon = slot;
            }
            fpsplayer.GetInventory().RemoveItem(slot);
        }
        [ServerRpc]
        internal void CmdMove(int slotIndex1, int slotIndex2)
        {

        }
    }

}
