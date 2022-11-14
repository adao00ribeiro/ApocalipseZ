using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoginManager : MonoBehaviour
{
    [SerializeField] private InputField EmailInputField;
    [SerializeField] private InputField SenhaInputField;
    [SerializeField] private Text debugText;

    private void Start()
    {
        EmailInputField.text = "adao-eduardo@hotmail.com";
        SenhaInputField.text = "123456";

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
            param.Add("user");
            param.Add(EmailInputField.text);
            param.Add(SenhaInputField.text);
            StartCoroutine(GameObject.FindObjectOfType<RequestApi>().Request<StructUser>(param.ToArray(), structuser =>
            {

                if (!string.IsNullOrEmpty(structuser.username))
                {
                    GameObject.FindObjectOfType<InformacaoClient>().userdata = new User(structuser);
                    // GameObject.FindObjectOfType<SceneController>().CarregarCenaAsync("Lobby");
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