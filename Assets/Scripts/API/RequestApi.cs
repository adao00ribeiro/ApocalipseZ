using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class RequestApi : MonoBehaviour
{
    [SerializeField] private  string url = "http://localhost:4000/api/";
    [SerializeField] private  string user = "user/";
    [SerializeField] private  string servidores = "servidores/";
    [SerializeField] private  string inventariosglobal = "servidores/";
    [SerializeField] private  string inventarioslocal = "servidores/";
    [SerializeField] private  string listacharacter = "servidores/";
    public IEnumerator Request <T>( string[] param , System.Action<T> callback = null )
    {
        string uri = url;
        foreach ( string item in param )
        {
            uri += item + "/";
        }
        Debug.Log (uri);
        using ( UnityWebRequest webRequest = UnityWebRequest.Get ( uri ) )
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest ( );

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch ( webRequest.result )
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError ( pages[page] + ": Error: " + webRequest.error );
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError ( pages[page] + ": HTTP Error: " + webRequest.error );
                    break;
                case UnityWebRequest.Result.Success:
                   // Debug.Log ( pages[page] + ":\nReceived: " + webRequest.downloadHandler.text );
                    // Show results as text
                    T tempuser = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                    // Or retrieve results as binary data
                    byte[] results = webRequest.downloadHandler.data;
                    callback(tempuser);
                    break;
            }
        }
    }
}