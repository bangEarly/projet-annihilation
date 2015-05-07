using UnityEngine;
using System.Collections;

public class QG_robot : Building 
{
	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		actions = new string[] {"worker"};
		maxBuildProgress = 10.0f;
	}

	public override void PerformAction(string actionToPerform)
	{
		base.PerformAction (actionToPerform);
		CreatUnit (actionToPerform);
	}
}