using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SceneLoader))]
public class CustomInspectorSceneLoader : Editor
{
     public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SceneLoader manager = (SceneLoader)target;

        if (GUILayout.Button("Update Scenes"))
        {
            manager.GetScene();
        }

    }
}
