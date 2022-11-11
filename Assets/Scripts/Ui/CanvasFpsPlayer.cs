using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace ApocalipseZ {
    public class CanvasFpsPlayer : MonoBehaviour
    {
        [SerializeField]UiPrimaryAndSecondWeapons UiPrimaryAndSecondWeapons;
        [SerializeField]UiInventory UiInventory;
        [SerializeField]UiFastItems UiFastItems;
        [SerializeField]UiFpsScopeCursorReticles UiFpsScopeCursorReticles;
        [SerializeField]UiPlayerStats UiPlayerStats;
        [SerializeField]MotionBlur motionBlur;
        [SerializeField]Volume volume;

        PlayerStats stats;
        FirstPersonCamera FirstPersonCamera;
        private void Awake ( )
        {
          //  volume = GameObject.Find ( "PostProcessing" ).GetComponent<Volume> ( );
          //  VolumeProfile proflile = volume.sharedProfile;
           // volume.profile.TryGet ( out motionBlur );
        }

        public static bool IsInventoryOpen = false;
        public void Init ( IFpsPlayer player )
        {
            UiPrimaryAndSecondWeapons = transform.Find ( "HUD/UiPrimaryAndSecondWeapons" ).GetComponent<UiPrimaryAndSecondWeapons> ( );
            UiInventory = transform.Find ( "HUD/UiInventory" ).GetComponent<UiInventory> ();
            UiFastItems = transform.Find ( "HUD/UiFastItems" ).GetComponent<UiFastItems> ( );
            UiFpsScopeCursorReticles = transform.Find ( "HUD/UiFpsScopeCursorReticles" ).GetComponent<UiFpsScopeCursorReticles> ( );
            UiPlayerStats = transform.Find ( "HUD/UiPlayerStats" ).GetComponent<UiPlayerStats> ( );
            
            UiPrimaryAndSecondWeapons.SetFpsPlayer ( player );
            UiInventory.SetFpsPlayer ( player );
            UiFastItems.SetFpsPlayer ( player );
            UiPlayerStats.SetFpsPlayer ( player );

            UiFpsScopeCursorReticles.SetWeaponManager ( player.GetWeaponManager ( ) ) ;
            UiFpsScopeCursorReticles.SetCamera ( player .GetFirstPersonCamera());
            UiPrimaryAndSecondWeapons.gameObject.SetActive ( IsInventoryOpen );
            UiInventory.gameObject.SetActive ( IsInventoryOpen );
            UiFastItems.gameObject.SetActive ( IsInventoryOpen );

            stats = player.GetPlayerStats ( );
            FirstPersonCamera = player.GetFirstPersonCamera ( );
            ActiveMotionBlur ( IsInventoryOpen );
        }

      


        // Update is called once per frame
        void Update ( )
        {
            if ( InputManager.GetInventory ( ) && !stats.IsDead( ))
            {
                IsInventoryOpen = !IsInventoryOpen;
                WeaponManager.IsChekInventory = true;
                UiPrimaryAndSecondWeapons.gameObject.SetActive ( IsInventoryOpen );
                UiInventory.gameObject.SetActive ( IsInventoryOpen );
                UiFastItems.gameObject.SetActive ( IsInventoryOpen );
                ActiveMotionBlur ( IsInventoryOpen );
            }
            if ( InputManager.GetEsc ( ) )
            {
                IsInventoryOpen =false;
                WeaponManager.IsChekInventory = true;
                UiPrimaryAndSecondWeapons.gameObject.SetActive ( IsInventoryOpen );
                UiInventory.gameObject.SetActive ( IsInventoryOpen );
                UiFastItems.gameObject.SetActive ( IsInventoryOpen );
                ActiveMotionBlur ( IsInventoryOpen );
            }
        }

        public void ActiveMotionBlur (bool active )
        {
            FirstPersonCamera.ActiveCursor ( active );
                motionBlur.active = active;
              //  Time.timeScale = active ? 0 : 1;
           
        }
        public UiFpsScopeCursorReticles GetUiFpsScopeCursorReticles ( )
        {
            return UiFpsScopeCursorReticles;
        }
        private InputManager PInputManager;
        public InputManager InputManager
        {
            get
            {
                if ( PInputManager == null )
                {
                    PInputManager = GameObject.Find ( "InputManager" ).GetComponent<InputManager> ( );
                }
                return PInputManager;
            }
        }
    }
}