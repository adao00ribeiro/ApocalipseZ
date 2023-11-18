using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

            param.Add(EmailInputField.text);
            param.Add(SenhaInputField.text);
            ActivePanelLoading(true);
            StartCoroutine(RequestApi.Post<string>("asdasdasd", (result, error) =>
            {
                if (error != null)
                {
                    // Lidar com o erro
                    Debug.LogError("Erro na requisição: " + error);
                }
                else
                {
                    // Lidar com o resultado
                    Debug.Log("Requisição bem-sucedida: " + result);
                }

            }));

        }
    }

    public void CenaServidor()
    {
        //GameObject.FindObjectOfType<NetworkManager>().StartServer();
    }

    public void Sair()
    {
        Application.Quit();
    }
}