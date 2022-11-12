using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
using System;

[System.Serializable]
public class SlotInventoryTemp
{

    public string guidid;
    public int Ammo;
    public int Quantity;
    public SlotInventoryTemp()
    {

        guidid = "";
        Ammo = 0;
        Quantity = 0;
    }
    public SlotInventoryTemp(string _guidid, int _ammo, int _Quantity)
    {

        guidid = _guidid;
        Ammo = _ammo;
        Quantity = _Quantity;
    }

    public bool Compare(SlotInventoryTemp other)
    {
        if (guidid == other.guidid && Ammo == other.Ammo && Quantity == other.Quantity)
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

        writer.WriteString(value.guidid);
        writer.WriteInt(value.Ammo);
        writer.WriteInt(value.Quantity);
    }
    public static SlotInventoryTemp ReadStringTest(this NetworkReader reader)
    {

        return new SlotInventoryTemp(reader.ReadString(), reader.ReadInt(), reader.ReadInt());

    }
}
