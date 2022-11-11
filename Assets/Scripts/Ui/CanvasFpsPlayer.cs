using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace ApocalipseZ
{
    public class CanvasFpsPlayer : MonoBehaviour
    {
        [SerializeField] Canvas CanvasInventory;
        [SerializeField] Canvas CanvasPrimaryAndSecondWeapons;
        [SerializeField] Canvas CanvasFastItems;
        [SerializeField] Canvas CanvasScope;
        [SerializeField] Canvas CanvasPlayerStats;
        [SerializeField] Canvas CanvasMissao;

        [SerializeField] UiPrimaryAndSecondWeapons UiPrimaryAndSecondWeapons;
        [SerializeField] UiInventory UiInventory;
        [SerializeField] UiFastItems UiFastItems;
        [SerializeField] UiFpsScopeCursorReticles UiFpsScopeCursorReticles;
        [SerializeField] UiPlayerStats UiPlayerStats;
        [SerializeField] MotionBlur motionBlur;
        [SerializeField] Volume volume;
        PlayerStats stats;
        FirstPersonCamera FirstPersonCamera;


        [SerializeField] private UISlotItem PrefabUiSlotItem;
        public static UISlotItem SlotSelecionado;
        public static UISlotItem SlotEnter;

        public static void Instancia(GameObject clone)
        {
            SlotSelecionado = Instantiate(clone).GetComponent<UISlotItem>();
        }
        private void Awake()
        {
            InputManager = GameController.Instance.InputManager;
            //  volume = GameObject.Find ( "PostProcessing" ).GetComponent<Volume> ( );
            //  VolumeProfile proflile = volume.sharedProfile;
            // volume.profile.TryGet ( out motionBlur );
        }

        public static bool IsInventoryOpen = false;

        // Update is called once per frame
        void Update()
        {
            if (InputManager.GetInventory() && !stats.IsDead())
            {
                IsInventoryOpen = !IsInventoryOpen;
                WeaponManager.IsChekInventory = true;
                CanvasInventory.enabled = IsInventoryOpen;
                ActiveMotionBlur(IsInventoryOpen);
            }
            if (InputManager.GetEsc())
            {
                IsInventoryOpen = false;
                WeaponManager.IsChekInventory = true;
                CanvasInventory.enabled = IsInventoryOpen;
                ActiveMotionBlur(IsInventoryOpen);
            }
        }

        public void ActiveMotionBlur(bool active)
        {
            //FirstPersonCamera.ActiveCursor(active);
            // motionBlur.active = active;
            //  Time.timeScale = active ? 0 : 1;
        }
        public UiFpsScopeCursorReticles GetUiFpsScopeCursorReticles()
        {
            return UiFpsScopeCursorReticles;
        }
        public UiInventory GetUiInventory()
        {
            return UiInventory;
        }

        internal void SetPlayerStats(PlayerStats playerStats)
        {
            stats = playerStats;
        }

        internal void SetInventory(Inventory inventory)
        {
            UiInventory.SetInventory(inventory);
        }

        private InputManager InputManager;

    }
}