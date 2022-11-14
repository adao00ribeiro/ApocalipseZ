using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Serializing;
using ApocalipseZ;
public struct UISlotItemTemp
{
    public int slotIndex;
    public TypeContainer type;
    public SlotInventoryTemp slot;
    public UISlotItemTemp(int _id, TypeContainer _type, SlotInventoryTemp _slot)
    {
        slotIndex = _id;
        type = _type;
        slot = _slot;
    }
}
public static class UISlotItemReadWrite
{
    public static void WriteStringTest(this Writer writer, UISlotItemTemp value)
    {
        writer.WriteInt32(value.slotIndex);
        writer.WriteByte((byte)value.type);
        writer.Write(value.slot);
    }
    public static UISlotItemTemp ReadStringTest(this Reader reader)
    {
        return new UISlotItemTemp(reader.ReadInt32(), (TypeContainer)reader.ReadByte(), reader.Read<SlotInventoryTemp>());
    }
}