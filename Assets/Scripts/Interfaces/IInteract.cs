using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
public interface IInteract
{

    void CmdInteract(NetworkConnection sender = null);

    void OnInteract(IFpsPlayer player);

    void StartFocus();

    void EndFocus();
    string GetTitle();
}
