using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
namespace ApocalipseZ
{
    public class WeaponManager : MonoBehaviour, IWeaponManager
    {
        public event Action<IWeapon> OnActiveWeapon;
        public Transform WeaponThir;
        public Transform pivoWeaponThir;
        public List<Weapon> ArmsWeapons = new List<Weapon>();

        public IWeapon activeSlot;

        IContainer container;

        public int switchSlotIndex = 0;
        public int currentWeaponIndex;

        [Tooltip("Animator that contain pickup animation")]
        public Animator weaponHolderAnimator;

        [HideInInspector]
        public GameObject tempGameobject;

        IFpsPlayer fpsplayer;
        //Transform where weapons will dropped on Drop()
        private Transform playerTransform;

        [SerializeField] private Transform swayTransform;

        private InputManager InputManager;

        public static bool IsChekInventory;

        private void Awake()
        {
            InputManager = GameController.Instance.InputManager;
        }
        void Start()
        {

            swayTransform = transform.Find("Camera & Recoil/Weapon holder/Sway").transform;

            weaponHolderAnimator = transform.Find("Camera & Recoil/Weapon holder").GetComponent<Animator>();

            foreach (Weapon weapon in swayTransform.GetComponentsInChildren<Weapon>(true))
            {
                ArmsWeapons.Add(weapon);
            }
            OnActiveWeapon?.Invoke(activeSlot);

        }
        public void SetFpsPlayer(FpsPlayer player)
        {
            //fpsplayer = player;
            playerTransform = player.gameObject.transform;
            // container = player.GetWeaponsSlots();
            container.OnContainerAltered += SlotChange;
        }
        private void OnDestroy()
        {
            container.OnContainerAltered -= SlotChange;
        }
        // Update is called once per frame
        void Update()
        {
            //  WeaponThir.position = pivoWeaponThir.position;
            SlotInput();
            if (IsChekInventory)
            {
                OnCanvasDsdsds();
                IsChekInventory = false;
            }
            if (activeSlot == null)
            {
                return;
            }


            if (InputManager.GetFire() && !fpsplayer.GetMoviment().CheckIsRun() && !CanvasFpsPlayer.IsInventoryOpen)
            {
                activeSlot.Fire();
                SSlotInventory slotTemp = container.GetSlotContainer(currentWeaponIndex);

                if (slotTemp != null)
                {
                    slotTemp.SetAmmo(activeSlot.CurrentAmmo);
                    // container.CmdUpdateSlot(container.GetTypeContainer(), slotTemp.GetSlotTemp());
                }

                OnActiveWeapon?.Invoke(activeSlot);
            }

            if (InputManager.GetReload())
            {
                activeSlot.ReloadBegin();
                OnActiveWeapon?.Invoke(activeSlot);

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
        private void SlotInput()
        {
            if (CanvasFpsPlayer.IsInventoryOpen)
            {
                return;
            }
            if (InputManager.GetAlpha1())
            {
                switchSlotIndex = 0;
                SlotChange();
            }
            else if (InputManager.GetAlpha2())
            {
                switchSlotIndex = 1;
                SlotChange();
            }

        }
        public void OnCanvasDsdsds()
        {
            if (CanvasFpsPlayer.IsInventoryOpen)
            {

                DesEquipWeapon();

                return;
            }
            else
            {

                SlotChange();

            }
        }
        private void SlotChange()
        {
            return;
            SSlotInventory slottemp = container.GetSlotContainer(switchSlotIndex);
            if (slottemp != null)
            {
                DesEquipWeapon();
                EquipWeapon(slottemp);
            }
        }
        public void DesEquipWeapon()
        {
            if (activeSlot != null)
            {
                weaponHolderAnimator.Play("Hide");
                Destroy(activeSlot.gameObject);
                activeSlot = null;
                OnActiveWeapon?.Invoke(activeSlot);
            }

        }
        public void EquipWeapon(SSlotInventory slot)
        {

            DataArmsWeapon tempArms = GameController.Instance.DataManager.GetArmsWeapon(slot.GetDataItem().Name);
            if (tempArms == null)
            {
                return;
            }
            activeSlot = Instantiate(tempArms.PrefabArmsWeapon, swayTransform).GetComponent<IWeapon>();
            activeSlot.CurrentAmmo = slot.GetAmmo();
            activeSlot.gameObject.SetActive(true);
            weaponHolderAnimator.Play("Unhide");
            currentWeaponIndex = switchSlotIndex;
            OnActiveWeapon?.Invoke(activeSlot);

        }

        public IWeapon GetActiveWeapon()
        {
            return activeSlot;
        }

        /*
            [Command]
            public void CmdDropWeapon(SlotInventoryTemp temp, NetworkConnectionToClient sender = null)
            {
                NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
                FpsPlayer fpstemp = sender.identity.GetComponent<FpsPlayer>();
                IContainer container = fpstemp.GetWeaponsSlots();
                //NetworkServer.Spawn ( Instantiate ( ScriptableManager.bullet , spawbulettransform.Position , spawbulettransform.Rotation ) );
                GameObject dropItemTemp = Instantiate(container.GetSlotContainer(temp.slotindex).GetSItem().Prefab);
                dropItemTemp.GetComponent<Item>().SetAmmo(container.GetSlotContainer(temp.slotindex).GetAmmo());
                dropItemTemp.transform.position = fpstemp.GetFirstPersonCamera().transform.position + fpstemp.GetFirstPersonCamera().transform.forward * 0.5f;
                NetworkServer.Spawn(dropItemTemp);
                container.RemoveItem(temp.slotindex);
                fpstemp.GetContainer(TypeContainer.WEAPONS).TargetGetContainer(opponentIdentity.connectionToClient, TypeContainer.WEAPONS, container.GetContainerTemp());

            }

            [TargetRpc]
            public void TargetDesEquipWeapon(NetworkConnection target)
            {

                DesEquipWeapon();
            }
            */
    }

}

