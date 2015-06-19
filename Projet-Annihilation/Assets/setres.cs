using UnityEngine;
using System.Collections;

public class setres : MonoBehaviour {
	private int larg;
	private int haut;
	private bool windowed;
	public void SetResolution (int lar, int hau)
	{
		windowed = true;
		larg = lar;
		haut = hau;
		Screen.SetResolution (larg, haut, windowed);
	}
	
	void onGUI () {
		Rect posWindow = new Rect (-80, 80, -70, -50);
		bool iswindow = windowed;
		iswindow = GUI.Toggle (posWindow, iswindow, "windowed");
		windowed = iswindow;
	}
	public void SetWindow()
	{
		onGUI ();
		Screen.SetResolution (larg, haut, windowed);
	}

}
