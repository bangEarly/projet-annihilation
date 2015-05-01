using UnityEngine;
using System.Collections;
using RTS;

public class GameInfos : MonoBehaviour {

	public Player actualPlayer;
	private static bool created = false;
	
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
}
