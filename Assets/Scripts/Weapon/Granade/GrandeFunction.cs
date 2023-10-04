using System.Collections;
using System.Collections.Generic;
using FishNet;
using UnityEngine;

public class GrandeFunction : MonoBehaviour
{
    public GameObject PrefabGranade;
    public Transform muzzle;
    public void ThrowGrenade()
    {

        if (InstanceFinder.NetworkManager.IsServer)
        {
            IProjectile go = Instantiate(PrefabGranade, muzzle.position, muzzle.rotation).GetComponent<IProjectile>();
            go.Initialize(0, 50);
            InstanceFinder.ServerManager.Spawn(go.gameobject);
        }

    }
}
