using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using FishNet.Connection;
using TMPro;

namespace ApocalipseZ
{
    public class PlayerStats : NetworkBehaviour, IStats
    {
        public event Action OnAlteredStats;

        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(OnSetHealth))]
        public int health;

        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(OnSetHydratation))]
        public int hydratation = 100;

        public float hydratationSubstractionRate = 3f;
        public int thirstDamage = 1;
        private float hydratationTimer;

        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(OnSetSatiety))]
        public int satiety = 100;

        public float satietySubstractionRate = 5f;
        public int hungerDamage = 1;
        private float satietyTimer;
        public float stamina;

        [SyncVar]
        public bool Disable;

        public DeadStatsManager deadStatsManager;
        void Start()
        {
            deadStatsManager = GameObject.FindAnyObjectByType<DeadStatsManager>();
        }
        private void OnSetHealth(int oldHealth, int newHealth, bool asServer)
        {
            health = newHealth;
            OnAlteredStats?.Invoke();
        }
        private void OnSetHydratation(int oldHydratation, int newHydratation, bool asServer)
        {
            hydratation = newHydratation;

            OnAlteredStats?.Invoke();
        }
        private void OnSetSatiety(int oldSatiety, int newSatiety, bool asServer)
        {
            satiety = newSatiety;

            OnAlteredStats?.Invoke();
        }
        private void OnStaminaChange(int oldStamina, int newStamina, bool asServer)
        {
            stamina = newStamina;

            OnAlteredStats?.Invoke();
        }

        void Update()
        {
            if (base.IsServer)
            {
                if (Disable)
                {
                    return;
                }
                if (IsDead())
                {
                    PlayerController playerController = transform.parent.GetComponent<PlayerController>();
                    deadStatsManager.ObserveViewUiDeadStats(playerController.PlayerName);
                    deadStatsManager.TargewtViewSeFudeo(base.Owner);
                    playerController.GetPlayer().GetMoviment().DisableCharacterController();
                    playerController.GetPlayer().DroppAllItems();
                    GameController.Instance.TimerManager.Add(() =>
                                    {       
                                           playerController.GetPlayer().GetMoviment().EnableCharacterController();
                        transform.position = GameController.Instance.PlayerSpawPoints.GetPointSpaw().position;
                            AddHealth(200);
                        AddHydratation(100);
                        AddSatiety(100);
                        playerController.GetPlayer().TargetRespaw(base.Owner);
                             }, 5);

                    Disable = true;
                    return;
                }
                if (Time.time > satietyTimer + satietySubstractionRate)
                {
                    if (satiety <= 0)
                    {
                        satiety = 0;
                        health -= hungerDamage;
                    }

                    satiety -= 1;
                    satietyTimer = Time.time;

                }

                if (Time.time > hydratationTimer + hydratationSubstractionRate)
                {
                    if (hydratation <= 0)
                    {
                        hydratation = 0;
                        health -= thirstDamage;
                    }
                    hydratation -= 1;
                    hydratationTimer = Time.time;
                }

                if (hydratation > 100)
                {
                    hydratation = 100;
                }
                if (satiety > 100)
                {
                    satiety = 100;
                }

                if (transform.position.y < -11.1 && !IsDead())
                {
                    TakeDamage(200);
                }

            }
            if (base.IsClient)
            {
                if (IsDead())
                {
                PlayerController playerController = transform.parent.GetComponent<PlayerController>();
                playerController.GetPlayer().GetFirstPersonCamera().CameraDeath();
                }
            }

        }

        public bool IsDead()
        {
            return health <= 0;
        }

        public void AddHealth(int life)
        {
            Disable = false;
            health += life;

            if (health > 100)
            {
                health = 100;
            }
            OnAlteredStats?.Invoke();
        }
        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health < 0)
            {
                health = 0;
            }
            OnAlteredStats?.Invoke();
        }

        public float GetDamage()
        {
            return 0;
        }

        public void AddSatiety(int points)
        {
            satiety += points;
        }

        public void AddHydratation(int points)
        {
            hydratation += points;
        }
    }
}