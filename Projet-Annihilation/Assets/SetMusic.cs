using UnityEngine;
using System.Collections;

public class SetMusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void setVolume () {
		float hSliderValue = 0f;
		onGUI (hSliderValue);
		AudioListener.volume = hSliderValue;
	}
	void onGUI(float hSliderValue)
	{	

		Rect rect = new Rect (-30, 130, -135, 115);
		hSliderValue = GUI.HorizontalSlider (rect, hSliderValue, 0f, 10f);
		 hSliderValue= hSliderValue/10f;
	}
}
