using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class DeadStatsManager : NetworkBehaviour
{

    [ObserversRpc]
    public void ObserveViewUiDeadStats(string name)
    {
        GameController.Instance.playerController.UiDeadStats.ActiveText(name);
    }
    [TargetRpc]
    public void TargewtViewSeFudeo(NetworkConnection conn)
    {
        GameController.Instance.playerController.UiDeadStats.ActiveFadeImage();
    }
}
