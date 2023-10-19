using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApocalipseZ;
using FishNet.Broadcast;
using FishNet;
using System;
using Random = UnityEngine.Random;
using FishNet.Managing.Scened;
using FishNet.Connection;
using UnityEngine.SceneManagement;
using System.Linq;
public struct SpawObjectTransform
{
    public string guidid;
    public Vector3 position;
    public Quaternion rotation;
}
public struct ConnectMessage : IBroadcast
{
    public int score;
    public Vector3 scorePos;
    public int lives;
}
public class SpawObjectsManager : MonoBehaviour
{
    [SerializeField] private PointItem[] ListPointItems;
    [SerializeField] private List<Item> ListItems;

    [Header("Config")]
    [SerializeField] private float MaxSpawItems;
    [SerializeField] private float TimeSpaw;
    float currentTimeSpaw;
    private float ItemTimeInTheWorldMinutes = 10;

    [Header("Scene Info")]
    public int currentSceneHandle;
    public string currentSceneName;
    private void Start()
    {
        /*
        if (InstanceFinder.IsClient)
        {
            Destroy(gameObject);
            return;
        }
*/
        Scene cena = gameObject.scene;
        ListPointItems = cena.GetRootGameObjects()
        .SelectMany(root => root.GetComponentsInChildren<PointItem>())
        .ToArray();



    }
    public void OnEnable()
    {
        currentSceneHandle = gameObject.scene.handle;
        currentSceneName = gameObject.scene.name;

        GameController.Instance.AddSpawObjectsManager(this);
    }
    public void OnDisable()
    {
        GameController.Instance.RemoveSpawObjectsManager(this);
    }
    public void SpawTimeObject()
    {

        int randnumber = Random.Range(0, ListPointItems.Length - 1);
        GameController.Instance.TimerManager.Add(() =>
        {
            Spawn(ListPointItems[randnumber].GetPrefab(), ListPointItems[randnumber].transform);
        }, Random.Range(0, TimeSpaw));

    }
    public void InitSpawPvpFlag()
    {
        foreach (var item in ListPointItems)
        {

            Spawn(item.GetPrefab(), item.transform);

        }
    }
    public void InitSpaw()
    {
        foreach (var item in ListPointItems)
        {
            GameController.Instance.TimerManager.Add(() =>
            {
                Spawn(item.GetPrefab(), item.transform);
                //  Destroy(item.gameObject);
            }, Random.Range(1, 20));
        }
    }
    public void Spawn(GameObject prefab, Transform pointSpawn)
    {
        if (prefab == null)
        {
            return;
        }
        if (ListItems.Count > MaxSpawItems)
        {
            return;
        }
        Item tempItem = Instantiate(prefab, pointSpawn.position, pointSpawn.rotation, pointSpawn).GetComponent<Item>();
        tempItem.SetSpawObjectManager(this);
        InstanceFinder.ServerManager.Spawn(tempItem.gameObject);
        AddItem(tempItem);
    }
    public void AddItem(Item Item)
    {
        ListItems.Add(Item);
    }
    public void RemoveItem(Item Item)
    {
        ListItems.Remove(Item);
    }
}
