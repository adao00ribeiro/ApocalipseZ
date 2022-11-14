using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Serializing;
using ApocalipseZ;
using System;

[System.Serializable]
public struct SlotInventoryTemp
{
    public string Name;
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
    public SlotInventoryTemp(string _name, string _guidid, int _ammo, int _Quantity)
    {
        Name = _name;
        guidid = _guidid;
        Ammo = _ammo;
        Quantity = _Quantity;
    }


    public bool Compare(SlotInventoryTemp other)
    {
        if (Name == other.Name && guidid == other.guidid && Ammo == other.Ammo && Quantity == other.Quantity)
        {
            return true;
        }

        return false;
    }
}
public static class SlotInventoryReadWrite
{
    public static void WriteStringTest(this Writer writer, SlotInventoryTemp value)
    {
        writer.WriteString(value.Name);
        writer.WriteString(value.guidid);
        writer.WriteInt32(value.Ammo);
        writer.WriteInt32(value.Quantity);
    }
    public static SlotInventoryTemp ReadStringTest(this Reader reader)
    {

        return new SlotInventoryTemp(reader.ReadString(), reader.ReadString(), reader.ReadInt32(), reader.ReadInt32());

    }
}
