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
        public PVPFLAGManager flagPvpManager;

        [SerializeField, Scene]
        public string[] ArrayScenes;

        public bool SceneStack = false;
        protected int _stackedSceneHandle = 0;

        public bool IsPvpScene;
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
            GameController.Instance.SceneManager.AddSceneLoader(gameObject.scene.name, gameObject.scene.handle, this);


            InstanceFinder.SceneManager.OnClientPresenceChangeStart += SceneManager_OnClientPresenceChangeStart; ;
        }

        private void SceneManager_OnClientPresenceChangeStart(ClientPresenceChangeEventArgs args)
        {
            if (flagPvpManager != null)
            {
                flagPvpManager.OnPlayer?.Invoke(args.Connection.FirstObject.GetComponent<PlayerController>());
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
        protected void OnDisable()
        {
            GameController.Instance.SceneManager.RemoveSceneLoader(this);
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
                GameController.Instance.SceneManager.LoadScene(
                    nob.NetworkObject, IsPvpScene, gameObject.scene.name, _stackedSceneHandle, ArrayScenes, SceneStack, IsPvpScene
                    );
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
                // GameController.Instance.SceneManager.UnloadScene(nob.NetworkObject);
            }
        }
    }


}