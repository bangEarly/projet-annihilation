using UnityEngine;
using System.Collections;

public class menu_choix_personnage : MonoBehaviour 
{

	public GUISkin backgroundMenu, backgroundPlayerSlot, backgroundMySlot, backgroundPlayerSlotEmpty;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

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
		//GUI.Box (new Rect(10 + (Screen.width / 2 - Screen.width / 8) / 5, space * 1, (Screen.width / 2 - Screen.width / 8) / 5, backgroundHeight), "Faction");

		PlayerInfos[] listPlayerInfos = Object.FindObjectsOfType<PlayerInfos> ();
		//Debug.Log (listPlayerInfos.Length);

		int i = 1;
		while (i < 8 && i - 1 < listPlayerInfos.Length) 
		{	
			//Debug.Log(listPlayerInfos[i].GetComponent<NetworkView>().isMine);
			if (listPlayerInfos[i - 1].GetComponent<NetworkView>().isMine)
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
		/*PlayerInfos[] listPlayerInfos = Object.FindObjectsOfType<PlayerInfos> ();
		int i = 0;
		while (i < listPlayerInfos.Length && listPlayerInfos[i].GetComponent<NetworkView>().owner != player) {
			i++;
		}

		if (i >= listPlayerInfos.Length) {
			Debug.Log ("Error, this player doesn't have any PlayerInfos object");
		} 
		else 
		{
			listPlayerInfos[i].playerNumber = Network.connections.Length;
			listPlayerInfos[i].GetComponent<NetworkView>().RPC("SetPlayerNumber", RPCMode.AllBuffered, Network.connections.Length);
			Debug.Log("coucou j'ai rajoute un nombre");
		}*/
	}
}
