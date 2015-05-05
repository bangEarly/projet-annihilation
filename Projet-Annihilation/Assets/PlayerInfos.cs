using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfos : MonoBehaviour {

	public string username = "Unknown", color = "black", faction = "robot";
	public int teamNumber = 1, playerNumber = -1;
	public bool host = false, ready = false;

	public GameObject playerPrefab;

	NetworkView playerInfosView;

	private List<int> playerNumberList = new List<int>() {0, 1, 2, 3, 4, 5, 6, 7};
	
	void Awake()
	{
		DontDestroyOnLoad (transform);
	}

	void Update()
	{
		playerInfosView = transform.GetComponent<NetworkView> ();
		if (Application.loadedLevel == 1 && playerInfosView.isMine) 
		{
			StartGame startGame = Object.FindObjectOfType<StartGame>();
			GameObject playerObject = (GameObject)Network.Instantiate(playerPrefab, startGame.listSpawnPosition[playerNumber], new Quaternion(0,0,0,0), 0);
			Player player = playerObject.GetComponent<Player>();
			NetworkView playerView = playerObject.GetComponent<NetworkView>();
			player.username = username;
			playerView.RPC("SetUsername", RPCMode.AllBuffered, username); 
			player.human = true;
			playerView.RPC ("SetHuman", RPCMode.AllBuffered);
			player.teamNumber = teamNumber;
			playerView.RPC("SetTeamNumber", RPCMode.AllBuffered, teamNumber);
			Camera.main.transform.position = new Vector3(startGame.listSpawnPosition[playerNumber].x, 40f, startGame.listSpawnPosition[playerNumber].z);
			Network.Destroy(gameObject);
		}
	}

	public int GetPlayerNumber()
	{
		int number = playerNumberList [0];
		playerInfosView.RPC ("RemoveNumber", RPCMode.AllBuffered, 0);
		return number;
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

	[RPC] public void LetTheGameBegin()
	{
		Application.LoadLevel (1);
	}

	[RPC] public void SetReady()
	{
		ready = !ready;
	}

	[RPC] public void RemoveNumber(int number)
	{
		playerNumberList.RemoveAt (0);
	}

}
