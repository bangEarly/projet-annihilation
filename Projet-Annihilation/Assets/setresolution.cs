using UnityEngine;
using System.Collections;

public class setresolution : MonoBehaviour {
	private int larg;
	private int haut;
	private bool windowed;

	public void switchres(int lar, int hau)
	{	larg = lar;
		haut = hau;
		Screen.SetResolution (lar, hau, !windowed);
	}
	void onGUI()
	{	
		Rect rect = new Rect (-80, 80, -60, -40);
		windowed = GUI.Toggle (rect, windowed, "windowed");

	}
	public void window()
	{
		onGUI ();
		switchres (larg, haut);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
