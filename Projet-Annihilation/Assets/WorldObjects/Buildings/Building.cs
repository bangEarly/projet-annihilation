using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Building : WorldObject {

	public float maxBuildProgress;
	protected Queue< string > buildQueue;
	private float currentBuildProgress = 0.0f;
	private Vector3 spawnPoint;

	private bool needsBuilding = false;

	public float workLeft;

	protected override void Awake()
	{
		base.Awake ();
		buildQueue = new Queue<string> ();
		float spawnX = selectionBounds.center.x + transform.forward.x * selectionBounds.extents.x + transform.forward.x * 10;
		float spawnZ = selectionBounds.center.z + transform.forward.z * selectionBounds.extents.z + transform.forward.z * 10;
		spawnPoint = new Vector3 (spawnX, 0.0f, spawnZ);
	}
	
	// Use this for initialization
	protected override void Start () {
	
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
	
		base.Update ();
		ProcessBuildQueue ();
	}

	protected override void OnGUI()
	{
		base.OnGUI ();
	}

	protected void CreatUnit(string unitName)
	{
		buildQueue.Enqueue (unitName);
	}

	protected void ProcessBuildQueue()
	{
		if (buildQueue.Count > 0) 
		{
			currentBuildProgress += Time.deltaTime * RessourceManager.BuildSpeed;
			if (currentBuildProgress > maxBuildProgress)
			{
				if (player)
				{
					player.AddUnit (buildQueue.Dequeue(), spawnPoint, transform.rotation);
					currentBuildProgress = 0.0f;
				}
			}
		}
	}

	public string[] getBuildQueueValues()
	{
		string[] values = new string[buildQueue.Count];
		int i = 0;
		foreach (var unit in buildQueue) 
		{
			values[i] = unit;
			i += 1;
		}
		return values;
	}

	public float getBuildPercentage()
	{
		return currentBuildProgress / maxBuildProgress;
	}

	public void StartConstruction()
	{
		CalculateBounds ();
		needsBuilding = true;
		hitPoints = 0;
	}

	public void Construct(float work)
	{
		workLeft -= work;
		if (workLeft <= 0) 
		{
			workLeft = 0;
			RestoreMaterials();
		}
	}

	public bool isBuilt()
	{
		return workLeft == 0;
	}

}
