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

    public bool CreateScene;

    void Start()
    {
        if (base.IsServer)
        {
            InvokeRepeating("InvokeReunirPlayer", 5, 5);
        }

    }
    void Update()
    {
        if (CreateScene)
        {

            GameController.Instance.SceneManager.CreateFlagPvp();

            CreateScene = false;
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

            AddScenePvpFlag();

        }
        yield return new WaitForEndOfFrame();
    }
    void OnEnable()
    {
        GameController.Instance.PvpManager = this;
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
            ListEspera[i].FirstObject.GetComponent<PlayerController>().DespawnPlayer();
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
    public void AddScenePvpFlag()
    {
        List<NetworkConnection> grupo = new List<NetworkConnection>();
        for (int i = 0; i < MaxPlayerPvpFlag; i++)
        {
            grupo.Add(ListEspera[i]);
            ListEspera.RemoveAt(i);
        }
        GameController.Instance.SceneManager.AddScenePvpFlag(grupo.ToArray(), 1);
    }

}



