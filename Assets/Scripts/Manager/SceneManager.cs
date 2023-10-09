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
    public class SceneManager : MonoBehaviour
    {
        [SerializeField, Scene]
        public List<string> ScenesLoadedGlobal = new();
        public List<PVPFLAGManager> ScenesLoadedPvpFlag = new();
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
                if (scene.name != "SceneFlagTest")
                {
                    ScenesLoadedGlobal.Add(scene.name);
                }

            }
        }
        public void OnDisable()
        {
            InstanceFinder.SceneManager.OnLoadEnd -= RegisterScenes;
        }
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

        internal void AddSceneLoaderPvpFlag(PVPFLAGManager loader)
        {
            ScenesLoadedPvpFlag.Add(loader);
        }
        internal void RemoveSceneLoaderPvpFlag(PVPFLAGManager loader)
        {
            ScenesLoadedPvpFlag.Remove(loader);
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

        internal void AddScenePvpFlag(NetworkConnection[] conns, int index)
        {
            SceneLookupData SceneLook = new SceneLookupData(ScenesLoadedPvpFlag[index].gameObject.scene.handle);
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
            for (int i = 0; i < conns.Length; i++)
            {
                ScenesLoadedPvpFlag[index].OnPlayer?.Invoke(conns[i].FirstObject.GetComponent<PlayerController>());
            }

        }
    }
}