using UnityEngine;
using System.Collections;

public class AutoConnectToPhoton : MonoBehaviour
{
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings("v1.0");
	}
	

	void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
		PhotonNetwork.CreateRoom(null, true, true, 2);
	}

	void OnJoinedRoom()
	{
		Application.LoadLevel("Game");
	}

	void OnPhotonCreateRoomFalied()
	{
		Debug.Log("OnPhotonCreateRoomFalied");
	}
	
	void OnFailedToConnectToPhoton( DisconnectCause cause )
	{
		Debug.Log("Failed to connect: " + cause.ToString());
	}

	void OnGUI()
	{
		GUILayout.Label("Connecting to Photon Servers");
	}
}