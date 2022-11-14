using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;


namespace ApocalipseZ
{
    public class EnemyStats : NetworkBehaviour, IStats
    {
        [SerializeField] private int Damage;
        [SerializeField] private int health;

        public bool IsDead()
        {
            return health <= 0;
        }

        public void AddHealth(int life)
        {
            health += life;

            if (health > 100)
            {
                health = 100;
            }

        }
        public void TakeDamage(int damage)
        {
            health -= damage;
            GetComponent<EnemyDetection>().SetIsProvoked(true);
            if (health < 0)
            {
                health = 0;

            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void CmdTakeDamage(int damage, NetworkConnection sender = null)
        {
            TakeDamage(damage);
        }

        public float GetDamage()
        {
            return Damage;
        }

        public void AddSatiety(int points)
        {

        }

        public void AddHydratation(int points)
        {

        }
    }
}