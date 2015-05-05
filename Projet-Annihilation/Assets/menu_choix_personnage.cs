using UnityEngine;
using System.Collections;

public class menu_choix_personnage : MonoBehaviour 
{

	public GUISkin backgroundMenu, backgroundPlayerSlot, backgroundMySlot, backgroundPlayerSlotEmpty;
	private PlayerInfos[] listPlayerInfos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{ 

		listPlayerInfos = Object.FindObjectsOfType<PlayerInfos> ();
		bool everybodyReady = true;

		for (int i = 0; i < listPlayerInfos.Length; i++) 
		{
			if ( listPlayerInfos[i].host && listPlayerInfos[i].transform.GetComponent<NetworkView>().isMine)
			{
				for (int j = 0; j < listPlayerInfos.Length; j++) 
				{
					if (listPlayerInfos[j].playerNumber == -1)
					{
						listPlayerInfos[j].transform.GetComponent<NetworkView>().RPC ("SetPlayerNumber", RPCMode.AllBuffered, listPlayerInfos[i].GetPlayerNumber());
					}
				}
			}
			everybodyReady &= listPlayerInfos[i].ready;
		}

		if (everybodyReady) 
		{
			listPlayerInfos[listPlayerInfos.Length - 1].GetComponent<NetworkView>().RPC("LetTheGameBegin", RPCMode.AllBuffered);
			Application.LoadLevel(1);
		}


	}

	void OnGUI()
	{
		GUI.skin = backgroundMenu;
		GUI.BeginGroup (new Rect (Screen.width / 8, Screen.height / 8, 3 * Screen.width / 4, 3 * Screen.height / 4));
		GUI.Box (new Rect (0, 0, 3 * Screen.width / 4, 3 * Screen.height / 4), "");

		int space = (3 * Screen.height / 4) / 5 / 10;
		int backgroundHeight = ((3 * Screen.height / 4) - 9 * space) / 9;
		GUI.Box(new Rect( 10, space * 1, Screen.width / 2 - Screen.width / 8, backgroundHeight), "");
		GUI.Box (new Rect(10, space *  1, (Screen.width / 2 - Screen.width / 8) / 5, backgroundHeight), "Color");
		GUI.Box (new Rect(10 + (Screen.width / 2 - Screen.width / 8) / 5, space * 1, (Screen.width / 2 - Screen.width / 8) / 5, backgroundHeight), "Faction");
		GUI.Box (new Rect(10 + 2 *(Screen.width / 2 - Screen.width / 8) / 5, space * 1, (Screen.width / 2 - Screen.width / 8) / 5, backgroundHeight), "Username");
		GUI.Box (new Rect(10 + 3 * (Screen.width / 2 - Screen.width / 8) / 5, space * 1, (Screen.width / 2 - Screen.width / 8) / 5, backgroundHeight), "Team");
		GUI.Box (new Rect(10 + 4 * (Screen.width / 2 - Screen.width / 8) / 5, space * 1, (Screen.width / 2 - Screen.width / 8) / 5, backgroundHeight), "Ready");

		//PlayerInfos[] listPlayerInfos = Object.FindObjectsOfType<PlayerInfos> ();
		//Debug.Log (listPlayerInfos.Length);

		int i = 1;
		while (i < 8 && i - 1 < listPlayerInfos.Length) 
		{	
			NetworkView playerInfosView = listPlayerInfos[i - 1].GetComponent<NetworkView>();
			if (playerInfosView.isMine)
			{
				GUI.skin = backgroundMySlot;
			}
			else 
			{
				GUI.skin = backgroundPlayerSlot;
			}

			GUI.Box(new Rect( 10, space * (i + 1) + backgroundHeight * i, Screen.width / 2 - Screen.width / 8, backgroundHeight), "");

			if (listPlayerInfos[i - 1].GetComponent<NetworkView>().isMine)
			{
				string new_name = GUI.TextField(new Rect( 10 + 2 * (Screen.width / 2 - Screen.width / 8) / 5, space * (i + 1) + backgroundHeight * i, (Screen.width / 2 - Screen.width / 8) / 5, backgroundHeight), listPlayerInfos[i - 1].username);
				if (new_name.Length < 10 && new_name != listPlayerInfos[i - 1].username)
				{
					listPlayerInfos[i - 1].GetComponent<NetworkView>().RPC("SetUsername", RPCMode.AllBuffered, new_name);
					listPlayerInfos[i - 1].username = new_name;
				}

				string readyButtonString;

				if (!listPlayerInfos[i -1].ready)
				{
					readyButtonString = "Ready";
				}
				else
				{
					GUI.Box (new Rect(10 + 4 * (Screen.width / 2 - Screen.width / 8) / 5, space * (i + 1) + backgroundHeight * i, (Screen.width / 2 - Screen.width / 8) / 5, backgroundHeight), "\u221A");
					readyButtonString = "Not Ready";
				}
				
				if (GUI.Button (new Rect (9 * (3 * Screen.width / 4) / 10, 9 * (3 * Screen.height / 4) / 10, (3 * Screen.width / 4) / 10 - 20, (3 * Screen.height / 4) / 10 - 10), readyButtonString))
				{
					playerInfosView.RPC("SetReady", RPCMode.AllBuffered);
				}



			}

			else 
			{
				GUI.Box(new Rect( 10 + 2 * (Screen.width / 2 - Screen.width / 8) / 5, space * (i + 1) + backgroundHeight * i, (Screen.width / 2 - Screen.width / 8) / 5, backgroundHeight), listPlayerInfos[i - 1].username);
			}
			i++;
		}

		while (i < 8)
		{
			GUI.skin = backgroundPlayerSlotEmpty;
			//int space = (3 * Screen.height / 4) / 5 / 9;
			//int backgroundHeight = ((3 * Screen.height / 4) - 9 * space) / 8;
			GUI.Box(new Rect( 10, space * (i + 1) + backgroundHeight * i, Screen.width / 2 - Screen.width / 8, backgroundHeight), ""); 
			i++;
		}

		 

		GUI.EndGroup ();
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		Debug.Log ("New Player connected from:" + player.ipAddress + ":" + player.port);
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log (player + "is disconnected");
		//Network.RemoveRPCs (player);
		Network.DestroyPlayerObjects (player);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Debug.Log ("The server has been disconnected");
		Application.LoadLevel (0);
	}
	
}
