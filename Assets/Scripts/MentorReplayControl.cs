using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum ReplayAction
{
    Play = 0,
    SpeedUp = 1,
    SlowDown = 2,
    CameraPrev = 3,
    CameraNext = 4,
    Reset = 5,
    Quit = 6
}

public class ReplayActionMessage : MessageBase
{
    public int action;
}

public class MentorReplayControl : MonoBehaviour {
    public ClientConnector client;

    public void SendReplayAction(ReplayAction action)
    {
        ReplayActionMessage msg = new ReplayActionMessage();
        msg.action = (int)action;

        client.SendMessage((short)OutgoingRequests.ReplayAction, msg);
    }
}
