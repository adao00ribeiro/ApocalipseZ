using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Barrier : MonoBehaviour
{
    [SerializeField] private Material PrefabMaterial;
    [SerializeField] private MeshRenderer childMesh;

    [SerializeField] private PVPFLAGManager manager;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<PVPFLAGManager>();
        childMesh = GetComponentInChildren<MeshRenderer>();
        childMesh.material = new Material(Shader.Find("Shader Graphs/ShaderBarrier"));
        manager.OnStart += PVPFLAGManager_OnStart; ;
    }

    private void PVPFLAGManager_OnStart(bool obj)
    {
        StartCoroutine(DisableBarrier());
    }

    void OnDisable()
    {
        manager.OnStart -= PVPFLAGManager_OnStart; ;
    }
    IEnumerator DisableBarrier()
    {
        GetComponent<BoxCollider>().enabled = false;
        while (time > 0)
        {
            SetTime(time);
            yield return new WaitForSeconds(0.1f);
            time -= 0.01f;
        }

        StopCoroutine(DisableBarrier());
    }
    private void SetTime(float time)
    {
        childMesh.material.SetFloat("_time", time);

    }
}
