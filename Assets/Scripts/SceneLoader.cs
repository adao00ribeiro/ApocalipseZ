using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Utility.Extension;
using Unity.VisualScripting;
using UnityEngine;


public class SceneLoader : NetworkBehaviour
{

    public string[] ArrayScenes;
    List<NetworkConnection> ListConns = new List<NetworkConnection>();
    private void OnTriggerEnter(Collider other)
    {
        if (!base.IsServer)
        {
            return;
        }
        PlayerController nob = other.GetComponentInParent<PlayerController>();


        if (nob != null)
        {
            LoadScene(nob.NetworkObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!base.IsServer)
        {
            return;
        }

        PlayerController nob = other.GetComponentInParent<PlayerController>();
        if (nob != null)
        {
           // UnloadScene(nob.NetworkObject);
        }
    }

    public void LoadScene(NetworkObject nob)
    {
        if (!nob.Owner.IsActive)
        {
            return;
        }
        UnloadScene(nob);
        List<SceneLookupData> ListSceneLook = new List<SceneLookupData>();
        ListSceneLook.Add(new SceneLookupData(_stackedSceneHandle, "CenaC"));
        ListSceneLook.Add(new SceneLookupData(_stackedSceneHandle, gameObject.scene.name));

        foreach (var item in ArrayScenes)
        {
            SceneLookupData lookupData = new SceneLookupData(_stackedSceneHandle, item);
            ListSceneLook.Add(lookupData);
        }
        //SceneLoadData sld = new SceneLoadData(SCENE_NAME);
        SceneLoadData sld = new SceneLoadData(ListSceneLook.ToArray())
        {
            Options = new LoadOptions()
            {
                AutomaticallyUnload = false,
                AllowStacking = false,
            },
            MovedNetworkObjects = new NetworkObject[] { nob },
            ReplaceScenes = ReplaceOption.None,
            PreferredActiveScene = ListSceneLook.ToArray()[0],
        };
        base.SceneManager.LoadConnectionScenes(nob.Owner, sld);

    }
    public void UnloadScene(NetworkObject nob)
    {
        List<string> removeScenes = new List<string>();
        foreach (var pair in SceneManager.SceneConnections)
        {
            removeScenes.Add(pair.Key.name);
        }
        removeScenes.Remove("CenaC");
        List<string> ListScenes = ArrayScenes.ToList();
        List<string> filteredScenes = new List<string>();
       ListScenes.Add(gameObject.scene.name);
        foreach (var item in removeScenes)
        {
            if (!ListScenes.Contains(item))
            {
               filteredScenes.Add(item);
            }
        }   
        foreach (var pair in filteredScenes)
        {
            print(pair);
        }
        if(filteredScenes.Count ==0 ){
            return;
        }
        List<SceneLookupData> ListSceneLook = new List<SceneLookupData>();
        foreach (var item in filteredScenes)
        {
           print(item);
            SceneLookupData lookupData = new SceneLookupData(_stackedSceneHandle, item);
            ListSceneLook.Add(lookupData);
        }

        SceneUnloadData sud = new SceneUnloadData(ListSceneLook.ToArray())
        {
            Options = new UnloadOptions()
            {
                Mode = UnloadOptions.ServerUnloadMode.KeepUnused
            }
        };

        base.SceneManager.UnloadConnectionScenes(nob.Owner , sud);
    }
    public bool SceneStack = false;
    private int _stackedSceneHandle = 0;

    private void Start()
    {
        if (base.SceneManager != null)
        {
            base.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;
            base.SceneManager.OnClientPresenceChangeStart += SceneManager_OnClientPresenceChangeStart;
            base.SceneManager.OnClientPresenceChangeEnd += SceneManager_OnClientPresenceChangeEnd;
        }
    }

    private void SceneManager_OnClientPresenceChangeStart(ClientPresenceChangeEventArgs obj)
    {

        ListConns.Add(obj.Connection);
    }
    private void SceneManager_OnClientPresenceChangeEnd(ClientPresenceChangeEventArgs obj)
    {

        ListConns.Remove(obj.Connection);
    }
    private void SceneManager_OnLoadEnd(SceneLoadEndEventArgs obj)
    {
        if (!obj.QueueData.AsServer)
        {
            return;
        }
        if (!SceneStack)
        {
            return;
        }
        if (_stackedSceneHandle != 0)
        {
            return;
        }

        if (obj.LoadedScenes.Length > 0)
        {
            _stackedSceneHandle = obj.LoadedScenes[0].handle;
        }
    }

}
