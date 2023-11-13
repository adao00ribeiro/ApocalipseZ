using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
public  class RequestApi :MonoBehaviour 
{
    public static Action<bool> IsLoading;
     public static Action<string> data;
    private  static string url = "http://localhost:5272";
    
    public static IEnumerator  Request <T>(Action<T> callback  )
    {
        CadastroRequest dadosParaEnviar = new CadastroRequest();
        dadosParaEnviar.email=  "adao-eduardo@hotmail.com";
        dadosParaEnviar.senha = "Adao1456+";
            string json = JsonUtility.ToJson(dadosParaEnviar);
          
        using ( UnityWebRequest webRequest = UnityWebRequest.Post ( url + "/api/usuario/login",json ,"application/json" ) )
        {
         IsLoading.Invoke(true);
            yield return webRequest.SendWebRequest ( );

            switch ( webRequest.result )
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError (  ": Error: " + webRequest.error );
                     data.Invoke(webRequest.error );
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError (  ": HTTP Error: " + webRequest.error );
                    data.Invoke(webRequest.error );
                    break;
                case UnityWebRequest.Result.Success:
                   // Debug.Log ( pages[page] + ":\nReceived: " + webRequest.downloadHandler.text );
                    // Show results as text
                 //   T tempuser = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                T teste = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                data?.Invoke(webRequest.downloadHandler.text);
                    // Or retrieve results as binary data
                 //   byte[] results = webRequest.downloadHandler.data;
                  callback(teste);
                    break;
            }
            IsLoading.Invoke(false);
        }
    }

 
}