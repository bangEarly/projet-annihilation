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
		amountLeft -= amount;
		if (amountLeft < 0) 
		{
			amountLeft = 0;
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
}
