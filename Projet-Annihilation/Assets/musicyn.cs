using UnityEngine;
using System.Collections;

public class musicyn : MonoBehaviour {
	private int ismusic;
	void onGUI()
	{
		Rect rect = new Rect (-120, 40, -135, -115);
		ismusic = GUI.Toggle (rect, ismusic, "music");
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		onGUI ();
		if (ismusic) {
			AudioListener.volume = 0f;
		}
	}
}
