using UnityEngine;
using System.Collections;

public class manage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void changeToScene (int scene) {
		Application.LoadLevel (scene);
	}

}
