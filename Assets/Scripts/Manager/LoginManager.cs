using System;
using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoginManager : MonoBehaviour
{
    [SerializeField] private InputField EmailInputField;
    [SerializeField] private InputField SenhaInputField;
    [SerializeField] private Text debugText;

    public GameObject panelLoading;
    private void Start()
    {
        EmailInputField.text = "adao-eduardo@hotmail.com";
        SenhaInputField.text = "123456";

    }

    private void ActivePanelLoading(bool obj)
    {

        panelLoading.SetActive(obj);
    }

    private bool ValidarInputField()
    {
        if (EmailInputField.text.Equals(""))
        {
            EmailInputField.Select();
            Debug.Log("Campo Email Vazio");
            return false;
        }
        if (EmailInputField.text.IndexOf('@') <= 0)
        {
            EmailInputField.Select();
            Debug.Log("email errado");
            return false;
        }
        if (SenhaInputField.text.Equals(""))
        {
            SenhaInputField.Select();
            Debug.Log("Campo Senha Vazio");
            return false;
        }
        return true;
    }

    public void Iniciar()
    {
        List<string> param = new List<string>();
        if (ValidarInputField())
        {

        CadastroRequest dadosParaEnviar = new CadastroRequest();
        dadosParaEnviar.email = "adao-eduardo@hotmail.com";
        dadosParaEnviar.senha = "Adao1456+";
        string json = JsonUtility.ToJson(dadosParaEnviar);
            ActivePanelLoading(true);
            StartCoroutine(UserService.Login(json,  (result, error) =>
            {
                if (error != null)
                {
                    Debug.LogError("Erro na requisição: " + error);
                }
                if(result !=null)
                {
                    PlayerPrefs.SetString("Token",result.accessToken);
                  
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
                }
                  ActivePanelLoading(false);
            }));

        }
    }

    public void GetUser()
    {
        //GameObject.FindObjectOfType<NetworkManager>().StartServer();
    }

    public void Sair()
    {
        Application.Quit();
    }
}