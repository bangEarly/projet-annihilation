using UnityEngine;
using System.Collections;

public class ui : MonoBehaviour
{

    public static GameObject Object1;
    private float _timer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _timer = BlackScreenTimer.Timer;
	}
}
