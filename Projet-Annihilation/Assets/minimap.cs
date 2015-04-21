using UnityEngine;
using System.Collections;

public class minimap : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void DrawMainCameraPosition()
	{
		Camera.main.ScreenToWorldPoint(new Vector3(0,0));
	}


}
