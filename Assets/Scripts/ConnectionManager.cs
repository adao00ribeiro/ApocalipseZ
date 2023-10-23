using System;
using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    public void Add(TKey key, TValue value)
    {
        keys.Add(key);
        values.Add(value);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = keys.IndexOf(key);
        if (index >= 0)
        {
            value = values[index];
            return true;
        }
        else
        {
            value = default(TValue);
            return false;
        }
    }
}
public class ConnectionManager : NetworkBehaviour
{
    public Action<int, PlayerController> OnPlayer;
    public Dictionary<int, PlayerController> players = new Dictionary<int, PlayerController>();

    public List<int> visuPLayerKeys = new List<int>();
    public List<PlayerController> visuPLayer = new List<PlayerController>();

    void Start()
    {

    }
    void OnEnable()
    {

        if (gameObject.scene.name == "CenaC")
        {
            GameController.Instance.ConnectionManager = this;
            OnPlayer += OnChangeOnPlayer; ;
        }
        GameController.Instance.SceneManager.AddSceneLoader(gameObject.scene.name, gameObject.scene.handle, this);
    }
    void OnDisable()
    {

        if (gameObject.scene.name == "CenaC")
        {

            OnPlayer -= OnChangeOnPlayer; ;
        }
        GameController.Instance.SceneManager.RemoveSceneLoader(gameObject.scene.handle);
    }
    private void OnChangeOnPlayer(int clientId, PlayerController controller)
    {

        controller.SpawPlayer();
    }
    public void AddConnection(int id, PlayerController player)
    {
        players.Add(id, player);
        visuPLayer.Add(player);
        visuPLayerKeys.Add(id);
    }

    public void RemoveConnection(int id)
    {
        //  players.Remove(id);
    }

    public PlayerController GetPlayerController(int id)
    {
        PlayerController player = null;
        players.TryGetValue(id, out player);
        return player;
    }
}
