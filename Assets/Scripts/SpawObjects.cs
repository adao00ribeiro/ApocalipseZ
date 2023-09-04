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
public class SpawObjects : MonoBehaviour
{
    private float timeSpaw;
    private void Start()
    {
        if( InstanceFinder.IsClient){
            Destroy(gameObject);
            return;
        }
        foreach (Transform item in transform)
        {
            PointItem point = item.gameObject.GetComponent<PointItem>();
           
            GameController.Instance.TimerManager.Add(() =>
            {
                Spawn(point.GetPrefab(), point.transform.position);
                Destroy(point.gameObject);
            }, Random.Range(1, 20));
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
