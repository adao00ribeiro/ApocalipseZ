using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{

    [SyncVar]
    private NetworkObject player;
    [SerializeField] private NetworkBehaviour PrefabPlayer;
    // Start is called before the first frame update
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            CmdSpawPlayer();

        }

    }

    [ServerRpc]
    public void CmdSpawPlayer(NetworkConnection sender = null)
    {
        PlayerSpawPoints playerspaw = GameController.Instance.PlayerSpawPoints;
        Vector3 novo = playerspaw.GetPointSpaw();
      
        NetworkBehaviour go = Instantiate(PrefabPlayer,novo,Quaternion.identity);
        go.transform.SetParent(this.transform);
        base.Spawn(go.gameObject, sender);


    }
    [ObserversRpc]
    public void ObserverSpawPlayer(GameObject player)
    {
        player.transform.SetParent(this.transform);
    }
    public NetworkObject GetPlayer()
    {
        return player;
    }
}
