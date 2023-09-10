using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class PlayerSpawPointsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> SpawPoints = new List<GameObject>();

    // Start is called before the first frame update

    internal Vector3 GetPointSpaw()
    {
        GameObject point = SpawPoints[Random.Range(0, SpawPoints.Count )];
        return point.transform.position;
    }

    internal void Add(GameObject gameObject)
    {
        SpawPoints.Add(gameObject);
    }
    internal void Remove(GameObject gameObject)
    {
        SpawPoints.Remove(gameObject);
    }
}
