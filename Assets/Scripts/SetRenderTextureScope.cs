using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using UnityEngine;

public class SetRenderTextureScope : MonoBehaviour
{
    public Camera cam;
    public void UpdateScope()
    {
        cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        FirstPersonCamera camera = GameController.Instance.playerController.GetPlayer().GetFirstPersonCamera();
        GetComponent<MeshRenderer>().material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        GetComponent<MeshRenderer>().material.mainTexture = cam.targetTexture;
    }
}
