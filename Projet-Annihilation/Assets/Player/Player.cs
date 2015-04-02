using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Player : MonoBehaviour {

	public string username;
	public bool human;
	public HUD hud;
	public WorldObject SelectedObject { get; set; }

	public Material notAllowedMaterial, allowedMaterial;

	private Building tempBuilding;
	private Unit tempCreator;
	private bool findingPlacement = false;

	//ressources
	public int startCrystalite, startCrystaliteLimit, startDilithium, startDilithiudLimit,startPower, startPowerLimit;
	private Dictionary< ResourceType, int> resources, resourceLimits;


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
		AddStartResources ();
		AddStartResourceLimits ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (human) 
		{
			hud.SetResourcesValues(resources, resourceLimits);
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
		resources [type] += amount;
	}

	public void IncrementResourceLimit(ResourceType type, int amount)
	{
		resourceLimits [type] += amount;
	}

	private void AddStartResourceLimits()
	{
		IncrementResourceLimit (ResourceType.Crystalite, startCrystaliteLimit);
		IncrementResourceLimit (ResourceType.Dilithium, startDilithiudLimit);
		IncrementResourceLimit (ResourceType.Power, startPowerLimit);
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
		GameObject newUnit = (GameObject)Instantiate (RessourceManager.GetUnit (unitName), spawnPoint, rotation);
		newUnit.transform.parent = units.transform;
	}

	public void CreatBuilding(string buildingName, Vector3 buildPoint, Unit creator, Rect playingArea)
	{
		GameObject newBuilding = (GameObject)Instantiate (RessourceManager.GetBuilding (buildingName), buildPoint, new Quaternion ());
		tempBuilding = newBuilding.GetComponent<Building> ();
		if (tempBuilding) 
		{
			tempCreator = creator;
			findingPlacement = true;
			tempBuilding.Set
		}
	}

}
