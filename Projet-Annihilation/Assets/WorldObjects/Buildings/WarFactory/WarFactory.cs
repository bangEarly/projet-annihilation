using UnityEngine;
using System.Collections;

public class WarFactory : Building {

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		actions = new string[] {"Tank", "Tank", "Tank", "Harvester", "Harvester", "Harvester"};
		maxBuildProgress = 10.0f;
	}

	public override void PerformAction(string actionToPerform)
	{
		base.PerformAction (actionToPerform);
		CreatUnit (actionToPerform);
	}

}
