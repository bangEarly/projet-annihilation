using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Player : MonoBehaviour {

	public string username;
	public bool human;
	public HUD hud;
	public WorldObject SelectedObject { get; set; }
	public List<WorldObject> selectedObjects = new List<WorldObject>();

	public Material notAllowedMaterial, allowedMaterial, inConstructionMaterial;

	private Building tempBuilding;
	private Unit tempCreator, unitToAdd;
	private bool findingPlacement = false;

	//ressources
	public int startCrystalite, startCrystaliteLimit, startDilithium, startDilithiumLimit, startPower;
	private Dictionary< ResourceType, int> resources, resourceLimits;

	public int test = 0;

	public int teamNumber;

	public Color teamColor;

    public bool isDead = false, won = false;

	void Awake()
	{
		resources = InitResourceList ();
		resourceLimits = InitResourceList ();
	}

	// Use this for initialization
	void Start () 
	{
		hud = GetComponentInChildren< HUD > ();

		//ajout des ressources de depart ainsi que la limite des ressources de depart
		AddStartResourceLimits ();
		AddStartResources ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (human) 
		{
			hud.SetResourcesValues(resources, resourceLimits);

			if (findingPlacement)
			{
				tempBuilding.CalculateBounds();
				if (CanPlaceBuilding())
				{
					tempBuilding.SetTransparentMaterial(allowedMaterial, false);
				}
				else
				{
					tempBuilding.SetTransparentMaterial(notAllowedMaterial, false);
				}
			}
		}
	}

	private Dictionary<ResourceType, int> InitResourceList()
	{
		Dictionary<ResourceType, int> list = new Dictionary<ResourceType, int> ();
		list.Add (ResourceType.Crystalite, 0);
		list.Add (ResourceType.Dilithium, 0);
		list.Add (ResourceType.Power, 0);
		return list;
	}

	public void AddResource(ResourceType type, int amount)
	{
		if (resources [type] + amount <= resourceLimits [type]) 
		{
			resources [type] += amount;
		}
	}

	public int GetResource(ResourceType resource)
	{
		return resources [resource];
	}

	public void IncrementResourceLimit(ResourceType type, int amount)
	{
		resourceLimits [type] += amount;
	}

	private void AddStartResourceLimits()
	{
		IncrementResourceLimit (ResourceType.Crystalite, startCrystaliteLimit);
		IncrementResourceLimit (ResourceType.Dilithium, startDilithiumLimit);
		IncrementResourceLimit (ResourceType.Power, 10000000);
	}

	private void AddStartResources()
	{
		AddResource (ResourceType.Crystalite, startCrystalite);
		AddResource (ResourceType.Dilithium, startDilithium);
		AddResource (ResourceType.Power, startPower);
	}

	public void AddUnit(string unitName, Vector3 spawnPoint, Quaternion rotation)
	{
		Debug.Log ("add" + unitName + "to player");

		Units units = GetComponentInChildren< Units > ();
		if (RessourceManager.networkIsConnected() && transform.GetComponent<NetworkView>().isMine) 
		{
			GameObject newUnit = (GameObject)Network.Instantiate (RessourceManager.GetUnit (unitName), spawnPoint, rotation, 0);
			unitToAdd = newUnit.GetComponent<Unit>();
			NetworkView playerView = transform.GetComponent<NetworkView>();
			//playerView.RPC("SetPlayerToUnit", RPCMode.AllBuffered);
			newUnit.transform.GetComponent<NetworkView>().RPC("SetParent", RPCMode.AllBuffered);
			//newUnit.transform.parent = units.transform;
		} 
		else 
		{
			GameObject newUnit = (GameObject)Instantiate (RessourceManager.GetUnit (unitName), spawnPoint, rotation);
			newUnit.transform.parent = units.transform;
		}


	}

	public void CreatBuilding(string buildingName, Vector3 buildPoint, Unit creator, Rect playingArea)
	{
		GameObject newBuilding = (GameObject)Instantiate (RessourceManager.GetBuilding (buildingName), buildPoint, new Quaternion ());
		tempBuilding = newBuilding.GetComponent<Building> ();
		if (tempBuilding) 
		{
			tempCreator = creator;
			findingPlacement = true;
			tempBuilding.SetTransparentMaterial (notAllowedMaterial, true);
			tempBuilding.SetColliders (false);
			tempBuilding.SetPlayingArea (playingArea);
			tempBuilding.GetComponent<NavMeshObstacle>().enabled = false;
		} 
		else 
		{
			Destroy(newBuilding);
		}
	}

	public bool IsFindingBuildingLocation()
	{
		return findingPlacement;
	}

	public void FindBuildingLocation()
	{
		Vector3 newLocation = WorkManager.FindHitPoint (Input.mousePosition);
		newLocation.y = Terrain.activeTerrain.SampleHeight (newLocation);
		tempBuilding.transform.position = newLocation;

	}

	public bool CanPlaceBuilding()
	{
		bool canPlace = true;

		Bounds placeBounds = tempBuilding.GetSelectionBounds ();

		float cx = placeBounds.center.x;
		float cy = placeBounds.center.y;
		float cz = placeBounds.center.z;

		/*float ex = placeBounds.extents.x;
		float ey = placeBounds.extents.y;
		float ez = placeBounds.extents.z;*/



		List<Vector3> corners = new List<Vector3> ();

		for (int i = -(int)placeBounds.extents.x; i < placeBounds.extents.x; i+=2) 
		{
			for (int j = -(int)placeBounds.extents.y; j < placeBounds.extents.y; j+= 2) 
			{
				for (int h = -(int)placeBounds.extents.z; h < placeBounds.extents.z; h+=2) 
				{
					corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + i, cy + j, cz + h)));
				}
			}
		}
		//Debug.Log (x);

		foreach (Vector3 corner in corners) 
		{
			GameObject hitObject = WorkManager.FindHitObject(corner);

			if (hitObject && hitObject.name != "Ground")
			{
				//WorldObject worldObject = hitObject.transform.GetComponent< WorldObject >();
				WorldObject worldObject2 = hitObject.transform.GetComponentInParent< WorldObject >();
				if (worldObject2 && placeBounds.Intersects(worldObject2.GetSelectionBounds()))
				{
					canPlace = false;
				}
			}
			else if ((hitObject && hitObject.name == "Ground") || (tempBuilding.transform.position.y < Terrain.activeTerrain.SampleHeight(tempBuilding.transform.position)))
			{
				Vector3 temp = new Vector3 (tempBuilding.transform.position.x, Terrain.activeTerrain.SampleHeight(tempBuilding.transform.position), tempBuilding.transform.position.z);
				tempBuilding.transform.position = temp;
			}
		}
		return canPlace;
	}

	public void StartConstruction()
	{
		findingPlacement = false;
		Buildings buildings = GetComponentInChildren< Buildings > ();
		if (RessourceManager.networkIsConnected() && transform.GetComponent<NetworkView>().isMine)
		{

			GameObject networkBuilding = (GameObject)Network.Instantiate(RessourceManager.GetBuilding(tempBuilding.name.Replace("(Clone)", "")), tempBuilding.transform.position, tempBuilding.transform.rotation, 0);

			tempBuilding.DestroyObject();
			tempBuilding = networkBuilding.GetComponent<Building>();
			NetworkView tempBuildingView = tempBuilding.GetComponent<NetworkView>();
			NetworkView playerView = transform.GetComponent<NetworkView>();
			//playerView.RPC ("SetPlayerToBuilding", RPCMode.AllBuffered);
			tempBuildingView.RPC ("SetParent", RPCMode.AllBuffered);
			//if (buildings) 
			//{
			//	tempBuilding.transform.parent = buildings.transform;
			//}
			//tempBuilding.SetPlayer();
			//tempBuildingView.RPC("RPCSetMaterial", RPCMode.AllBuffered, inConstructionMaterial, true);
			tempBuildingView.RPC("RPCStartConstruction", RPCMode.AllBuffered);
			//NetworkView tempCreatorView = tempCreator.GetComponent<NetworkView>();
			playerView.RPC("SetBuildingToWorker", RPCMode.AllBuffered);
			tempBuilding.SetTransparentMaterial (inConstructionMaterial, true);
		}
		else
		{
			if (buildings) 
			{
				tempBuilding.transform.parent = buildings.transform;
			}
			tempBuilding.SetPlayer ();
			tempBuilding.SetColliders (true);

			tempBuilding.StartConstruction ();
			tempBuilding.SetTransparentMaterial (inConstructionMaterial, false);
			tempBuilding.GetComponent<NavMeshObstacle>().enabled = true;
		}
		tempCreator.SetBuilding (tempBuilding);
		float spawnX = tempBuilding.selectionBounds.center.x + tempBuilding.transform.forward.x * tempBuilding.selectionBounds.extents.x + tempBuilding.transform.forward.x * (float)1.5;
		float spawnZ = tempBuilding.selectionBounds.center.z + tempBuilding.transform.forward.z * tempBuilding.selectionBounds.extents.z + tempBuilding.transform.forward.z * (float)1.5;
		tempBuilding.spawnPoint = new Vector3 (spawnX, 0.0f, spawnZ);
	}

	public void CancelBuildingPlacement()
	{
		AddResource (ResourceType.Crystalite, tempBuilding.costCrystalite);
		AddResource (ResourceType.Dilithium, tempBuilding.costDilithium);
		AddResource (ResourceType.Power, tempBuilding.costPower);
		findingPlacement = false;
		Destroy (tempBuilding.gameObject);
		tempBuilding = null;
		tempCreator = null;
	}

	[RPC] void SetUsername(string username)
	{
		this.username = username;
	}

	[RPC] void SetTeamNumber(int number)
	{
		teamNumber = number;
	}

	[RPC] void SetHuman()
	{
		human = true;
	}

	/*[RPC] void SetPlayerToBuilding()
	{
		tempBuilding.SetPlayer (this);
	}*/

	/*[RPC] void SetPlayerToUnit()
	{
		unitToAdd.SetPlayer (this);
	}*/

	[RPC] void SetBuildingToWorker()
	{
		tempCreator.GoToBuilding (tempBuilding);
	}

	[RPC] void AddToList()
	{
		RessourceManager.AddPlayerToList (this);
	}

	[RPC] void SetDead()
	{
		isDead = true;
	}

}
