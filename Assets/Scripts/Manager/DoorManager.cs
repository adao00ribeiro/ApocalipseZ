using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;

using FishNet.Object;
using UnityEngine;

public class DoorManager : NetworkBehaviour
{
    public RotationOjects[] ListDoor;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        ListDoor = GameObject.FindObjectsByType<RotationOjects>(FindObjectsSortMode.None);
        foreach (var item in ListDoor)
        {
            item.SetDoorManager(this);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void CmdInteract(string id)
    {
        ObserversInteract(id);
    }
    [ObserversRpc(ExcludeOwner = false)]
    public void ObserversInteract(string id)
    {
        foreach (var item in ListDoor)
        {
            if (item.guidid == id)
            {
                item.Open();
                break;
            }
        }
    }
}
