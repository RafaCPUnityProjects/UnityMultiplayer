using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private string connectIP = "127.0.0.1"; //localhost

	void OnGUI()
	{
		if (GUILayout.Button("Host"))
		{
			//host a game
			Network.InitializeServer(8, 25005, true);

			//load level
			Application.LoadLevel("GameScene");
		}

		connectIP = GUILayout.TextField(connectIP);
		if (GUILayout.Button("Connect"))
		{
			//connect
			Network.Connect(connectIP, 25005);
		}
	}

	void OnConnectedToServer()
	{
		Network.isMessageQueueRunning = false;

		//load level
		Application.LoadLevel("GameScene");
	}
}
