using UnityEngine;
using System.Collections;

public class gomain : MonoBehaviour {


	


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

	public void gotomain (int scene) {
		Application.LoadLevel (scene);
	}
}
