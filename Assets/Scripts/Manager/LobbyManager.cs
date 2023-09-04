using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using ApocalipseZ;
public class LobbyManager : MonoBehaviour
{
    public Transform pointSpaw;
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
            model = Instantiate(cha.PrefabCharacter, pointSpaw);
        }
    }

    public void SelectCharacter(string name)
    {
        PlayerPrefs.SetString("NamePlayer", name);
        SpawCharacter(name);
    }

}
