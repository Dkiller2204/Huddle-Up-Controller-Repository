using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReplayActionMessage : MessageBase
{
    public int action;
}

public class MentorReplayControl : MonoBehaviour {
    public ClientConnector client;

    public void SendReplayAction(int action)
    {
        ReplayActionMessage msg = new ReplayActionMessage();
        msg.action = action;

        client.SendMessage((short)OutgoingRequests.ReplayAction, msg);
    }
}
