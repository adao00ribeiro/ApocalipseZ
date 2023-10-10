using System.Collections;
using System.Collections.Generic;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

[RequireComponent(typeof(NetworkTransform))]
public class Flag : NetworkBehaviour, IInteract
{
    private PVPFLAGManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindFirstObjectByType<PVPFLAGManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    [ServerRpc(RequireOwnership = false)]
    public void CmdInteract(NetworkConnection sender = null)
    {
        OnInteract(sender.FirstObject.GetComponent<PlayerController>().GetPlayer());
    }

    public void EndFocus()
    {

    }

    public string GetTitle()
    {
        return "Flag";
    }

    public void OnInteract(IFpsPlayer player)
    {
        GetComponent<Rigidbody>().useGravity = false;
        player.SetFlag(this);
    }

    public void StartFocus()
    {

    }


}
