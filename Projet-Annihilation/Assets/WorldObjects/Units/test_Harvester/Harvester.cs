using UnityEngine;
using System.Collections;
using RTS;

public class Harvester : Unit 
{

	public float capacity;

	private bool harvesting = false, emptying = false;
	private float currentLoad = 0.0f;
	private ResourceType harvestType;
	private Resource resourceDeposit;
	public Building resourceStore;

	public float collectionAmount, depositAmount;
	private float currentDeposit = 0.0f;

	private GUIStyle fullStyle = new GUIStyle ();

	// Use this for initialization
	protected override void  Start () 
	{
		base.Start ();
		harvestType = ResourceType.Unknown;
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();

		if ((transform.position.x - destination.x < 0.5 && transform.position.x - destination.x > -0.5) && 
		    (transform.position.y - destination.y < 1  && transform.position.y - destination.y > -1) && 
		    (transform.position.z - destination.z < 0.5 && transform.position.z - destination.z > -0.5) && agent.velocity == new Vector3(0,0,0)) 
		{

			if (harvesting || emptying) 
			{
				Arms[] arms = GetComponentsInChildren<Arms> ();
				foreach (Arms arm in arms) {
					arm.GetComponent<Renderer> ().enabled = true;
				}

				if (harvesting) 
				{
					Collect ();
					if (currentLoad >= capacity || resourceDeposit.isEmpty ()) 
					{
						currentLoad = Mathf.Floor (currentLoad);
						harvesting = false;
						emptying = true;
						foreach (Arms arm in arms) 
						{
							arm.GetComponent<Renderer> ().enabled = false;
						}
						StartMove (resourceStore.transform.position, resourceStore.gameObject);
					}
				} 
				else 
				{
					Deposit ();
					if (currentLoad <= 0) 
					{
						emptying = false;
						foreach (Arms arm in arms) 
						{
							arm.GetComponent<Renderer> ().enabled = false;
						}
						if (!resourceDeposit.isEmpty ()) 
						{
							harvesting = true;
							StartMove (resourceDeposit.transform.position, resourceDeposit.gameObject);
						}
					}
				}

			}
		}
	}

	protected override void OnGUI ()
	{
		if (harvesting || emptying || currentlySelected) 
		{
			DrawSelection ();
		}
	}

	public override void SetHoverState(GameObject hoverObject)
	{
		base.SetHoverState(hoverObject);

		if (player && player.human && currentlySelected)
		{
			if (hoverObject.name != "Ground")
			{
				Resource resource = hoverObject.transform.parent.GetComponent<Resource>();
				if (resource && !resource.isEmpty()) 
				{
					player.hud.SetCursorState(CursorState.Harvest);
				}
			}
		}
	}

	public override void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
	{
		base.MouseClick (hitObject, hitPoint, controller);

		if (player && player.human) 
		{
			if (hitObject.name != "Ground")
			{
				Resource resource = hitObject.transform.parent.GetComponent<Resource>();

				if (resource && !resource.isEmpty())
				{
					StartHarvest(resource);
				}
			}
			else
			{
				StopHarvest ();
			}
		}

	}

	private void StartHarvest(Resource resource)
	{
		Debug.Log ("Beginning of the harvest");
		resourceDeposit = resource;

		if(harvestType == ResourceType.Unknown || harvestType != resource.GetResourceType()) {
			harvestType = resource.GetResourceType();
			currentLoad = 0.0f;
		}

		StartMove (resource.transform.position, resource.gameObject);
		harvesting = true;
		emptying = false;
	}

	private void StopHarvest()
	{
		harvesting = false;
		emptying = false;
		currentLoad = 0.0f;
		harvestType = ResourceType.Unknown;
	}

	private void Collect()
	{
		float collect = collectionAmount * Time.deltaTime;

		if (currentLoad + collect > capacity) 
		{
			collect = capacity - currentLoad;
		}

		resourceDeposit.Remove (collect);
		currentLoad += collect;
	}

	private void Deposit()
	{
		currentDeposit += depositAmount * Time.deltaTime;
		int deposit = Mathf.FloorToInt (currentDeposit);
		if (deposit >= 1) 
		{
			if (deposit > currentLoad)
			{
				deposit = Mathf.FloorToInt(currentLoad);
			}
			currentDeposit -= deposit;
			currentLoad -= deposit;
			ResourceType depositType = harvestType;
			if (harvestType == ResourceType.Ore)
			{
				depositType = ResourceType.Crystalite;
			}
			player.AddResource(depositType, deposit);
		}
	}

	protected override void DrawSelectionBox (Rect selectBox)
	{
		if (harvesting || emptying) 
		{
			float percentFull = currentLoad / capacity;
			fullStyle.normal.background = RessourceManager.ResourceHealthBar;
			GUI.Label ( new Rect (selectBox.x, selectBox.y - 20, selectBox.width * percentFull, 3), "", fullStyle);
		}
		if (currentlySelected) 
		{
			base.DrawSelectionBox (selectBox);
		}
	}

}
