using UnityEngine;
using System.Collections;
using System.Threading;

public class StartNetwork : MonoBehaviour {

	public GameObject playerInfosPrefab;
	public string remoteIP;
	public int listenPort;
	public bool server;

	// Use this for initialization
	void Start () 
	{
		if (server) 
		{
			Network.InitializeServer (7, listenPort, true);


			GameObject playerInfosObject = (GameObject) Network.Instantiate(playerInfosPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0), 0);
			NetworkView playerInfosView = playerInfosObject.GetComponent<NetworkView>();
			playerInfosView.RPC("SetPlayerNumber", RPCMode.AllBuffered, 0);
			playerInfosView.RPC("SetHostTrue", RPCMode.AllBuffered);
			Application.LoadLevel (2);
		} 
		else 
		{

			Network.Connect(remoteIP, listenPort);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
}
