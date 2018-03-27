using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public enum Stats {
    NotSet = 0,
    Communication = 1,
    Leadership = 2,
    Creativity = 3,
    Initiative = 4
}

public class MentorScoreMessage : MessageBase
{
    public int playerMSG;
    public int scoreTypeMSG;
}

public class MentorScore : MonoBehaviour
{
    public ClientConnector client;
    public int player = -1;
    public Stats scoreType = Stats.NotSet;
    public TMP_Text text;

    public void SendScore()
    {
        MentorScoreMessage msg = new MentorScoreMessage();
        msg.playerMSG = player;
        msg.scoreTypeMSG = (int)scoreType;

        client.SendMessage((short)OutgoingRequests.IncreaseScore, msg);
    }

    public void SetButtonState(Button b)
    {
        GameObject[] playerButtons = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject o in playerButtons)
        {
            o.GetComponent<Button>().interactable = true;
        }
        
        b.interactable = false;
    }
    public void SetPlayer(int player)
    {
        this.player = player;
    }

    public void SetScoreType(int scoreType)
    {
        this.scoreType = (Stats)scoreType;
        SendScore();
    }

    private void Update()
    {
        text.text = this.ToString();
    }

    public override string ToString()
    {
        if (player != -1 && scoreType.ToString() != "NotSet")
        {
            return "Player " + player + "'s " + scoreType.ToString() + " increased";
        }
        return "";
    }
}