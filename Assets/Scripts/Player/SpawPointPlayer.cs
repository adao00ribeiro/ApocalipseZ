using ApocalipseZ;
using FishNet;
using UnityEngine;

public class SpawPointPlayer : MonoBehaviour
{
    public void Start()
    {
        if (InstanceFinder.NetworkManager.IsClient)
        {
            Destroy(gameObject);
        }
    }
    public void OnEnable()
    {
        GameController.Instance.PlayerSpawPoints.Add(this.gameObject);
    }
    public void OnDisable()
    {
        GameController.Instance.PlayerSpawPoints.Remove(this.gameObject);
    }
}
