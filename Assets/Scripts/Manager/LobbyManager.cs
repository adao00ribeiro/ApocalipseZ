using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using ApocalipseZ;
using GameKit.Utilities.Types;
using FishNet.Managing;
public class LobbyManager : MonoBehaviour
{
    /// <summary>

    public GameObject model;

    public void SpawCharacter(string nameCharacter)
    {
        if (model)
        {
            Destroy(model);
        }
        DataCharacter cha = GameController.Instance.DataManager.GetDataCharacter(nameCharacter);
        if (cha)
        {

            model = Instantiate(cha.PrefabCharacter);
            model.transform.position = GameController.Instance.PlayerSpawPoints.GetPointSpaw();
        }
    }

    public void SelectCharacter(string name)
    {
        PlayerPrefs.SetString("NamePlayer", name);
        SpawCharacter(name);
    }

}
