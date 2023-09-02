using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using Unity.Mathematics;
using UnityEngine;

public class DoorManager : NetworkBehaviour
{

    public RotationOjects[] ListDoor;
    

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        //ListDoor = GameObject.FindObjectsByType<RotationOjects>(FindObjectsSortMode.None);

        for (int i = 0; i < ListDoor.Length; i++)
        {
            ListDoor[i].guidid = i;
            ListDoor[i].SetDoorManager(this);
        }
     
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        CmdUpdateAllDoors();
        
    }
    [ServerRpc(RequireOwnership = false)]
    public void CmdInteract(int id)
    {
        if (!base.IsHost)
        {
            ActionDoor(id);

        }
        ObserversInteract(id);
    }
    [ObserversRpc(ExcludeOwner = false)]
    public void ObserversInteract(int id)
    {
        ActionDoor(id);
    }

    public void ActionDoor(int id)
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

   [ServerRpc(RequireOwnership = false)]
    public void CmdUpdateAllDoors(NetworkConnection conn = null)
    {
        print("chamandoupdate"+ conn.ClientId);
        List<int> ListOpenDoor = new List<int>();

        foreach (var item in ListDoor)
        {
            if (item.IsOpen)
            {
                ListOpenDoor.Add(item.guidid);
            }
        }

        TargetUpdateAllDoors(conn, ListOpenDoor);
    }

    [TargetRpc]
    public void TargetUpdateAllDoors(NetworkConnection conn, List<int> ListOpenDoor)
    {
        print("target" + ListOpenDoor.Count);
        foreach (var item in ListOpenDoor)
        {
            ActionDoor(item);
        }
    }
}
