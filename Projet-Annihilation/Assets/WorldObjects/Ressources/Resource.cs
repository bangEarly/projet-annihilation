using UnityEngine;
using System.Collections;
using RTS;

public class Resource : WorldObject
{
	public int capacity;
	public float amountLeft;
	protected ResourceType resourceType;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		amountLeft = capacity;
		resourceType = ResourceType.Unknown;
	}
	
	public void Remove(float amount)
	{
		if (!RessourceManager.networkIsConnected()) {
			amountLeft -= amount;
			if (amountLeft <= 0) {
				amountLeft = 0;
				Destroy (gameObject);
			}
		} else {
			networkview.RPC ("RpcRemove", RPCMode.AllBuffered, amount);
			if (amountLeft <= 0)
			{
				Network.Destroy(networkview.viewID);
			}
		}
	}

	public bool isEmpty()
	{
		return amountLeft <= 0;
	}

	public ResourceType GetResourceType()
	{
		return resourceType;
	}

	protected override void CalculateCurrentHealth ()
	{
		healthPercentage = amountLeft / capacity;
		healthStyle.normal.background = RessourceManager.ResourceHealthBar;
	}

	[RPC] void RpcRemove(float amount)
	{
		amountLeft -= amount;
		if (amountLeft < 0) {
			amountLeft = 0;
		}
	}

}
