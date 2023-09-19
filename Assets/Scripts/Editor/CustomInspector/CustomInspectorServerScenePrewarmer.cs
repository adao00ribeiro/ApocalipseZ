using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ServerSceneNetworkManager))]
public class CustomInspectorServerScenePrewarmer : Editor
{
     public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ServerSceneNetworkManager ServerScenePrewarmer = (ServerSceneNetworkManager)target;

        if (GUILayout.Button("Update Scenes"))
        {
            ServerScenePrewarmer.UpdateSceneList();
        }

    }
}
