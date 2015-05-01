using UnityEngine;
using System.Collections;

public class multi_fonctions : MonoBehaviour 
{
	public GameObject networkMaster, playerInfosPrefab;
	public StartNetwork startNetwork;
	public string serverIP = "127.0.0.1"; 
	public int serverPort = 25000;
	public GUISkin separation;

	public void OnGUI()
	{
		GUI.skin = separation;
		GUI.BeginGroup(new Rect(0,0, Screen.width, Screen.height));
		int sizeButtonX = 250;
		int sizeButtonY = 30;

		serverIP = GUI.TextField(new Rect(5 * Screen.width / 8 + 30, Screen.height / 2 - 60, 120, 30), serverIP, 40);

		GUI.Box (new Rect (Screen.width / 2, Screen.height / 4, 6, 2 * Screen.height / 3), "");

		if (GUI.Button(new Rect(Screen.width/8, Screen.height / 2, sizeButtonX, sizeButtonY), "Creer une partie")) 
		{
			GameObject instantiateMaster = Instantiate (networkMaster);
			startNetwork = instantiateMaster.GetComponent < StartNetwork > ();
			startNetwork.server = true;
			startNetwork.listenPort = serverPort;
		}

		if (GUI.Button(new Rect( 5 * Screen.width / 8, Screen.height / 2, sizeButtonX, sizeButtonY), "Rejoindre une partie"))
		{
			GameObject instantiateMaster = Instantiate (networkMaster);
			startNetwork = instantiateMaster.GetComponent <StartNetwork> ();
			startNetwork.server = false;
			startNetwork.remoteIP = serverIP;
			startNetwork.listenPort = serverPort;
		}

		GUI.EndGroup ();

	
	}

	public void OnConnectedToServer()
	{
		Debug.Log ("Connection effectue");
		Debug.Log ("peerType: " + Network.peerType);
		Debug.Log ("isClient: " + Network.isClient);
		GameObject playerInfosObject = (GameObject) Network.Instantiate(playerInfosPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0), 0);
		Application.LoadLevel (2);
	}



}
