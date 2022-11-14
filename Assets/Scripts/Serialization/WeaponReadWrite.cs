using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Serializing;

namespace ApocalipseZ
{

    [System.Serializable]
    public struct WeaponNetwork
    {
        public int currentAmmo;
        public WeaponNetwork(int current)
        {
            currentAmmo = current;
        }
    }
    public static class WeaponReadWrite
    {
        public static void WriteWeapon(this Writer writer, WeaponNetwork value)
        {
            writer.WriteInt32(value.currentAmmo);
        }
        public static WeaponNetwork ReadWeapon(this Reader reader)
        {

            return new WeaponNetwork(reader.ReadInt32());

        }
    }
}
