using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
public class RequestApi : MonoBehaviour
{
    private static string url = "http://localhost:5272";

    public static IEnumerator Get<T>(Action<T, string> callback)
    {
        CadastroRequest dadosParaEnviar = new CadastroRequest();
        dadosParaEnviar.email = "adao-eduardo@hotmail.com";
        dadosParaEnviar.senha = "Adao1456+";
        string json = JsonUtility.ToJson(dadosParaEnviar);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url + "/api/usuario/login", json, "application/json"))
        {
            yield return webRequest.SendWebRequest();

            string errorMessage = null;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    errorMessage = "Error: " + webRequest.error;
                    Debug.LogError(errorMessage);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    errorMessage = "HTTP Error: " + webRequest.error;
                    Debug.LogError(errorMessage);
                    break;
                case UnityWebRequest.Result.Success:
                    T result = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                    callback(result, null); // Callback without error
                    yield break;
            }

            callback(default(T), errorMessage); // Callback with error
        }
    }

    public static IEnumerator Post<T>(string json, Action<T, string> callback)
    {
        /*
        CadastroRequest dadosParaEnviar = new CadastroRequest();
        dadosParaEnviar.email = "adao-eduardo@hotmail.com";
        dadosParaEnviar.senha = "Adao1456+";
        string json = JsonUtility.ToJson(dadosParaEnviar);
         */
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url + "/api/usuario/login", json, "application/json"))
        {
            yield return webRequest.SendWebRequest();

            string errorMessage = null;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    errorMessage = "Error: " + webRequest.error;
                    Debug.LogError(errorMessage);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    errorMessage = "HTTP Error: " + webRequest.error;
                    Debug.LogError(errorMessage);
                    break;
                case UnityWebRequest.Result.Success:
                    T result = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                    callback(result, null); // Callback without error
                    yield break;
            }

            callback(default(T), errorMessage); // Callback with error
        }
    }
}
