using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
public class LobbyManager : MonoBehaviour
{
    public void SelectSwat()
    {
        PlayerPrefs.SetString("NamePlayer", "Cube");

    }
    public void SelectPedro()
    {
        PlayerPrefs.SetString("NamePlayer", "Capsule");
    }
}
