using ApocalipseZ;
using FishNet;
using UnityEngine;

public class SpawPointPlayer : MonoBehaviour
{

    public string currentScene;
    public int currentSceneHandle;
    public void Start()
    {
        currentScene = gameObject.scene.name;
        currentSceneHandle = gameObject.scene.handle;
    }

}
