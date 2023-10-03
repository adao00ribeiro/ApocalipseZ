using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType { SMG, SniperRiffle, Pistol, Shotgun, Melee, Grenade }
public enum FireMode { automatic, single }
[System.Serializable]
[CreateAssetMenu(fileName = "DataArmsWeapon", menuName = "Data/ArmsWeapon", order = 0)]
public class DataArmsWeapon : ScriptableObject
{
    public string GuidId = System.Guid.NewGuid().ToString();
    public string Name;
    public WeaponType Type;

    [Header("Fire mode")]
    public FireMode fireMode;
    [Header("Weapon stats", order = 0)]
    public int Damage;
    public float fireRate;
    public float spread;
    public ContantsRecoil recoil;
    public ParticleSystem MuzzleFlashParticlesFX;
    public string shotSFX, reloadingSFX, emptySFX;
    public GameObject shell;
    public int shellsPoolSize;
    public float shellEjectingForce;

    [Header("Scope settings")]

    [Tooltip("Can weapon use scope when aiming?")]
    public bool canUseScope;
    [Tooltip("Zoom FOV when scope is active")]
    public float scopeFOV;
    [Tooltip("Input sensetivity when player aiming")]
    public float scopeSensitivityX;
    [Tooltip("Input sensetivity when player aiming")]
    public float scopeSensitivityY;

    [Header("Melee settings (for melee only!)")]
    public float meleeAttackDistance;
    public float meleeAttackRate;
    public int meleeDamagePoints;
    public float meleeRigidbodyHitForce;
    public float meleeHitTime;
    public string meleeHitFX;

    [Header("Ballistic settings")]
    [Tooltip("Initial bullet velocity in meters per second. Recomended to take real weapons parameters")]
    public float bulletInitialVelocity;
    [Tooltip("Air resistance force that slow down bullet in air. Default value is 1. If 0 no air resistance applied to bullet")]
    public float airResistanceForce;
    [Tooltip("Projectile prefab used as projectile. Select one from prefabs")]
    public string projectile;
    [Tooltip("Max amount of projectiles which being created on Start()")]
    public GameObject PrefabArmsWeapon;

    public DataArmsWeapon()
    {
        this.GuidId = System.Guid.NewGuid().ToString();
    }
}

[System.Serializable]
public struct ContantsRecoil
{
    public Vector2 recolTop;
    public Vector2 recolDown;
}


