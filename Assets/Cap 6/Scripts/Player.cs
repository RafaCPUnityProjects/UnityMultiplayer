using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float moveSpeed = 5f;
	
	void Update()
	{
		if (networkView == null || networkView.isMine)
		{
			transform.Translate(
				new Vector3(
					Input.GetAxis("Horizontal"),
					0,
					Input.GetAxis("Vertical")
					) * moveSpeed * Time.deltaTime
				);
		}
	}
}
