using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public const string SCENE_NAME = "NewScene";

    private void OnTriggerEnter(Collider other)
    {

        PlayerController nob = other.GetComponentInParent<PlayerController>();

        print(nob.gameObject.name);
        if (nob != null)
        {
            LoadScene(nob.NetworkObject);
        }
    }

    private void LoadScene(NetworkObject nob)
    {
        if (!nob.Owner.IsActive)
        {
            return;
        }

        //SceneLoadData sld = new SceneLoadData(SCENE_NAME);
        SceneLookupData lookup = new SceneLookupData(_stackedSceneHandle, SCENE_NAME);
        SceneLoadData sld = new SceneLoadData(lookup);
        sld.Options.AllowStacking = false;
        sld.MovedNetworkObjects = new NetworkObject[] { nob };
        sld.ReplaceScenes = ReplaceOption.None;
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    public bool SceneStack = false;
    private int _stackedSceneHandle = 0;

    private void Start()
    {
        InstanceFinder.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;
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
