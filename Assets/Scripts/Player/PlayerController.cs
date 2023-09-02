using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
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
        NetworkBehaviour go = Instantiate(PrefabPlayer, playerspaw.GetPointSpaw(), Quaternion.identity, transform);
        base.Spawn(go.gameObject, sender);
    }

    public NetworkObject GetPlayer()
    {
        return player;
    }
}
