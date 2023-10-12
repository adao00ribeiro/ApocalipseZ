using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApocalipseZ;
using FishNet.Object;
using FishNet.Connection;
public interface IFpsPlayer
{
    public GameObject gameobject { get;}
    Moviment GetMoviment();
    PlayerStats GetPlayerStats();
    FirstPersonCamera GetFirstPersonCamera();
    WeaponManager GetWeaponManager();
    Inventory GetInventory();
    void SetFlag(Flag flag);
}