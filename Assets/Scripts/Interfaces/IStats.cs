using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;

public interface IStats
{
    public bool IsDead();
    public void AddHealth(int hp);
    public void AddSatiety(int points);

    public void AddHydratation(int points);
    public void TakeDamage(int damage);

    float GetDamage();
}