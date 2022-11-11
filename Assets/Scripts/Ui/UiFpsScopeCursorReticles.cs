using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ApocalipseZ
{
    public class UiFpsScopeCursorReticles : MonoBehaviour
    {
        [SerializeField] private GameObject useCursor;
        [SerializeField] private Text useText;
        public bool UseNonPhysicalReticle = true;
        [Tooltip("Scope image used for riffle aiming state")]
        public GameObject scopeImage;
        [Tooltip("Crosshair image")]
        public GameObject reticleDynamic;
        public GameObject reticleStatic;
        public FirstPersonCamera cam;
        public Weapon weaponActive;
        [SerializeField] private float scopeTimer = 0f;
        private float scopeActivateTimer = 0.5f;
        private float normalFOV;
        private float normalSensX;
        private float normalSensY;
        public LayerMask layerNoWeapon;
        // Start is called before the first frame update
        void Init()
        {
            useCursor = transform.Find("UseCursor").gameObject;
            useText = useCursor.GetComponentInChildren<Text>();
            useCursor.SetActive(false);

            scopeImage = transform.Find("Scope").gameObject;
            reticleDynamic = transform.Find("Reticles/DynamicReticle").gameObject;
            reticleStatic = transform.Find("Reticles/StaticReticle").gameObject;
            scopeImage.SetActive(false);
            normalFOV = cam.GetComponent<Camera>().fieldOfView;
            normalSensX = cam.sensitivity.x;
            normalSensY = cam.sensitivity.y;
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
        }

        // Update is called once per frame
        void Update()
        {
            return;
            if (weaponActive == null)
            {
                cam.GetComponent<Camera>().fieldOfView = normalFOV;
                cam.sensitivity.x = normalSensX;
                cam.sensitivity.y = normalSensY;
                scopeImage.SetActive(false);
                scopeActivateTimer = scopeTimer;
                return;
            }
            scopeActivateTimer -= Time.deltaTime;

            if (weaponActive.GetScriptableWeapon().canUseScope && weaponActive.SetAim)
            {
                if (scopeActivateTimer <= 0)
                {
                    scopeImage.SetActive(true);
                    cam.GetComponent<Camera>().fieldOfView = weaponActive.GetScriptableWeapon().scopeFOV;
                    cam.sensitivity.x = weaponActive.GetScriptableWeapon().scopeSensitivityX;
                    cam.sensitivity.y = weaponActive.GetScriptableWeapon().scopeSensitivityY;
                    cam.GetComponent<Camera>().cullingMask = layerNoWeapon;
                }
            }
            else
            {
                cam.GetComponent<Camera>().fieldOfView = normalFOV;

                cam.sensitivity.x = normalSensX;
                cam.sensitivity.y = normalSensY;
                scopeImage.SetActive(false);
                scopeActivateTimer = scopeTimer;
                cam.GetComponent<Camera>().cullingMask = cam.defaultLayer; ;
            }
        }

        internal void SetCamera(FirstPersonCamera firstPersonCamera)
        {
            cam = firstPersonCamera;
        }

        internal void SetWeaponManager(IWeaponManager weaponManager)
        {
            //   weaponManager.OnActiveWeapon += SetActiveWeapon; ;

        }

        private void SetActiveWeapon(Weapon obj)
        {
            weaponActive = obj;
        }

        public void ActiveReticle(bool UseNonPhysicalReticle)
        {
            if (UseNonPhysicalReticle)
                reticleStatic.SetActive(false);
            else
                reticleDynamic.gameObject.SetActive(false);
        }
        public void EnableCursor()
        {
            useCursor.SetActive(true);
        }
        public void DisableCursor()
        {
            if (useCursor == null)
            {
                return;
            }
            useCursor.SetActive(false);
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