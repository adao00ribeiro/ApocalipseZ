using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLogin 
{
     public bool sucesso ;
        
        public string accessToken;
      
        public string refreshToken ;

        public List<string> erros ;

    public override string ToString()
    {
        return accessToken;
    }
}