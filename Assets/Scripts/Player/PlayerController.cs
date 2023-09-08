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
    public string playerName;
    private FpsPlayer player;
    [SerializeField] private NetworkBehaviour PrefabPlayer;
    // Start is called before the first frame update
    public string currentSCENE_NAME;
    public string anteriorSCENE_NAME;
    public override void OnStartServer()
    {
        base.OnStartServer();
        currentSCENE_NAME = this.gameObject.scene.name;
        //playerName = (string)connectionToClient.authenticationData;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            ChatUI.localPlayerName = playerName;
            CmdSpawPlayer();

        }

    }

    [ServerRpc]
    public void CmdSpawPlayer(NetworkConnection sender = null)
    {
        PlayerSpawPointsManager playerspaw = GameController.Instance.PlayerSpawPoints;
        Vector3 novo = playerspaw.GetPointSpaw();
        NetworkBehaviour go = Instantiate(PrefabPlayer, novo, Quaternion.identity);
        player = go.GetComponent<FpsPlayer>();
        go.transform.SetParent(this.transform);
        base.Spawn(go.gameObject, sender);
        ObserverSpawPlayer(go.gameObject);
    }
    [ObserversRpc]
    public void ObserverSpawPlayer(GameObject player)
    {
        player.transform.SetParent(this.transform);

        if (base.IsOwner)
        {
            GameController.Instance.CanvasFpsPlayer.gameObject.transform.SetParent(player.transform);
        }
    }
    public FpsPlayer GetPlayer()
    {
        return player;
    }
}
