using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public List<Scene> ScenesLoaded = new();
    string[] scenesToLoad = new string[] { "NewScene", "Cena3" };
    // Start is called before the first frame update
    public void OnEnable()
    {
        InstanceFinder.SceneManager.OnLoadEnd += RegisterScenes;
        
    }
    public void RegisterScenes(SceneLoadEndEventArgs args)
    {
        //Only Register on Server
        if (!args.QueueData.AsServer) return;

        //if you know you only loaded one scene you could just grab index [0]
        foreach (var scene in args.LoadedScenes)
        {
            ScenesLoaded.Add(scene);
        }
      

    }
    public void OnDisable()
    {
        InstanceFinder.SceneManager.OnLoadEnd -= RegisterScenes;
    }

}
