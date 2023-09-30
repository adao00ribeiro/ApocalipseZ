using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : NetworkBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Text chatHistory;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] InputField chatMessage;
    [SerializeField] Button sendButton;

    // This is only set on client to the name of the local player
    internal static string localPlayerName;

    // Server-only cross-reference of connections to player names
    internal static readonly Dictionary<NetworkConnection, string> connNames = new Dictionary<NetworkConnection, string>();

    public override void OnStartServer()
    {
        connNames.Clear();
    }

    public override void OnStartClient()
    {
        chatHistory.text = "";
    }

    //  [ServerRpc(requiresAuthority = false)]
    [ServerRpc(RequireOwnership = false)]
    void CmdSend(string message, NetworkConnection sender = null)
    {
        print(sender.FirstObject);
        if (!connNames.ContainsKey(sender))
            connNames.Add(sender, sender.FirstObject.GetComponent<PlayerController>().PlayerName);

        if (!string.IsNullOrWhiteSpace(message))
            RpcReceive(connNames[sender], message.Trim());
    }

    [ObserversRpc]
    void RpcReceive(string playerName, string message)
    {
        string prettyMessage = playerName == localPlayerName ?
            $"<color=red>{playerName}:</color> {message}" :
            $"<color=blue>{playerName}:</color> {message}";
        AppendMessage(prettyMessage);
    }

    void AppendMessage(string message)
    {
        StartCoroutine(AppendAndScroll(message));
    }

    IEnumerator AppendAndScroll(string message)
    {
        chatHistory.text += message + "\n";

        // it takes 2 frames for the UI to update ?!?!
        yield return null;
        yield return null;

        // slam the scrollbar down
        scrollbar.value = 0;
    }

    // Called by UI element ExitButton.OnClick
    public void ExitButtonOnClick()
    {
        // StopHost calls both StopClient and StopServer
        // StopServer does nothing on remote clients
        base.OnStopNetwork();
        //   NetworkManager.singleton.StopHost();
    }

    // Called by UI element MessageField.OnValueChanged
    public void ToggleButton(string input)
    {
        sendButton.interactable = !string.IsNullOrWhiteSpace(input);
    }

    // Called by UI element MessageField.OnEndEdit
    public void OnEndEdit(string input)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Submit"))
            SendMessage();
    }

    // Called by OnEndEdit above and UI element SendButton.OnClick
    public void SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(chatMessage.text))
        {
            CmdSend(chatMessage.text.Trim());
            chatMessage.text = string.Empty;
            chatMessage.ActivateInputField();
        }
    }
}

