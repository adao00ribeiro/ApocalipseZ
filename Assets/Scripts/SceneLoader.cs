using FishNet.Managing.Scened;
using FishNet.Object;

using UnityEngine;

public class SceneLoader : NetworkBehaviour
{
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
            UnloadScene(nob.NetworkObject);
        }
    }

    public void LoadScene(NetworkObject nob)
    {
        if (!nob.Owner.IsActive)
        {
            return;
        }
        //SceneLoadData sld = new SceneLoadData(SCENE_NAME);
        SceneLookupData lookupData = new SceneLookupData(_stackedSceneHandle, gameObject.scene.name);
        SceneLoadData sld = new SceneLoadData(lookupData)
        {
            Options = new LoadOptions()
            {
                AutomaticallyUnload = false,
                AllowStacking = false,
            },
            MovedNetworkObjects = new NetworkObject[] { nob },
            ReplaceScenes = ReplaceOption.None,
            PreferredActiveScene = lookupData,
        };

        base.SceneManager.LoadConnectionScenes(nob.Owner, sld);
    }
    public void UnloadScene(NetworkObject nob)
    {
        SceneLookupData lookupData = new SceneLookupData(gameObject.scene.name);

        SceneUnloadData sud = new SceneUnloadData(lookupData)
        {
            Options = new UnloadOptions()
            {
                Mode = UnloadOptions.ServerUnloadMode.KeepUnused
            }
        };

        base.SceneManager.UnloadConnectionScenes(nob.Owner, sud);
    }
    public bool SceneStack = false;
    private int _stackedSceneHandle = 0;

    private void Start()
    {
        if (base.SceneManager != null)
        {
            base.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;
        }
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
