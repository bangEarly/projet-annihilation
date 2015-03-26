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

		if (percentLeft < 0) 
		{
			percentLeft = 0;
		}
		int numBlocksToShow = (int)(percentLeft * numBlocks);
		Ore[] blocks = GetComponentsInChildren< Ore > ();
		//print (blocks.Length);
		if (numBlocksToShow >= 0 && numBlocksToShow < blocks.Length) 
		{
			Ore[] sortedBlocks = new Ore[blocks.Length];

			foreach (Ore ore in blocks) 
			{
				sortedBlocks[blocks.Length - int.Parse(ore.name)] = ore;
				//print ("ore n:");
				//print ( ore.name);
			}
			for (int i = numBlocksToShow; i < sortedBlocks.Length; i++) 
			{
				sortedBlocks[i].GetComponent< Renderer >().enabled = false;
			}
			CalculateBounds ();
		}
	}


}
