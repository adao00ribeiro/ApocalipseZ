using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserService 
{
  public static IEnumerator Login(string json, Action<UserLogin, string> callback)
    {
        /*
        CadastroRequest dadosParaEnviar = new CadastroRequest();
        dadosParaEnviar.email = "adao-eduardo@hotmail.com";
        dadosParaEnviar.senha = "Adao1456+";
        string json = JsonUtility.ToJson(dadosParaEnviar);
         */
        using (UnityWebRequest webRequest = UnityWebRequest.Post(API.url + "/api/usuario/login", json, "application/json"))
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
                    UserLogin result = JsonUtility.FromJson<UserLogin>(webRequest.downloadHandler.text);
                    callback(result, null); // Callback without error
                    yield break;
            }

            callback(default(UserLogin), errorMessage); // Callback with error
        }
    }
}
