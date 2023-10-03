using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ApocalipseZ
{
    public class UiFpsScopeCursorReticles : UIAbstract
    {
        [SerializeField] private GameObject useCursor;
        [SerializeField] private Text useText;
        public bool UseNonPhysicalReticle = false;
        [Tooltip("Scope image used for riffle aiming state")]
        public GameObject scopeImage;
        [Tooltip("Crosshair image")]
        public GameObject reticleDynamic;
        public GameObject reticleStatic;

        public FpsPlayer player
        {
            get
            {
                return GameController.Instance.playerController.GetPlayer();
            }
        }

        public float restingSize = 75f;
        public float maxSize = 250f;
        public float speed = 2f;
        private float currentSize;
        // Start is called before the first frame update
        public void Start()
        {
            useCursor = transform.Find("UseCursor").gameObject;
            useText = useCursor.GetComponentInChildren<Text>();
            useCursor.SetActive(false);
            reticleDynamic = transform.Find("Reticles/DynamicReticle").gameObject;
            reticleStatic = transform.Find("Reticles/StaticReticle").gameObject;
            if (UseNonPhysicalReticle)
            {
                reticleStatic.SetActive(true);
                reticleDynamic.SetActive(false);
            }
            else
            {
                reticleStatic.SetActive(false);
                reticleDynamic.SetActive(true);
            }

            InteractObjects interact = (InteractObjects)player.GetInteractObjects();
            interact.OnEnableCursor += SetCursor; ;
            interact.OnNameObjectInteract += SetUseText; ;
        }


        // Update is called once per frame
        void Update()
        {
            if (player == null)
            {
                return;
            }
            // Check if player is currently moving and Lerp currentSize to the appropriate value.
            if (isMoving)
            {
                currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);
            }
            else
            {
                currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
            }

            // Set the reticle's size to the currentSize value.
            reticleDynamic.GetComponent<RectTransform>().sizeDelta = new Vector2(currentSize, currentSize);
            if (player.GetWeaponManager().activeSlot != null)
            {
                player.GetWeaponManager().activeSlot.IsFire = false;
            }

        }
        bool isMoving
        {

            get
            {
                if (player.GetWeaponManager().activeSlot)
                {
                    return player.GetWeaponManager().activeSlot.IsFire;
                }
                // If we have assigned a rigidbody, check if its velocity is not zero. If so, return true.
                if (player.GetMoviment().GetCharacterController() != null)
                    if (player.GetMoviment().GetCharacterController().velocity.sqrMagnitude != 0)
                        return true;
                    else
                        return false;

                return player.GetMoviment().CheckMovement();
            }

        }

        public void SetCursor(bool value)
        {
            useCursor.SetActive(value);
        }

        internal void SetUseText(string text)
        {
            if (useText == null)
            {
                return;
            }
            useText.text = text;
        }

    }
}