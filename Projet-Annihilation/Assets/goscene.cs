using UnityEngine;
using System.Collections;

public class goscene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void changescene (int scene) {
		Application.LoadLevel (scene);
	}
}
