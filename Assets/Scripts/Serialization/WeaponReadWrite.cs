using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Serializing;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Object.Synchronizing;

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
  public class SyncMyContainer : SyncBase, ICustomSync
{
    /* If you intend to serialize your type
    * as a whole at any point in your custom
    * SyncObject and would like the automatic
    * serializers to include it then use
    * GetSerializedType() to return the type.
    * In this case, the type is MyContainer.
    * If you do not need a serializer generated
    * you may return null. */
    public object GetSerializedType() => typeof(WeaponNetwork);
}
    public static class WeaponReadWrite
    {
      
    }
}
