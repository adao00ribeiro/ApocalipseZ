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
    [SerializeField] private List<PointItem> ListPointItems = new List<PointItem>();
    private float timeSpaw;
    private void Start()
    {
        if (InstanceFinder.IsClient)
        {
            Destroy(gameObject);
            return;
        }

    }
    
    public void SpawTimeObject(){

            int randnumber = Random.Range(0 ,ListPointItems.Count  - 1);
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
        treeGo.GetComponent<Item>().IsServerSpaw = true;
        InstanceFinder.ServerManager.Spawn(treeGo);
    }
    internal void Add(PointItem pointItem)
    {
        ListPointItems.Add(pointItem);
    }

    internal void Remove(PointItem pointItem)
    {
        ListPointItems.Remove(pointItem);
    }
}
