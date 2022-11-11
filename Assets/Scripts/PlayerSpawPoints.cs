using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class PlayerSpawPoints : MonoBehaviour
{
	[SerializeField]private List<Vector3> Points = new List<Vector3>();
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
        if ( _instance != null && _instance != this )
        {
            Destroy ( this.gameObject );
        }
        else
        {
            _instance = this;
        }
        foreach ( Transform item in transform )
        {
          Points.Add ( item.position);
        }

    }

    internal Vector3 GetPointSpaw ( )
    {
        Vector3 novo =  Points[Random.Range ( 0 , Points.Count - 1 )];
        print (novo);
        return novo;
    }
}
