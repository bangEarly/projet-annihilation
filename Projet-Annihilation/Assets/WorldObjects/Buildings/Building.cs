using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Building : WorldObject {

	public float maxBuildProgress;
	protected Queue< string > buildQueue;
	private float currentBuildProgress = 0.0f;
	public Vector3 spawnPoint;

	private bool needsBuilding = false;

	public float workLeft, maxWork;
	private GUIStyle constructionBar = new GUIStyle ();

	protected override void Awake()
	{
		base.Awake ();
	}
	
	// Use this for initialization
	protected override void Start () {
	
		base.Start ();
		buildQueue = new Queue<string> ();
		float spawnX = selectionBounds.center.x + transform.forward.x * selectionBounds.extents.x + transform.forward.x * 20;
		float spawnZ = selectionBounds.center.z + transform.forward.z * selectionBounds.extents.z + transform.forward.z * 20;
		spawnPoint = new Vector3 (spawnX, 0.0f, spawnZ);
		//workLeft = maxWork;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
		base.Update ();
		ProcessBuildQueue ();
	}

	protected override void OnGUI()
	{
		if (currentlySelected || workLeft > 0) 
		{
			DrawSelection();
		}
	}

	protected void CreatUnit(string unitName)
	{
		Unit unitToMake = RessourceManager.GetUnit (unitName).GetComponent<Unit>();
		if (unitToMake.costCrystalite > player.GetResource (ResourceType.Crystalite) ||
			unitToMake.costDilithium > player.GetResource (ResourceType.Dilithium) ||
			unitToMake.costPower > player.GetResource (ResourceType.Power)) 
		{
			player.hud.notEnoughtResourceTimer = 3;
			Debug.Log ("Not enough resources!");
		} 
		else 
		{
			player.AddResource(ResourceType.Crystalite, - unitToMake.costCrystalite);
			player.AddResource(ResourceType.Dilithium, - unitToMake.costDilithium);
			player.AddResource(ResourceType.Power, - unitToMake.costPower);
			buildQueue.Enqueue (unitName);
		}
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
		hitPoints = 1;
	}

	public virtual void Construct(float work)
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

		}
	}

	public bool isBuilt()
	{
		return workLeft == 0;
	}

	protected override void DrawSelectionBox (Rect selectBox)
	{
		if (workLeft > 0) 
		{
			float percentageWork = workLeft / maxWork;
			constructionBar.normal.background = RessourceManager.ConstructionTexture;
			GUI.Label ( new Rect (selectBox.x, selectBox.y - 20, selectBox.width * percentageWork, 3), "", constructionBar);
		}
		if (currentlySelected) 
		{
			base.DrawSelectionBox(selectBox);
		}
	}

	public void DestroyObject()
	{
		Destroy (gameObject);
	}

	[RPC] void SetMaterial(Material material, bool storeExistingMaterial)
	{
		if (storeExistingMaterial) 
		{
			oldMaterials.Clear();
		}
		Renderer[] renderers = GetComponentsInChildren< Renderer > ();
		foreach (Renderer renderer in renderers) 
		{
			if (storeExistingMaterial)
			{
				oldMaterials.Add(renderer.material);
			}
			renderer.material = material;
		}
	}

	[RPC] void RPCStartConstruction()
	{
		CalculateBounds ();
		needsBuilding = true;
		hitPoints = 1;
	}

	[RPC] void SyncWorkLeft(float work)
	{
		workLeft = work;
	}

	[RPC] void ConstructionFinished()
	{
		workLeft = 0;
		hitPoints = maxHitPoints;
		RestoreMaterials();
	}

	[RPC] void SetParent()
	{
		Buildings buildings = tempPlayer.transform.GetComponentInChildren<Buildings> ();
		transform.parent = buildings.transform;
		player = transform.root.GetComponent<Player> ();
	}

}
