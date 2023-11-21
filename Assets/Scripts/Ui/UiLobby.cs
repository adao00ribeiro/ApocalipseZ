using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ApocalipseZ;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
public class UiLobby : MonoBehaviour
{
    [SerializeField] public GameObject PrefabUiSlotCharacter;
    [SerializeField] public TextMeshProUGUI TextUserName;
    [SerializeField] public TextMeshProUGUI TextCoinAP;
    [SerializeField] public TextMeshProUGUI TextCoinDP;
    public Transform ContentCharacters;


    // Declarando um delegate
    public delegate void SpawnCharacterDelegate();
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

    public void PopuleContentCharacter(List<string> ListCharacter, UnityAction<string> callbackSpawCharacter)
    {
        foreach (var item in ListCharacter)
        {
            DataCharacter character = GameController.Instance.DataManager.GetDataCharacter(item);
            if (character)
            {
                GameObject temp = Instantiate(PrefabUiSlotCharacter, ContentCharacters);
                temp.GetComponent<Button>().onClick.AddListener(()=>{
                    callbackSpawCharacter(character.Name);
                });
                temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = character.Name;
                temp.transform.GetChild(1).GetComponent<Image>().sprite = character.Avatar;
            }
        }
    }
    // Start is called before the first frame update
}
