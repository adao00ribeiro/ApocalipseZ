using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;
using GameKit.Utilities.Types;
using UnityEngine.SceneManagement;


public class SceneLoader : NetworkBehaviour
{
    [SerializeField, Scene]
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




    public string[,] sceneMatrix = new string[10, 8];
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
}