using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class WarFactory : Building {

	//public Dictionary<ResourceType, int> cost = new Dictionary<ResourceType, int> () {
	//	{ResourceType.Crystalite, 200},
	//	{ResourceType.Dilithium, 50}};

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		actions = new string[] {"Tank", "Tank", "Tank", "Harvester", "Harvester", "Harvester"};
		maxBuildProgress = 10.0f;
		//cost.Add (ResourceType.Crystalite, 200);
		//cost.Add (ResourceType.Dilithium, 50);
	}

	public override void PerformAction(string actionToPerform)
	{
		base.PerformAction (actionToPerform);
		CreatUnit (actionToPerform);
	}

}
