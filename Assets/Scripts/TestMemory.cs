using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestMemory : MonoBehaviour
{
    public GameObject prefab;
    public int max = 100;


    public GameObject pai;

    public bool IsCreate;

    public bool IsDestroy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsCreate)
        {
            StartCoroutine(Create());
            IsCreate = false;
        }

        if (IsDestroy)
        {
            Destroy(pai);
            IsDestroy = false;
        }
    }

    IEnumerator Create()
    {
        pai = GameObject.CreatePrimitive(PrimitiveType.Cube);

        for (int i = 0; i < max; i++)
        {
            Instantiate(prefab, pai.transform);
        }
        yield return null;
    }

}
