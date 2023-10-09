using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;
using GameKit.Utilities.Types;
using UnityEngine.SceneManagement;
using FishNet;
using System;
using Unity.VisualScripting;
namespace ApocalipseZ
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField, Scene]
        public string[] ArrayScenes;

        List<NetworkConnection> ListConns = new List<NetworkConnection>();
        public bool SceneStack = false;
        protected int _stackedSceneHandle = 0;
        private string[,] sceneMatrix = new string[10, 8];


        protected virtual void Start()
        {
            /*
            if (InstanceFinder.NetworkManager.IsClient)
            {
                Destroy(gameObject);
                return;
            }
            */
            GetScene();
        }
        protected virtual void OnEnable()
        {
            if (gameObject.scene.name == "SceneFlagTest")
            {
                GameController.Instance.PvpManager.AddSceneLoaderPvpFlag(this);
            }
            if (InstanceFinder.SceneManager != null)
            {
                InstanceFinder.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes; ;
                InstanceFinder.SceneManager.OnQueueStart += SceneManager_OnQueueStart;
                InstanceFinder.SceneManager.OnQueueEnd += SceneManager_OnQueueEnd;
                InstanceFinder.SceneManager.OnLoadStart += SceneManager_OnLoadStart;
                InstanceFinder.SceneManager.OnUnloadStart += SceneManager_OnUnloadStart;
                InstanceFinder.SceneManager.OnLoadPercentChange += SceneManager_OnLoadPercentChange;
                InstanceFinder.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;
                InstanceFinder.SceneManager.OnUnloadEnd += SceneManager_OnUnloadEnd;
                InstanceFinder.SceneManager.OnClientPresenceChangeStart += SceneManager_OnClientPresenceChangeStart;
                InstanceFinder.SceneManager.OnClientPresenceChangeEnd += SceneManager_OnClientPresenceChangeEnd;

            }
        }
        private void SceneManager_OnClientLoadedStartScenes(NetworkConnection connection, bool arg2)
        {

        }
        private void SceneManager_OnQueueStart()
        {

        }
        private void SceneManager_OnQueueEnd()
        {

        }
        private void SceneManager_OnUnloadEnd(SceneUnloadEndEventArgs args)
        {

        }

        private void SceneManager_OnUnloadStart(SceneUnloadStartEventArgs args)
        {

        }

        private void SceneManager_OnLoadStart(SceneLoadStartEventArgs args)
        {

        }
        protected void SceneManager_OnLoadPercentChange(SceneLoadPercentEventArgs args)
        {

        }

        protected void SceneManager_OnClientPresenceChangeStart(ClientPresenceChangeEventArgs obj)
        {

        }
        protected void SceneManager_OnClientPresenceChangeEnd(ClientPresenceChangeEventArgs obj)
        {

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
        protected void OnDisable()
        {
            if (gameObject.scene.name == "SceneFlagTest")
            {
                GameController.Instance.PvpManager.RemoveSceneLoaderPvpFlag(this);
            }
            if (InstanceFinder.SceneManager != null)
            {

                InstanceFinder.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes; ;
                InstanceFinder.SceneManager.OnQueueStart -= SceneManager_OnQueueStart;
                InstanceFinder.SceneManager.OnQueueEnd -= SceneManager_OnQueueEnd;
                InstanceFinder.SceneManager.OnLoadStart -= SceneManager_OnLoadStart;
                InstanceFinder.SceneManager.OnUnloadStart -= SceneManager_OnUnloadStart;
                InstanceFinder.SceneManager.OnLoadPercentChange -= SceneManager_OnLoadPercentChange;
                InstanceFinder.SceneManager.OnLoadEnd -= SceneManager_OnLoadEnd;
                InstanceFinder.SceneManager.OnUnloadEnd -= SceneManager_OnUnloadEnd;
                InstanceFinder.SceneManager.OnClientPresenceChangeStart -= SceneManager_OnClientPresenceChangeStart;
                InstanceFinder.SceneManager.OnClientPresenceChangeEnd -= SceneManager_OnClientPresenceChangeEnd;

            }
        }


        public void LoadScene(NetworkObject nob)
        {

            if (!nob.Owner.IsActive)
            {
                return;
            }
            UnloadScene(nob, "CenaC");
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
            InstanceFinder.SceneManager.LoadConnectionScenes(nob.Owner, sld);

        }
        public void UnloadScene(NetworkObject nob, string noremovecena)
        {
            List<string> removeScenes = new List<string>();

            foreach (var pair in InstanceFinder.SceneManager.SceneConnections)
            {
                removeScenes.Add(pair.Key.name);
            }
            removeScenes.Remove(noremovecena);
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


        public void GetScene()
        {
            int rowIndex = 0;
            int colIndex = 0;
            // Adicione as cenas do arquivo de configuração de build à lista
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                // Verifique se o nome da cena contém "Assets/Scenes/Tchanks"
                if (scenePath.Contains("Assets/Scenes/Tchanks"))
                {
                    // Adicione o nome da cena à matriz
                    sceneMatrix[rowIndex, colIndex] = sceneName;

                    // Avance para a próxima coluna
                    colIndex++;

                    // Se atingir a última coluna, vá para a próxima linha
                    if (colIndex >= 8)
                    {
                        colIndex = 0;
                        rowIndex++;

                        // Se atingir a última linha, saia do loop
                        if (rowIndex >= 10)
                        {
                            break;
                        }
                    }
                }
            }
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    string sceneName = sceneMatrix[row, col];
                    if (!string.IsNullOrEmpty(sceneName))
                    {
                        //  Debug.Log("Scene name at [" + row + "," + col + "]: " + sceneName);
                    }
                }
            }

            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 8; col++)
                {

                    if (sceneMatrix[row, col] == gameObject.scene.name)
                    {

                        ArrayScenes = ElementosAoRedor(sceneMatrix, row, col);
                        break;
                    }
                }
            }



            //ScenesTeSTES = Resources.LoadAll<Object>("SceneTchanks");

            //Object[,] matrix = new Object[8,10];

        }


        string[] ElementosAoRedor(string[,] matriz, int linha, int coluna)
        {
            int linhas = matriz.GetLength(0);
            int colunas = matriz.GetLength(1);
            List<string> elementos = new List<string>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int novaLinha = linha + i;
                    int novaColuna = coluna + j;

                    // Verifique se a nova linha e a nova coluna estão dentro dos limites da matriz
                    if (novaLinha >= 0 && novaLinha < linhas && novaColuna >= 0 && novaColuna < colunas)
                    {
                        if (matriz[linha, coluna] != matriz[novaLinha, novaColuna])
                        {
                            elementos.Add(matriz[novaLinha, novaColuna]);
                        }

                    }
                }
            }

            return elementos.ToArray();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!InstanceFinder.NetworkManager.IsServer)
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
            if (!InstanceFinder.NetworkManager.IsServer)
            {
                return;
            }

            PlayerController nob = other.GetComponentInParent<PlayerController>();
            if (nob != null)
            {
                // UnloadScene(nob.NetworkObject);
            }
        }
    }


}