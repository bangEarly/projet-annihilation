using UnityEngine;
using System.Collections;
using RTS;
using System.Collections.Generic;
using System;

public class StartGame : MonoBehaviour {

	public List<Vector3> listSpawnPosition = new List<Vector3> {
		new Vector3 (-18f, 0.5f, 19f),
		new Vector3 (-285f, 0.5f, -8f),
		new Vector3 (-243f, 0.5f, -266f),
		new Vector3 (55f, 0.5f, -284f)
	};

	// Use this for initialization
	void Start () 
	{
		System.Random random = new System.Random ();
		if (Network.isServer || Network.isClient) 
		{
			/*GameObject playerObject = (GameObject)Network.Instantiate (RessourceManager.GetPlayerObject (), listSpawnPosition [random.Next(0,4)], new Quaternion (0, 0, 0, 0), 0);
			Player player = playerObject.GetComponent<Player> ();
			player.human = true;
			RessourceManager.SetActualPlayer (player);
			Camera.main.transform.position = new Vector3(player.transform.position.x, Camera.main.transform.position.y, player.transform.position.z);*/
		} 
		else 
		{
			GameObject playerObject = (GameObject)Instantiate (RessourceManager.GetPlayerObject (), listSpawnPosition [random.Next(0, 4)], new Quaternion (0, 0, 0, 0));
			Player player = playerObject.GetComponent<Player> ();
			player.human = true;
			RessourceManager.SetActualPlayer (player);
			Camera.main.transform.position = new Vector3(player.transform.position.x, Camera.main.transform.position.y, player.transform.position.z);
		}
	}

}
