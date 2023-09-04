using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class PlayerSpawPoints : MonoBehaviour
{
    [SerializeField] private GameObject[] SpawPoints;
    private static PlayerSpawPoints _instance;
    public static PlayerSpawPoints Instance
    {
        get
        {
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        SpawPoints = GameObject.FindGameObjectsWithTag("SpawPlayer");


    }

    internal Vector3 GetPointSpaw()
    {
        GameObject point = SpawPoints[Random.Range(0, SpawPoints.Length - 1)];
        return point.transform.position;
    }
}
