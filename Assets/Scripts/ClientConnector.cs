using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

enum IngoingRequests
{
    IsGameState = 100,
    IsMenuState = 101,
    IsReplayState = 102,
}

enum OutgoingRequests
{
    IncreaseScore = 151,
    ReplayAction = 152,
}

public class ClientConnector : MonoBehaviour {
    string ipV4Regex = "^(?:(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])(.(?!$)|$)){4}$";
    public NetworkClient myClient;
    public string IP = "";

    public int portNumber = 50000;

    public GameObject connectMenu, gameMenu, replayMenu, loadingMenu;
    public TMP_Text connectMenuText, loadingMenuText;

    public void Start()
    {
        myClient = new NetworkClient();

        myClient.RegisterHandler((short)IngoingRequests.IsGameState, OnOpenGameState);
        myClient.RegisterHandler((short)IngoingRequests.IsMenuState, OnOpenMenuState);
        myClient.RegisterHandler((short)IngoingRequests.IsReplayState, OnOpenReplayState);

        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        myClient.RegisterHandler(MsgType.Error, OnError);
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        loadingMenuText.text = "Waiting for Game/Replay to start";
    }

    public void OnDisconnected(NetworkMessage netMsg)
    {
        replayMenu.SetActive(false);
        gameMenu.SetActive(false);
        loadingMenu.SetActive(false);
        connectMenu.SetActive(true);
        connectMenuText.text = "Connection Error: " + IP;
    }

    public void OnError(NetworkMessage netMsg)
    {
        Debug.Log("Error connecting with code " + netMsg);
    }

    private void OnChangeReplayPlayingState(NetworkMessage netMsg)
    {
        throw new NotImplementedException();
    }

    private void OnOpenReplayState(NetworkMessage netMsg)
    {
        loadingMenu.SetActive(false);
        replayMenu.SetActive(true);
    }

    private void OnOpenMenuState(NetworkMessage netMsg)
    {
        replayMenu.SetActive(false);
        gameMenu.SetActive(false);
        loadingMenu.SetActive(true);
    }

    private void OnOpenGameState(NetworkMessage netMsg)
    {
        loadingMenu.SetActive(false);
        gameMenu.SetActive(true);
    }

    public void SetIP(string ip)
    {
        IP = ip;
    }

    public void ConnectToServer()
    {
        Regex r = new Regex(ipV4Regex);
        if (!(IP == null || IP == String.Empty) && r.IsMatch(IP))
        {
            connectMenu.SetActive(false);
            loadingMenu.SetActive(true);
            myClient.Connect(IP, portNumber);
            //show waiting menu
            //StartCoroutine(CheckConnection());
            loadingMenuText.text = "Attempting To Connect";
        }
        else {
            connectMenuText.text = "Invalid IP: " + IP;
        }
    }

    IEnumerator CheckConnection()
    {
        yield return new WaitForSeconds(3);

        if (myClient.isConnected)
        {
            loadingMenuText.text = "Waiting for Game/Replay to start";
        }
        else
        {
            loadingMenu.SetActive(false);
            connectMenu.SetActive(true);
            connectMenuText.text = "Error connection unsuccesful: " + IP;
        }
    }

    public void SendMessage(short increaseScore, MessageBase msg)
    {
        myClient.Send(increaseScore, msg);
    }
}
