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
	}

	public void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Application.LoadLevel (0);
	}

}
