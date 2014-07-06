using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {
	public GameObject player;

	IEnumerator Start()
	{
		//waiting two frames
		yield return null;
		yield return null;

		Network.isMessageQueueRunning = true;
		Network.Instantiate(player, Vector3.zero, Quaternion.identity, 0);
	}
}
