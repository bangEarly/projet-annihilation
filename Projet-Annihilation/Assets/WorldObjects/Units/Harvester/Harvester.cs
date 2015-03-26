using UnityEngine;
using System.Collections;
using RTS;

public class Harvester : Unit 
{

	public float capacity;

	private bool harvesting = false, emptying = false;
	private float currentLoad = 0.0f;
	private ResourceType harvestType;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
