using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float TimeLife;
    // Start is called before the first frame update
    void Start()
    {
        Invoke ("DestroyObject" , TimeLife );
    }

    public void DestroyObject ( )
    {
        Destroy (gameObject );
        
    }
}
