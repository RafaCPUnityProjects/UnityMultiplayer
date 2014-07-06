using UnityEngine;
using System.Collections;

public class Networkedplayer : MonoBehaviour {
	//how far back to rewind interpolation?
	public float interpolationBackTime = 0.1f;

	//a snapshot of values received over the network
	private struct networkState
	{
		public Vector3 position;
		public double timeStamp;

		public networkState(Vector3 pos, double time)
		{
			this.position = pos;
			this.timeStamp = time;
		}
	}

	//we'll keep a buffer of 20 states
	networkState[] stateBuffer = new networkState[20];
	int stateCount = 0; //how many states have been recorded

	void Update()
	{
		if (networkView.isMine) // don't run interpolation on local object
			return;

		if (stateCount == 0) // no states to interpolate
			return;

		double currentTime = Network.time;
		double interpolationTime = currentTime - interpolationBackTime;

		//if the latest packet is newer than interpolation time:
		//we have enough packets to interpolate
		if (stateBuffer[0].timeStamp > interpolationTime)
		{
			for (int i = 0; i < stateCount; i++)
			{
				//find the closest state that matches network time
				//or use oldest state
				if(stateBuffer[i].timeStamp <= interpolationTime ||
					i == stateCount - 1)
				{
					//the state closest to network time
					networkState lhs = stateBuffer[i];

					//the state one slot newer
					networkState rhs = stateBuffer[Mathf.Max(i - 1, 0)];

					//use time between lhs and rhs to interpolate
					double length = rhs.timeStamp - lhs.timeStamp;
					float t = 0f;
					if (length > 0.0001)
					{
						t = (float)((interpolationTime - lhs.timeStamp) / length);
					}

					transform.position = Vector3.Lerp(
						lhs.position,
						rhs.position,
						t);
					break;
				}
			}
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 position = Vector3.zero;
		if (stream.isWriting)
		{
			position = transform.position;
			stream.Serialize(ref position);
		}
		else
		{
			stream.Serialize(ref position);
			BufferState(new networkState(position,info.timestamp));
		}
	}

	//save new state to buffer
	void BufferState(networkState state)
	{
		//shift buffer contents to accomodate new state
		for (int i = stateBuffer.Length - 1; i > 0; i--)
		{
			stateBuffer[i] = stateBuffer[i - 1];
		}

		//save state to slot 0
		stateBuffer[0] = state;

		//increment state count (up to buffer size)
		stateCount = Mathf.Min(stateCount + 1, stateBuffer.Length);
	}
}
