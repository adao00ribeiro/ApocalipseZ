
using System;
using System.Collections.Generic;
using FishNet.Managing.Scened;
using FishNet.Object;
using GameKit.Utilities.Types;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerSceneNetworkManager : NetworkBehaviour
{
        [SerializeField, Scene]
        private List<string> ListScenes = new List<string>();

        public override void OnStartServer()
        {
            foreach (string item in ListScenes)
            {
                SceneLookupData lookupData = new SceneLookupData(item);
                SceneLoadData sld = new SceneLoadData(lookupData)
                {
                    Options = new LoadOptions
                    {
                        AutomaticallyUnload = false
                    },
                };

                base.SceneManager.LoadConnectionScenes(sld);
            }
        }

    public void UpdateSceneList()
    {
        // Adicione as cenas do arquivo de configuração de build à lista
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            // Verifique se o nome da cena contém "Assets/Scenes/Tchanks"
            if (scenePath.Contains("Assets/Scenes/Tchanks"))
            {
                ListScenes.Add(sceneName);
            }
        }
    }
}

