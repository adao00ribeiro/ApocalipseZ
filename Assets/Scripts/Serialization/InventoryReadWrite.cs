using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace ApocalipseZ
{
    public struct InventoryTemp
    {
        public List<SlotInventoryTemp> slot;
        public int maxSlot;

        public InventoryTemp ( List<SlotInventoryTemp> _slot , int maxslot )
        {
            slot = _slot;
            maxSlot = maxslot;
        }
    }
    public static class InventoryReadWrite
    {
        public static void WriteInventory ( this NetworkWriter writer , InventoryTemp value )
        {

            writer.WriteList ( value.slot );
            writer.WriteInt ( value.maxSlot );

        }
        public static InventoryTemp ReadInventory ( this NetworkReader reader )
        {

            return new InventoryTemp ( reader.ReadList<SlotInventoryTemp> ( ) , reader.ReadInt ( ) );

        }
    }
}