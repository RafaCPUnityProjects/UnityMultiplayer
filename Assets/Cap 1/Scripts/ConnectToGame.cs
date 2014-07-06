using UnityEngine;
using System.Collections;

public class ConnectToGame : MonoBehaviour
{
	private string ip = "localhost";
	private int port = 25005;
	
	void OnGUI()
	{
		// let the user enter IP address
		GUILayout.Label( "IP Address" );
		ip = GUILayout.TextField( ip, GUILayout.Width( 200f ) );
		
		// let the user enter port number
		// port is an integer, so only numbers are allowed
		GUILayout.Label( "Port" );
		string port_str = GUILayout.TextField( port.ToString(), GUILayout.Width( 100f ) );
		int port_num = port;
		if( int.TryParse( port_str, out port_num ) )
			port = port_num;
		
		// connect to the IP and port
		if( GUILayout.Button( "Connect", GUILayout.Width( 100f ) ) )
		{
			Network.Connect( ip, port );
		}
		
		// host a server on the given port, only allow 1 incoming connection (one other player)
		if( GUILayout.Button( "Host", GUILayout.Width( 100f ) ) )
		{
			Network.InitializeServer( 1, port, true );
		}
	}
	
	void OnConnectedToServer()
	{
		Debug.Log( "Connected to server" );
		// this is the NetworkLevelLoader we wrote earlier in the chapter – pauses the network, loads the level, waits for the level to finish, and then unpauses the network
		NetworkLevelLoader.Instance.LoadLevel( "Game" );
	}
	
	void OnServerInitialized()
	{
		Debug.Log( "Server initialized" );
		NetworkLevelLoader.Instance.LoadLevel( "Game" );
	}
}