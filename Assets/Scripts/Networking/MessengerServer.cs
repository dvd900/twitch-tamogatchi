using System;
using UnityEngine;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

public delegate void OnNetMsg(NetMsg msg);

public class MessengerBehavior : WebSocketBehavior {

    public MessengerBehavior() { }

	protected override void OnMessage (MessageEventArgs e) {

        MessengerServer.MsgWrapper wrapper = JsonUtility.FromJson<MessengerServer.MsgWrapper>(e.Data);

        MessengerServer.singleton.HandleMessage(wrapper);
    }
}


public class MessengerServer : MonoBehaviour {

    public class MsgWrapper {
        public int messageInd;
        public string data;
    }

    public static MessengerServer singleton;

	public int port; 

	public WebSocketServer m_server;

    private Dictionary<int, OnNetMsg> msgHandlers;
    private Queue<MsgWrapper> msgQueue;

	void Awake() {
		singleton = this;

        msgHandlers = new Dictionary<int, OnNetMsg>();
        msgQueue = new Queue<MsgWrapper>();
	}

	void Start() {
		m_server = new WebSocketServer (port);
		m_server.AddWebSocketService<MessengerBehavior> ("/Chat");
		m_server.Start ();
	}

    public void HandleMessage(MsgWrapper msg) {
        msgQueue.Enqueue(msg);
    }

    public void SetHandler(int msgInd, OnNetMsg callback) {
        msgHandlers[msgInd] = callback;
    }

    public void ClearHandler(int msgInd) {
        msgHandlers[msgInd] = null;
    }

    private void Update() {
        if(msgQueue.Count != 0) {
            Debug.Log("Checking nonzero msg queue... Count: " + msgQueue.Count);
        }

        while (msgQueue.Count != 0) {
            MsgWrapper wrapper = msgQueue.Dequeue();

            if(msgHandlers[wrapper.messageInd] != null) {
                NetMsg msg = JsonUtility.FromJson<NetMsg>(wrapper.data);
                msgHandlers[wrapper.messageInd](msg);
            }
        }
    }

    void OnDestroy() {
		m_server.Stop ();
	}

	public void SendToMessenger(string msg) {
		m_server.WebSocketServices.Broadcast (msg);
	}
}

