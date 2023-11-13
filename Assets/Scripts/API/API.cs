using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class API : MonoBehaviour
{
    [SerializeField] private string URL = "http://localhost:";
    [SerializeField] private string port = "5272";

    void Start()
    {
        StartCoroutine(Login<string>());
    }


    public IEnumerator Login<T>(System.Action<T> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", "adao-eduardo@hotmail.com");
        form.AddField("senha", "Adao1456+");
            string url = URL+port + "/api/usuario/login";
            print(url);
        
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
                  webRequest.SetRequestHeader("Content-Type", "application/json");
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            //    webRequest.SetRequestHeader("Authorization", "Bearer " + bearerToken);
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
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
