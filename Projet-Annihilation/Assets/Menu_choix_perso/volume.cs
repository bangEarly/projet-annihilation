using UnityEngine;
using System.Collections;

public class volume : MonoBehaviour {

	private float slidervalue;
	void onGUI()
	{	
		Rect rect = new Rect (-30, 50, -135, -115);
		slidervalue = GUI.HorizontalSlider (rect, slidervalue, 0f, 10f);
		AudioListener.volume = slidervalue / 10f;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void setvolume () {
		onGUI ();
	}
}
