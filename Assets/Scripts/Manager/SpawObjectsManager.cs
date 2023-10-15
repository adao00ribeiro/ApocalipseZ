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
    [SerializeField] private PointItem[] ListPointItems ;

    [Header("Config")]
    [SerializeField]private float TimeSpaw;
    float currentTimeSpaw;


    [Header("Scene Info")]
    public int currentSceneHandle;
    public string currentSceneName;
    private void Start()
    {
        if (InstanceFinder.IsClient)
        {
            Destroy(gameObject);
            return;
        }

    }
    public void OnEnable()
    {
        currentSceneHandle = gameObject.scene.handle;
        currentSceneName = gameObject.scene.name;
        ListPointItems = GameObject.FindObjectsByType<PointItem>(FindObjectsSortMode.None);
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
            //  Destroy(item.gameObject);
        }, Random.Range(1, 20));

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
        GameObject treeGo = Instantiate(prefab, pointSpawn.position, pointSpawn.rotation);
        treeGo.GetComponent<Item>().SetSpawObjectManager(this);
        treeGo.GetComponent<Item>().IsServerSpaw = true;
        InstanceFinder.ServerManager.Spawn(treeGo);
    }
  
}
