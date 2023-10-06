using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PVPManager : NetworkBehaviour
{
    [SerializeField] List<NetworkConnection> ListEspera = new List<NetworkConnection>();

    [SerializeField] private int MaxPlayerPvpFlag;
    public List<SceneLoader> ScenesLoadedPvpFlag = new();
    public bool SceneStack = false;
    private int _stackedSceneHandle = 0;


    void Start()
    {
        if (base.IsServer)
        {
            InvokeRepeating("InvokeReunirPlayer", 5, 5);
        }

    }
    public void InvokeReunirPlayer()
    {
        print("Reunindo player");
        StartCoroutine(ReunirPlayer());
    }
    IEnumerator ReunirPlayer()
    {
        if (ListEspera.Count == MaxPlayerPvpFlag)
        {
            yield return new WaitForSeconds(2);

            LoadScenePvpFlag();

        }
        yield return new WaitForEndOfFrame();
    }
    void OnEnable()
    {
        GameController.Instance.PvpManager = this;
    }

    void Update()
    {


    }

    [ServerRpc(RequireOwnership = false)]
    public void CmdAddWaitinLine(NetworkConnection conn = null)
    {
        ListEspera.Add(conn);
    }
    [ServerRpc(RequireOwnership = false)]
    public void CmdRemoveWaitinLine(NetworkConnection conn = null)
    {
        ListEspera.Remove(conn);
    }
    public void LoadScenePvpFlag()
    {
        List<NetworkConnection> grupo = new List<NetworkConnection>();
        List<NetworkObject> objetos = new List<NetworkObject>();
        for (int i = 0; i < MaxPlayerPvpFlag; i++)
        {
            grupo.Add(ListEspera[i]);
            objetos.Add(ListEspera[i].FirstObject);
            ListEspera[i].FirstObject.GetComponent<PlayerController>().GetPlayer().GetPlayerStats().TakeDamage(300);
            ListEspera[i].FirstObject.GetComponent<PlayerController>().currentScene = "SceneFlagTest";
            ListEspera.RemoveAt(i);
        }
        SceneLookupData SceneLook = new SceneLookupData("SceneFlagTest");
        List<NetworkObject> objects = new List<NetworkObject>();
        SceneLoadData sld = new SceneLoadData(SceneLook)
        {
            Options = new LoadOptions()
            {
                AutomaticallyUnload = true,
                AllowStacking = true,
                LocalPhysics = LocalPhysicsMode.Physics3D
            },
            MovedNetworkObjects = objetos.ToArray(),
            ReplaceScenes = ReplaceOption.All,
            PreferredActiveScene = SceneLook

        };
        base.SceneManager.LoadConnectionScenes(grupo.ToArray(), sld);
    }

    internal void AddSceneLoaderPvpFlag(SceneLoader loader)
    {
        ScenesLoadedPvpFlag.Add(loader);
    }
    internal void RemoveSceneLoaderPvpFlag(SceneLoader loader)
    {
        ScenesLoadedPvpFlag.Remove(loader);
    }
}



