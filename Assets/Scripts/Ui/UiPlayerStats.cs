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
        IFpsPlayer player;
        [SerializeField] PlayerStats stats;
        [SerializeField] WeaponManager WeaponManager;
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
        public void OnUpdateHealth()
        {
            if (player == null)
            {
                return;
            }
            if (HealthSlider.value != stats.health)
            {
                StartCoroutine(HitFX());
            }
            HealthSlider.value = stats.health;
            HealthText.text = stats.health.ToString();
            HydratationSlider.value = stats.hydratation;
            HydratationText.text = stats.hydratation.ToString();
            SatietySlider.value = stats.satiety;
            SatietySliderText.text = stats.satiety.ToString();
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
        public void SetFpsPlayer(IFpsPlayer _player)
        {
            player = _player;
            WeaponManager = _player.GetWeaponManager();
            stats = player.GetPlayerStats();
            stats.OnAlteredStats += OnUpdateHealth; ;
            // WeaponManager.OnActiveWeapon += OnActiveSlot; ;
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