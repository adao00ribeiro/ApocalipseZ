using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ApocalipseZ;
public class UiLobby : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI TextUserName;
    [SerializeField] public TextMeshProUGUI TextCoinAP;
    [SerializeField] public TextMeshProUGUI TextCoinDP;

    public void SetTextUserName(string text)
    {
        TextUserName.text = text;
    }
    public void SetTextCoinAP(string text)
    {
        TextCoinAP.text = text;
    }
    public void SetTextCoinDP(string text)
    {
        TextCoinDP.text = text;
    }
    // Start is called before the first frame update
}
