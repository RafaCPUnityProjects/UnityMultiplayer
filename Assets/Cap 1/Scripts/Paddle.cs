using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour
{
	// how fast the paddle can move
	public float MoveSpeed = 10f;
	
	// how far up and down the paddle can move
	public float MoveRange = 10f;
	
	// whether this paddle can accept player input
	public bool AcceptsInput = true;

	//posicao como vem da rede
	private Vector3 readNetworkPos;

	void Start()
	{
		//limitando o input apenas para o nosso paddle
		AcceptsInput = networkView.isMine;
	}

	void Update()
	{
		// does not accept input, abort
		if( !AcceptsInput )
		{
			//nao e nosso entao faz um lerp com a ultima posicao recebida da rede
			transform.position = Vector3.Lerp(transform.position, readNetworkPos, 10f * Time.deltaTime);

			//saindo sem pegar input do player
			return;
		}
		
		//get user input
		float input = Input.GetAxis( "Vertical" );
		
		// move paddle
		Vector3 pos = transform.position;
		pos.z += input * MoveSpeed * Time.deltaTime;
		
		// clamp paddle position
		pos.z = Mathf.Clamp( pos.z, -MoveRange, MoveRange );
		
		// set position
		transform.position = pos;
	}

	void OnSerializeNetworkView( BitStream stream )
	{
		//se estou escrevendo a informacao, subo a posicao do pad
		if(stream.isWriting)
		{
			Vector3 pos = transform.position;
			stream.Serialize( ref pos );
		}
		else //se estou lendo capturo a info
		{
			Vector3 pos = Vector3.zero;
			stream.Serialize (ref pos);
			readNetworkPos = pos;
		}
	}
}