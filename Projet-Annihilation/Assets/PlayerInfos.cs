using UnityEngine;
using System.Collections;

public class PlayerInfos : MonoBehaviour {

	public string username = "Unknown", color = "black", faction = "robot";
	public int teamNumber = 1, playerNumber = -1;
	public bool host = false;

	void Awake()
	{
		DontDestroyOnLoad (transform);
	}

	[RPC] public void SetPlayerNumber(int number)
	{
		playerNumber = number;
	}

	[RPC] public void SetHostTrue()
	{
		host = true;
	}

	[RPC] public void SetUsername(string username)
	{
		this.username = username;
	}

}
