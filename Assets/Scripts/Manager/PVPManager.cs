using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;


public class PVPManager : MonoBehaviour
{
    public int TotalConexoes;
    [SerializeField] List<NetworkConnection> ListEspera = new List<NetworkConnection>();
    [SerializeField] private int MaxPlayerPvpFlag;

    public bool CreateScene;

    void Start()
    {
        if (InstanceFinder.IsServer)
        {
            InvokeRepeating("InvokeReunirPlayer", 5, 5);
        }

    }
    void Update()
    {
        if (InstanceFinder.IsServer)
        {
            TotalConexoes = ListEspera.Count;
            if (CreateScene)
            {
                GameController.Instance.SceneManager.CreateFlagPvp();
                CreateScene = false;
            }
        }
    }
    public void InvokeReunirPlayer()
    {
        print("Reunindo player");
        StartCoroutine(ReunirPlayer());
    }
    IEnumerator ReunirPlayer()
    {
        yield return new WaitForSeconds(2);

        if (ListEspera.Count == MaxPlayerPvpFlag)
        {
            CreateScenePvpFlag();
        }
        yield return new WaitForEndOfFrame();
    }
    void OnEnable()
    {
        GameController.Instance.PvpManager = this;
    }


    public void AddWaitinLine(NetworkConnection conn)
    {
        ListEspera.Add(conn);
    }

    public void RemoveWaitinLine(NetworkConnection conn)
    {
        ListEspera.Remove(conn);
    }
    public void CreateScenePvpFlag()
    {
        List<NetworkConnection> grupo = ListEspera.GetRange(0, MaxPlayerPvpFlag);
        GameController.Instance.SceneManager.CreateFlagPvpConn(grupo);
        ListEspera.RemoveRange(0, MaxPlayerPvpFlag);

    }
    public void AddScenePvpFlag()
    {
        List<NetworkConnection> grupo = new List<NetworkConnection>();
        for (int i = 0; i < MaxPlayerPvpFlag; i++)
        {
            grupo.Add(ListEspera[i]);
        }
        GameController.Instance.SceneManager.AddScenePvpFlag(grupo.ToArray(), 1);
        ListEspera.RemoveRange(0, MaxPlayerPvpFlag);
    }

}