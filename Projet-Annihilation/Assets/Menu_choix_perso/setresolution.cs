using UnityEngine;
using System.Collections;

public class setresolution : MonoBehaviour {
	private int larg;
	private int haut;
	private bool windowed;
	public void switch9()
	{
		switchres (1920, 1080);
	}
	public void switch4()
	{
		switchres (854, 480);
	}
	public void switchhd()
	{
		switchres (1366, 768);
	}
	void switchres(int lar, int hau)
	{	larg = lar;
		haut = hau;
		Screen.SetResolution (lar, hau, !windowed);
	}
	void OnGUI()
	{	
		Rect rect = new Rect (-80, 80, -60, -40);
		windowed = GUI.Toggle (rect, windowed, "windowed");

	}
	public void window()
	{
		OnGUI ();
		switchres (larg, haut);
	}

}
