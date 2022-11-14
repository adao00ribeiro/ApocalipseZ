using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public interface IFpsPlayer
{

    IMoviment GetMoviment();
    PlayerStats GetPlayerStats();

    [Command]
    void CmdSpawBullet(SpawBulletTransform spawBulletTransform, NetworkConnectionToClient networkConnectionToClient);
    FirstPersonCamera GetFirstPersonCamera();
    WeaponManager GetWeaponManager();
    Inventory GetInventory();
}