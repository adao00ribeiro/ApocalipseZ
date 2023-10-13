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
using System.Linq;

namespace ApocalipseZ
{
    [System.Serializable]
    public struct SceneData
    {

        [SerializeField, Scene]
        public string Name;
        public int handle;
        public ConnectionManager ConnectionManager;
        public SceneData(string _name, int _handle, ConnectionManager _loader) : this()
        {
            this.Name = _name;
            this.handle = _handle;
            this.ConnectionManager = _loader;
        }
    }
    public class SceneManager : MonoBehaviour
    {
        public List<SceneData> ScenesLoaded = new();
        void OnEnable()
        {
            InstanceFinder.SceneManager.OnClientPresenceChangeStart += SceneManager_OnClientPresenceChangeStart; ;

        }
        void OnDisable()
        {
            InstanceFinder.SceneManager.OnClientPresenceChangeStart -= SceneManager_OnClientPresenceChangeStart; ;
        }

        private void SceneManager_OnClientPresenceChangeStart(ClientPresenceChangeEventArgs args)
        {
            PlayerController controller = GameController.Instance.ConnectionManager.GetPlayerController(args.Connection.ClientId);

            if (controller == null)
            {
                return;
            }

            if (controller.CurrentScene == args.Scene.handle)
            {
                foreach (SceneData item in ScenesLoaded)
                {

                    if (controller.CurrentScene == item.handle)
                    {
                        item.ConnectionManager?.OnPlayer(args.Connection.ClientId, controller);
                        break;
                    }

                }
            }
        }

        public void LoadSceneC(NetworkObject nob)
        {
            if (!nob.Owner.IsActive)
            {
                return;
            }
            SceneLookupData SceneLook = new SceneLookupData("CenaC");
            List<NetworkObject> objects = new List<NetworkObject>();

            SceneLoadData sld = new SceneLoadData(SceneLook)
            {
                Options = new LoadOptions()
                {
                    AutomaticallyUnload = false,
                    AllowStacking = false,
                },
                MovedNetworkObjects = new NetworkObject[] { nob },
                ReplaceScenes = ReplaceOption.All,
                PreferredActiveScene = SceneLook,
            };
            InstanceFinder.SceneManager.LoadConnectionScenes(nob.Owner, sld);
        }


        internal void LoadScene(NetworkObject nob, bool IsPvpScene, string currentScene, int _stackedSceneHandle, string[] ArrayScenes, bool SceneStack, bool AutomaticallyUnload)
        {

            if (!nob.Owner.IsActive)
            {
                return;
            }
            UnloadScene(nob, _stackedSceneHandle, ArrayScenes);
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

        internal void UnloadScene(NetworkObject nob, int _stackedSceneHandle, string[] ArrayScenes)
        {
            List<string> removeScenes = new List<string>();

            foreach (var pair in InstanceFinder.SceneManager.SceneConnections)
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
            if (filteredScenes.Count == 0)
            {
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
                    Mode = UnloadOptions.ServerUnloadMode.UnloadUnused
                }
            };

            InstanceFinder.SceneManager.UnloadConnectionScenes(nob.Owner, sud);
        }
        private void UnloadPvpScene(List<NetworkConnection> grupo)
        {
            List<string> removeScenes = new List<string>();

            foreach (var pair in InstanceFinder.SceneManager.SceneConnections)
            {
                removeScenes.Add(pair.Key.name);
            }

            List<SceneLookupData> ListSceneLook = new List<SceneLookupData>();
            foreach (var item in removeScenes)
            {
                SceneLookupData lookupData = new SceneLookupData(0, item);
                ListSceneLook.Add(lookupData);
            }

            SceneUnloadData sud = new SceneUnloadData(ListSceneLook.ToArray())
            {
                Options = new UnloadOptions()
                {
                    Mode = UnloadOptions.ServerUnloadMode.KeepUnused
                }
            };

            InstanceFinder.SceneManager.UnloadConnectionScenes(grupo.ToArray(), sud);
        }
        internal void AddSceneLoader(string nameScene, int handle, ConnectionManager loader)
        {
            foreach (var item in ScenesLoaded)
            {
                if (item.handle == handle)
                {
                    return;
                }
            }
            ScenesLoaded.Add(new SceneData(nameScene, handle, loader));
        }
        internal void RemoveSceneLoader(int handle)
        {


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
        public void CreateFlagPvpConn(List<NetworkConnection> grupo)
        {


            List<NetworkObject> objects = new List<NetworkObject>();
            for (int i = 0; i < grupo.Count; i++)
            {
                grupo[i].FirstObject.GetComponent<PlayerController>().DespawnPlayer();
                grupo[i].FirstObject.GetComponent<PlayerController>().IsLoading = true;
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
            UnloadPvpScene(grupo);
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