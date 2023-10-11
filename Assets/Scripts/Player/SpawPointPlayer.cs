using ApocalipseZ;
using FishNet;
using UnityEngine;

public class SpawPointPlayer : MonoBehaviour
{

    public string currentScene;

    public void Start()
    {
        currentScene = gameObject.scene.name;
    }

}
