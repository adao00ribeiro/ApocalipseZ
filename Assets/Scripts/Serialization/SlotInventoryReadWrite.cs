using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
using System;

[System.Serializable]
public struct SlotInventoryTemp
{
    public string Name ;
    public string guidid;
    public int Ammo;
    public int Quantity;
    public SlotInventoryTemp(String none = "None")
    {
        Name = "";
        guidid = "";
        Ammo = 0;
        Quantity = 0;
    }
    public SlotInventoryTemp(string _name , string _guidid, int _ammo, int _Quantity)
    {
        Name = _name;
        guidid = _guidid;
        Ammo = _ammo;
        Quantity = _Quantity;
    }

   
    public bool Compare(SlotInventoryTemp other)
    {
        if (Name==other.Name &&guidid == other.guidid && Ammo == other.Ammo && Quantity == other.Quantity)
        {
            return true;
        }

        return false;
    }
}
public static class SlotInventoryReadWrite
{
    public static void WriteStringTest(this NetworkWriter writer, SlotInventoryTemp value)
    {
     writer.WriteString(value.Name);
        writer.WriteString(value.guidid);
        writer.WriteInt(value.Ammo);
        writer.WriteInt(value.Quantity);
    }
    public static SlotInventoryTemp ReadStringTest(this NetworkReader reader)
    {

        return new SlotInventoryTemp(reader.ReadString(),reader.ReadString(), reader.ReadInt(), reader.ReadInt());

    }
}
