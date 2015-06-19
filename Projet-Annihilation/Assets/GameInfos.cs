using UnityEngine;
using System.Collections;
using RTS;
using System.Collections.Generic;

public class GameInfos : MonoBehaviour {

	public Player actualPlayer;
	private static bool created = false;
	public List<Player> players = new List<Player>();
	
	void Awake ()
	{
		if (!created) 
		{
			DontDestroyOnLoad (transform.gameObject);
			RessourceManager.SetGameInfos (this);
			created = true;
		} 
		else 
		{
			Destroy(this.gameObject);
		}
	}

	public void Update()
	{
		if (Input.GetKey (KeyCode.M) && Input.GetMouseButton (0)) {
			actualPlayer.AddResource(ResourceType.Crystalite, 1000);
			actualPlayer.AddResource(ResourceType.Power, 1000);
		}
		if (actualPlayer.GetComponentInChildren<Buildings> ().transform.GetComponentsInChildren<Building> ().Length == 0 &&
			actualPlayer.GetComponentInChildren<Units> ().transform.GetComponentsInChildren<Unit> ().Length == 0) {

			actualPlayer.GetComponent<NetworkView>().RPC("SetDead", RPCMode.AllBuffered);
		}
		if (players.Count >= 2 && Victory() && !actualPlayer.isDead) 
		{
			actualPlayer.won = true;
		}

	}

	public void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Application.LoadLevel (0);
	}

	public bool Victory() //verification que le joueur actuel est bien le seul joueur en vie
	{
		bool yes = true;
		foreach (Player player in players) {
			yes = yes && (player.isDead || player.teamNumber == actualPlayer.teamNumber);
		}
		return yes;
	}

}
