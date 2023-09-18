using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DoorManager))]
public class CustomInspectorDoorManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DoorManager manager = (DoorManager)target;

        if (GUILayout.Button("Update List Door"))
        {
            manager.GetDoorInScene();
        }

    }
}
