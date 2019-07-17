using System;
using UnityEngine;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

public class MessengerBehavior : WebSocketBehavior {
	
	public MessengerBehavior() { }

	protected override void OnMessage (MessageEventArgs e) {
        Debug.Log("received : " + e.Data);
	}
}
public class ServiceCheckBehavior : WebSocketBehavior {

}


public class MessengerServer : MonoBehaviour {

	public static MessengerServer singleton;

	public int port; 

	public WebSocketServer m_server;

	void Awake() {
		singleton = this;
	}

	void Start() {
		m_server = new WebSocketServer (port);
		m_server.AddWebSocketService<ServiceCheckBehavior> ("/Service");
		m_server.AddWebSocketService<MessengerBehavior> ("/Chat");
		m_server.Start ();
		Console.ReadKey (true);

	}

	void OnDestroy() {
		m_server.Stop ();
	}

	public void SendToMessenger(string msg) {
		m_server.WebSocketServices.Broadcast (msg);
	}
}

