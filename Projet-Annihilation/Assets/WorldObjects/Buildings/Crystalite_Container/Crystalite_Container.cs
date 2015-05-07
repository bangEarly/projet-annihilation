using UnityEngine;
using System.Collections;
using RTS;

public class Crystalite_Container : Building 
{
	
	// Use this for initialization
	void Start () 
	{
		base.Start ();
		actions = new string[] {"Harvester"};
		maxBuildProgress = 10.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update ();	
	}

	public override void Construct(float work)
	{
		workLeft -= work;
		if (RessourceManager.networkIsConnected ()) 
		{
			networkview.RPC ("SyncWorkLeft", RPCMode.AllBuffered, workLeft);
		}
		if (workLeft <= 0) 
		{
			if (RessourceManager.networkIsConnected())
			{
				networkview.RPC ("ConstructionFinished", RPCMode.AllBuffered);
			}
			else
			{
				workLeft = 0;
				hitPoints = maxHitPoints;
				RestoreMaterials();
			}
			player.IncrementResourceLimit (RTS.ResourceType.Crystalite, 1000);
			
		}
	}

	public override void PerformAction(string actionToPerform)
	{
		base.PerformAction (actionToPerform);
		CreatUnit (actionToPerform);
	}

}
