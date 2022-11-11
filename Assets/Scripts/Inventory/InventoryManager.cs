using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace ApocalipseZ
{
    public class InventoryManager : MonoBehaviour,IInventoryManager
    {
       Canvas canvas;
       IFpsPlayer player;
       UiInventory uiInventory;
       [SerializeField] MotionBlur motionBlur;
       [SerializeField] Volume volume;

        public static bool showInventory = false;
        public bool isOpen = true;
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
        private void Start()
        {
            uiInventory = transform.Find ("HUD/InventoryPanel" ).GetComponent<UiInventory> ( );
            canvas = GetComponent<Canvas>();

            volume = GameObject.Find ( "PostProcessing" ).GetComponent<Volume>();

            VolumeProfile proflile = volume.sharedProfile;
            volume.profile.TryGet(out motionBlur);
           
            
        }

        private void Update()
        {
            if (player == null )
            {
                return;
            }
            if (InputManager.GetInventory() && !player.GetPlayerStats ( ).IsDead( ))
            {
                showInventory = !showInventory;
            }

            if (showInventory)
            {
               
                InventoryOpen ();
            }
            else
            {
                InventoryClose();
            }
        }
        public void SetFpsPlayer(IFpsPlayer _player)
        {
            player = _player;
            uiInventory.SetFpsPlayer ( _player );
            InventoryClose ( );
        }
        public void InventoryOpen()
        {
            if (isOpen)
                return;
            else
            {
                canvas.enabled = true;
             
                 motionBlur.active = true;
                Time.timeScale = 0;
                isOpen = true;

            }
        }
        public void InventoryClose()
        {
            if (!isOpen)
                return;
            else
            {
                canvas.enabled = false;
                motionBlur.active = false;
                Time.timeScale = 1;
                isOpen = false;
            }
        }

    }
}