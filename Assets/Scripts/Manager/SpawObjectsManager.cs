using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ApocalipseZ;
using FishNet.Object;
using FishNet.Broadcast;
using FishNet;

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
    private float timeSpaw;
    private void Start()
    {
        if (InstanceFinder.IsClient)
        {
            Destroy(gameObject);
            return;
        }
        PointItem[] ListPointItems = GameObject.FindObjectsByType<PointItem>(FindObjectsSortMode.None);

        foreach (var item in ListPointItems)
        {
            /*
            GameController.Instance.TimerManager.Add(() =>
            {
                Spawn(item.GetPrefab(), item.transform.position);
                Destroy(item.gameObject);
            }, Random.Range(1, 20));
            */
        }
    }
    public void Spawn(GameObject prefab, Vector3 pointSpawn)
    {
        if (prefab == null)
        {
            return;
        }
        GameObject treeGo = Instantiate(prefab, pointSpawn, Quaternion.identity);
        treeGo.GetComponent<Item>().IsServerSpaw = true;
        InstanceFinder.ServerManager.Spawn(treeGo);

    }



}
