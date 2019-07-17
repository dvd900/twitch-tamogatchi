using System;
using UnityEngine;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

public delegate void OnNetMsg(NetMsg msg);

public class MessengerBehavior : WebSocketBehavior {

    public class MsgWrapper {
        public int msgInd;
        public string data;
    }

    public MessengerBehavior() { }

	protected override void OnMessage (MessageEventArgs e) {
        Debug.Log("recieved message wrapper " + e.Data);

        MsgWrapper wrapper = JsonUtility.FromJson<MsgWrapper>(e.Data);

        NetMsg msg = null;
        switch(wrapper.msgInd) {
            case NetMsgInds.ClickMessage:
                msg = JsonUtility.FromJson<ClickMessage>(wrapper.data);
                Debug.Log("got click message");
                break;
        }

        if(msg != null) {
            MessengerServer.singleton.HandleMessage(msg, wrapper.msgInd);
        }
    }
}


public class MessengerServer : MonoBehaviour {

	public static MessengerServer singleton;

	public int port; 

	public WebSocketServer m_server;

    private Dictionary<int, OnNetMsg> msgHandlers;

	void Awake() {
		singleton = this;

        msgHandlers = new Dictionary<int, OnNetMsg>();
	}

	void Start() {
		m_server = new WebSocketServer (port);
		m_server.AddWebSocketService<MessengerBehavior> ("/Chat");
		m_server.Start ();
	}

    public void HandleMessage(NetMsg msg, int msgInd) {
        if(msgHandlers[msgInd] != null) {
            msgHandlers[msgInd](msg);
        }
    }

    public void SetHandler(int msgInd, OnNetMsg callback) {
        msgHandlers[msgInd] = callback;
    }

    public void ClearHandler(int msgInd) {
        msgHandlers[msgInd] = null;
    }

    void OnDestroy() {
		m_server.Stop ();
	}

	public void SendToMessenger(string msg) {
		m_server.WebSocketServices.Broadcast (msg);
	}
}

