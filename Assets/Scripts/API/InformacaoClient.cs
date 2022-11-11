using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformacaoClient : MonoBehaviour
{
   public  User userdata;   
    private void Start ( )
    {
        DontDestroyOnLoad ( this);
    }
}
