using UnityEngine;
using System.Collections;

public class musicyn : MonoBehaviour {
	private bool ismusic;
	void OnGUI()
	{
		Rect rect = new Rect (-120, 40, -135, -115);
		ismusic = GUI.Toggle (rect, ismusic, "music");
	}
	// Use this for initialization
	void Start () {
	
	}
	public void slider()
	{
		OnGUI ();
	}
	
	// Update is called once per frame
	public void toggle () {
		OnGUI ();
		if (!ismusic) {
			AudioListener.volume = 0f;
		}
	}
}
