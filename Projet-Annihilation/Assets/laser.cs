using UnityEngine;
using System.Collections;

public class laser : MonoBehaviour {

	public WorldObject startPoint;
	public WorldObject endPoint;
	LineRenderer laserLine;

	// Use this for initialization
	void Start () 
	{
		laserLine = GetComponent<LineRenderer> ();
		laserLine.SetWidth (.2f, .2f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = startPoint.transform.position;
		transform.rotation = Quaternion.Inverse(Quaternion.FromToRotation (transform.position, endPoint.transform.position));
		laserLine.SetPosition (0, startPoint.transform.position);
		laserLine.SetPosition (1, endPoint.transform.position);
	}
}
