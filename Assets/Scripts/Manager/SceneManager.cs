using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FishNet.Object;
using System;
using FishNet.Managing.Scened;
using FishNet;
using FishNet.Connection;
using GameKit.Utilities.Types;

namespace ApocalipseZ
{
    [System.Serializable]
    public struct SceneData{

        [SerializeField, Scene]
        public string Name;
        public int handle;
        public SceneLoader SceneLoader;
        public SceneData(string _name, int _handle, SceneLoader _loader) : this()
        {
            this.Name = _name;
            this.handle = _handle;
            this.SceneLoader = _loader;
        }
    }
    public class SceneManager : MonoBehaviour
    {
     
        public List<SceneData> ScenesLoaded = new();
      
       
        internal void LoadScene(NetworkObject nob, bool IsPvpScene, string currentScene, int _stackedSceneHandle, string[] ArrayScenes, bool SceneStack, bool AutomaticallyUnload)
        {

            if (!nob.Owner.IsActive)
            {
                return;
            }
            // UnloadScene(nob, "CenaC");
            List<SceneLookupData> ListSceneLook = new List<SceneLookupData>();
            if (!IsPvpScene)
            {
                ListSceneLook.Add(new SceneLookupData(_stackedSceneHandle, "CenaC"));
                ListSceneLook.Add(new SceneLookupData(_stackedSceneHandle, currentScene));
            }

            foreach (var item in ArrayScenes)
            {
                SceneLookupData lookupData = new SceneLookupData(_stackedSceneHandle, item);
                ListSceneLook.Add(lookupData);
            }

            SceneLoadData sld = new SceneLoadData(ListSceneLook.ToArray())
            {
                Options = new LoadOptions()
                {
                    AutomaticallyUnload = AutomaticallyUnload,
                    AllowStacking = SceneStack,
                },
                MovedNetworkObjects = new NetworkObject[] { nob },
                ReplaceScenes = IsPvpScene ? ReplaceOption.All : ReplaceOption.None,
                PreferredActiveScene = ListSceneLook.ToArray()[0],
            };
            InstanceFinder.SceneManager.LoadConnectionScenes(nob.Owner, sld);
        }
        public void LoadScenePvpFlag(List<NetworkConnection> ListEspera, int MaxPlayerPvpFlag)
        {

        }
        internal void UnloadScene(NetworkObject networkObject)
        {
            throw new NotImplementedException();
        }

        internal void AddSceneLoader(string nameScene, int handle ,  SceneLoader loader)
        {
            ScenesLoaded.Add(new SceneData(nameScene , handle , loader));
        }
        internal void RemoveSceneLoader(SceneLoader loader)
        {
            foreach (var item in ScenesLoaded)
            {
                    if(item.SceneLoader == loader){
                        ScenesLoaded.Remove(item);
                    }
            }
        }

        public void CreateFlagPvp()
        {
            SceneLookupData SceneLook = new SceneLookupData("SceneFlagTest");
            SceneLoadData sld = new SceneLoadData(SceneLook)
            {
                Options = new LoadOptions()
                {
                    AutomaticallyUnload = true,
                    AllowStacking = true,
                    LocalPhysics = LocalPhysicsMode.Physics3D
                },
                ReplaceScenes = ReplaceOption.All,
            };
            InstanceFinder.SceneManager.LoadConnectionScenes(sld);
        }
        public void CreateFlagPvpConn(List<NetworkConnection> grupo){
        List<NetworkObject> objects = new List<NetworkObject>();
        for (int i = 0; i < grupo.Count; i++)
        {
            grupo[i].FirstObject.GetComponent<PlayerController>().DespawnPlayer();
            objects.Add(grupo[i].FirstObject);
        }
        SceneLookupData SceneLook = new SceneLookupData("SceneFlagTest");
        SceneLoadData sld = new SceneLoadData(SceneLook)
        {
            Options = new LoadOptions()
            {
                AutomaticallyUnload = true,
                AllowStacking = true,
                LocalPhysics = LocalPhysicsMode.Physics3D
            },
            MovedNetworkObjects = objects.ToArray(),
            ReplaceScenes = ReplaceOption.All,
            PreferredActiveScene = SceneLook

        };
        InstanceFinder.SceneManager.LoadConnectionScenes(grupo.ToArray(), sld);
        }
        internal void AddScenePvpFlag(NetworkConnection[] conns, int index)
        {
           
            SceneLookupData SceneLook = new SceneLookupData(ScenesLoaded[index].handle);
            List<NetworkObject> objects = new List<NetworkObject>();
            for (int i = 0; i < conns.Length; i++)
            {
                conns[i].FirstObject.GetComponent<PlayerController>().DespawnPlayer();
                objects.Add(conns[i].FirstObject);

            }
            SceneLoadData sld = new(SceneLook);
            sld.MovedNetworkObjects = objects.ToArray();
            sld.PreferredActiveScene = SceneLook;
            InstanceFinder.SceneManager.LoadConnectionScenes(conns, sld);
            
       
        }
       
    }
}