using UnityEngine;
using System.Collections;

public class PhotonScorekeeper : MonoBehaviour
{
	// the maximum score a player can reach
	public int ScoreLimit = 10;
	
	//pontos de spawn dos players 1 e 2
	public Transform spawnP1;
	public Transform spawnP2;
	
	public GameObject paddlePrefab;
	
	public TextMesh p1ScoreDisplay;
	public TextMesh p2ScoreDisplay;
	
	// Player 1’s score
	private int p1score = 0;
	
	// Player 2’s score
	private int p2score = 0;
	
	void Start()
	{
		if(Network.isServer)
		{
			//o servidor nao chama "OnPlayerConected" - spawn manual
			Network.Instantiate (paddlePrefab, spawnP1.position, Quaternion.identity, 0);
			
			//ninguem conectou ainda entao...
			p2ScoreDisplay.text = "Waiting...";
		}
	}
	
	//chamado no servidor qnd um jogador conecta
	void OnPlayerConnected(NetworkPlayer player)
	{
		//envia um rpc pra esse jogador mandando ele fazer o network instantiate
		networkView.RPC ("net_DoSpawn", player, spawnP2.position);
		
		//alguem conectou! score = 0
		p2ScoreDisplay.text = "0";
	}
	
	//aparece no servidor qnd o cliente desconecta
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		//player 2 left - reset scores
		p1score = 0;
		p2score = 0;
		
		//atualiza display de scores:
		p1ScoreDisplay.text = p1score.ToString();
		p2ScoreDisplay.text = "Waiting...";
		
		Application.LoadLevel(Application.loadedLevel);
	}
	
	//aparece no cliente se desconectado do servidor
	void OnDisconnectedFromServer(NetworkDisconnection cause)
	{
		//voltar ao menu da aplicacao
		Application.LoadLevel("Menu");
	}
	
	[RPC]
	void net_DoSpawn (Vector3 position)
	{
		//spawn the player paddle
		Network.Instantiate(paddlePrefab, position, Quaternion.identity, 0);
	}
	
	//agora funciona com RPC call
	public void AddScore( int player )
	{
		networkView.RPC("net_AddScore", RPCMode.Server, player);
	}
	
	// give the appropriate player a point
	[RPC]
	public void net_AddScore(int player)
	{
		// player 1
		if( player == 1 )
		{
			p1score++;
		}
		// player 2
		else if( player == 2 )
		{
			p2score++;
		}
		
		// check if either player reached the score limit
		if( p1score >= ScoreLimit || p2score >= ScoreLimit )
		{
			// player 1 has a better score than player 2
			if( p1score > p2score )
				Debug.Log( "Player 1 wins" );
			// player 2 has a better score than player 1
			if( p2score > p1score )
				Debug.Log( "Player 2 wins" );
			// both players have the same score - tie
			else
				Debug.Log( "Players are tied" );
			
			// reset scores and start over
			p1score = 0;
			p2score = 0;
		}
		
		p1ScoreDisplay.text = p1score.ToString();
		p2ScoreDisplay.text = p2score.ToString();
	}
}