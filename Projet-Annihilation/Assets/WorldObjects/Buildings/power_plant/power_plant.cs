using UnityEngine;
using System.Collections;
using RTS;

public class power_plant : Building
{

	// Use this for initialization
	void Start () 
	{
		base.Start ();
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
			player.AddResource(ResourceType.Power, 500);
			//Debug.Log("oups");
			
		}
	}

}
