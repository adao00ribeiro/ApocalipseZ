using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using FishNet;
using UnityEngine;

public class ExitScene : MonoBehaviour
{
    public SceneLoader loader;
    private void OnTriggerEnter(Collider other)
    {
        if (!InstanceFinder.NetworkManager.IsServer)
        {
            return;
        }

        PlayerController nob = other.GetComponentInParent<PlayerController>();
        if (nob != null)
        {
            nob.DespawnPlayer();
            GameController.Instance.SceneManager.LoadSceneC(nob.NetworkObject);
        }
    }
}
