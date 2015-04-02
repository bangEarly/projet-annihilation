using UnityEngine;
using System.Collections;

public class Worker : Unit {

	public int buildSpeed;

	private Building currentProject;
	private bool building = false;
	private float amountBuilding = 0.0f;

	// Use this for initialization
	void Start ()
	{
		base.Start ();
		actions = new string[] {"WarFactory"};
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update ();
	}

	public override void SetBuilding (Building project)
	{
		base.SetBuilding (project);
		currentProject = project;
		StartMove (currentProject.transform.position, currentProject.gameObject);
		building = true;
	}

	public override void PerformAction (string actionToPerform)
	{
		base.PerformAction (actionToPerform);
		CreateBuilding (actionToPerform);
	}

	private void CreateBuilding(string buildingName)
	{
		Vector3 buildPoint = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 10);
		if (player) 
		{
			player.
		}

	}
}
