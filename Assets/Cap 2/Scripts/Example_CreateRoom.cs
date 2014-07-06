using UnityEngine;
using System.Collections;

public class Example_CreateRoom : MonoBehaviour
{
	void OnGUI()
	{
		// if we're not inside a room, let the player create a room
		if( PhotonNetwork.room == null )
		{
			if( GUILayout.Button( "Create Room", GUILayout.Width( 200f ) ) )
			{
				// create a room called "RoomNameHere", visible to the lobby, allows other players to join, and allows up to 8 players
				PhotonNetwork.CreateRoom( "RoomNameHere", true, true, 8 );
			}
		}
		else
		{
			// display some stats about this room
			GUILayout.Label( "Connected to room: " + PhotonNetwork.room.name );
			GUILayout.Label( "Players in room: " + PhotonNetwork.playerList.Length );
			
			// disconnect from the current room
			if( GUILayout.Button( "Disconnect", GUILayout.Width( 200f ) ) )
			{
				PhotonNetwork.LeaveRoom();
			}
		}
	}
}