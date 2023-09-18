using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReflectScopeShader : MonoBehaviour
{
    public Camera can;

    [SerializeField] private Material material;
    // Start is called before the first frame update
    void Update()
    {
        if (can == null)
        {
            return;
        }
        Vector2 screenPixels = can.WorldToScreenPoint(transform.position);
        screenPixels = new Vector2(screenPixels.x / Screen.width, screenPixels.y / Screen.height);

        material.SetVector("_ObjectScreenPosition", screenPixels);
    }
}