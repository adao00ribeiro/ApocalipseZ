using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public struct UISlotItemTemp
{
    public int slotIndex;
    public TypeContainer type;
    public SlotInventoryTemp slot;
    public UISlotItemTemp ( int _id , TypeContainer _type , SlotInventoryTemp _slot )
    {
        slotIndex = _id;
        type = _type;
        slot = _slot;
    }
}
public static class UISlotItemReadWrite 
{
    public static void WriteStringTest ( this NetworkWriter writer , UISlotItemTemp value )
    {
        writer.WriteInt ( value.slotIndex );
        writer.WriteByte ( (byte)value.type );
        writer.Write ( value.slot );
    }
    public static UISlotItemTemp ReadStringTest ( this NetworkReader reader )
    {
        return new UISlotItemTemp ( reader.ReadInt ( ) ,( TypeContainer ) reader.ReadByte () ,reader.Read<SlotInventoryTemp> ( ) );
    }
}