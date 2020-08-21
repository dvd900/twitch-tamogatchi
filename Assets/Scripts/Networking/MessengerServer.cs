using System;
using UnityEngine;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

public delegate void OnNetMsg(string msg);

public class MessengerBehavior : WebSocketBehavior {

    public MessengerBehavior() { }

	protected override void OnMessage (MessageEventArgs e) {
        MessengerServer.singleton.HandleMessage(e.Data);
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
    private Queue<string> msgQueue;

	void Awake() {
		singleton = this;

        msgHandlers = new Dictionary<int, OnNetMsg>();
        msgQueue = new Queue<string>();
	}

	void Start() {
		m_server = new WebSocketServer (port);
		m_server.AddWebSocketService<MessengerBehavior> ("/Chat");
		m_server.Start ();
	}

    public void HandleMessage(string msg) {
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
            string message = msgQueue.Dequeue();
            int messageInd = int.Parse(message.Split('_')[0]);
            message = message.Remove(0,message.IndexOf('_')+1);
            msgHandlers[messageInd](message);
        }
    }

    void OnDestroy() {
        if(m_server != null)
        {
		    m_server.Stop ();
        }
	}

	public void SendToMessenger(string msg) {
		m_server.WebSocketServices.Broadcast (msg);
	}
}

