using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ApocalipseZ
{
    public class UiPlayerStats : UIAbstract
    {
        [SerializeField] private Slider HealthSlider;
        [SerializeField] private Text HealthText;
        [SerializeField] private Image DamageScreen;
        float damageScreenFadeSpeed = 1.4f;
        [SerializeField] private Slider HydratationSlider;
        [SerializeField] private Text HydratationText;
        [SerializeField] private Slider SatietySlider;
        [SerializeField] private Text SatietySliderText;
        [SerializeField] private Text WeaponText;
        [SerializeField] private Text AmmoText;
        WeaponManager weaponManager;
        private void Awake()
        {
            HealthSlider = transform.Find("HealthSlider").GetComponent<Slider>();
            HealthText = transform.Find("HealthSlider/HealthText").GetComponent<Text>();

            HydratationSlider = transform.Find("HydratationSlider").GetComponent<Slider>();
            HydratationText = transform.Find("HydratationSlider/HydratationText").GetComponent<Text>();

            SatietySlider = transform.Find("SatietySlider").GetComponent<Slider>();
            SatietySliderText = transform.Find("SatietySlider/SatietyText").GetComponent<Text>();

            WeaponText = transform.Find("InfoPanel/WeaponText").GetComponent<Text>();
            AmmoText = transform.Find("InfoPanel/AmmoText").GetComponent<Text>();
        }
        void Start()
        {
            PlayerStats stats = GameController.Instance.playerController.GetPlayer().GetPlayerStats();
            weaponManager = GameController.Instance.playerController.GetPlayer().GetWeaponManager();
            stats.OnAlteredHealth += OnUpdateHealth; ;
            stats.OnAlteredHydratation += OnUpdateHydratation; ;
            stats.OnAlteredSatiety += OnUpdateSatiety; ;

        }
        void FixedUpdate()
        {
            if (weaponManager.activeSlot != null)
            {
                OnActiveSlot(weaponManager.activeSlot);
            }
        }
        public void OnUpdateHealth(int health)
        {

            if (HealthSlider.value != health)
            {
                StartCoroutine(HitFX());
            }
            HealthSlider.value = health;
            HealthText.text = health.ToString();
        }
        public void OnUpdateHydratation(int hydratation)
        {
            HydratationSlider.value = hydratation;
            HydratationText.text = hydratation.ToString();
        }
        public void OnUpdateSatiety(int satiety)
        {

            SatietySlider.value = satiety;
            SatietySliderText.text = satiety.ToString();
        }
        public void OnActiveSlot(Weapon weapon)
        {
            if (weapon)
            {
                WeaponText.text = weapon.WeaponName;
                AmmoText.text = weapon.CurrentAmmo.ToString();
            }
            else
            {
                WeaponText.text = " ";
                AmmoText.text = " ";
            }
        }


        IEnumerator HitFX()
        {
            DamageScreen.color = new Color(DamageScreen.color.r, DamageScreen.color.g, DamageScreen.color.b, 0.4f);
            float alpha = DamageScreen.color.a;
            while (DamageScreen.color.a > 0)
            {
                alpha -= damageScreenFadeSpeed * Time.deltaTime;
                Color temp = new Color(DamageScreen.color.r, DamageScreen.color.g, DamageScreen.color.b, alpha);
                DamageScreen.color = temp;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}