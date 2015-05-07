using UnityEngine;
using System.Collections;
using RTS;
using System.Collections.Generic;

public class Worker : Unit {

	public int buildSpeed;

	private Building currentProject;
	private bool building = false;
	private float amountBuilding = 0.0f;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		actions = new string[] {"WarFactory", "QGrobot", "tour_de_guet", "powerplant", "CrystaliteContainer", "WarFactory_test"};
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();

		if (agent.velocity == new Vector3 (0,0,0)) 
		{

			if (building)
			{
				currentProject.Construct(buildSpeed * Time.deltaTime);
				if (currentProject.workLeft <= 0)
				{
					StopBuilding();
				}
			}
		}
	}

	public override void SetBuilding (Building project)
	{
		base.SetBuilding (project);
		currentProject = project;
		StartMove (currentProject.transform.position, currentProject.gameObject);
		building = true;
	}

	public void StopBuilding()
	{
		currentProject = null;
		building = false;
	}

	public override void PerformAction (string actionToPerform)
	{
		base.PerformAction (actionToPerform);
		CreateBuilding (actionToPerform);
	}

	private void CreateBuilding(string buildingName)
	{
		Building buildingToBuild = RessourceManager.GetBuilding (buildingName).GetComponent<Building> (); 
		if (buildingToBuild.costCrystalite > player.GetResource (ResourceType.Crystalite) 
			|| buildingToBuild.costDilithium > player.GetResource (ResourceType.Dilithium)
			|| buildingToBuild.costPower > player.GetResource (ResourceType.Power)) 
		{
			player.hud.notEnoughtResourceTimer = 3;
			Debug.Log ("Not enough resources!");
		} 
		else 
		{
			player.AddResource (ResourceType.Crystalite, - buildingToBuild.costCrystalite);
			player.AddResource (ResourceType.Dilithium, - buildingToBuild.costDilithium);
			player.AddResource (ResourceType.Power, - buildingToBuild.costPower);

			Vector3 buildPoint = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 10);
			if (player) {
				player.CreatBuilding (buildingName, buildPoint, this, playingArea);
			}
		}

	}

	public override void MouseClick (GameObject hitObject, Vector3 hitPoint, Player controller)
	{
		base.MouseClick(hitObject, hitPoint, controller);

		if (player && player.human) 
		{
			if (hitObject.name != "Ground")
			{
				Building building = hitObject.transform.parent.GetComponent< Building >();
				if (building && !building.isBuilt())
				{
					SetBuilding(building);
				}
			}
			else
			{
				StopBuilding();
			}
		}
	}

}
