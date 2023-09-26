using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApocalipseZ;
using FishNet.Object;
using FishNet.Connection;
public interface IFpsPlayer
{
    Moviment GetMoviment();
    PlayerStats GetPlayerStats();
    void CmdSpawBullet(SpawBulletTransform spawBulletTransform, NetworkConnection networkConnectionToClient);
    FirstPersonCamera GetFirstPersonCamera();
    WeaponManager GetWeaponManager();
    Inventory GetInventory();
}