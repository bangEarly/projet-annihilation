using UnityEngine;
using System.Collections;
using RTS;

public class OreDeposit : Resource {

	private int numBlocks;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		numBlocks = GetComponentsInChildren< Ore > ().Length;
		resourceType = ResourceType.Ore;
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();
		float percentLeft = (float)amountLeft / (float)capacity;
		Ore[] blocks = GetComponentsInChildren< Ore > ();
		if (percentLeft <= 0) 
		{
			percentLeft = 0;
			Destroy(gameObject);
		}
		int numBlocksToShow = (int)(percentLeft * numBlocks);

		if (numBlocksToShow >= 0 && numBlocksToShow < blocks.Length) 
		{
			Ore[] sortedBlocks = new Ore[blocks.Length];

			foreach (Ore ore in blocks) 
			{
				sortedBlocks[blocks.Length - int.Parse(ore.name)] = ore;

			}
			for (int i = numBlocksToShow; i < sortedBlocks.Length; i++) 
			{
				sortedBlocks[i].GetComponent< Renderer >().enabled = false;
			}
			CalculateBounds ();
		}
	}


}
