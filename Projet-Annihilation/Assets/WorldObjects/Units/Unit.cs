using UnityEngine;
using System.Collections;
using RTS;

public class Unit : WorldObject 
{
	//variables movement
	protected bool moving, rotating;
	protected Vector3 destination;
	private Quaternion targetRotation;
	public float moveSpeed, rotateSpeed;
	protected NavMeshAgent agent;

	protected override void Awake () 
	{
		base.Awake ();
	}

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		agent = GetComponent<NavMeshAgent> ();
	}

	protected override void Update ()
	{	
		base.Update ();

		if (agent.velocity == Vector3.zero) 
		{
			movingIntoPosition = false;
		} 
		else 
		{
			CalculateBounds ();
			if (RessourceManager.networkIsConnected())
			{
				networkview.RPC("SyncPosition", RPCMode.AllBuffered, transform.position);
			}
		}
	}

	protected override void OnGUI()
	{
		base.OnGUI ();
	}

	public override void SetHoverState(GameObject hoverObject)
	{
		base.SetHoverState (hoverObject);

		if (player && player.human && currentlySelected) 
		{
			if (hoverObject.name == "Ground")
			{
				player.hud.SetCursorState(RTS.CursorState.Move);
			}
		}
	}

	public override void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
	{
		base.MouseClick (hitObject, hitPoint, controller);

		if (player && player.human && currentlySelected && (!RessourceManager.networkIsConnected() || player.GetComponent<NetworkView>().isMine)) 
		{
			if (hitObject.name == "Ground" && hitPoint != RessourceManager.InvalidPosition)
			{
				float x = hitPoint.x;
				float y = hitPoint.y;
				float z = hitPoint.z;

				Vector3 destination = new Vector3(x, y, z);
				attacking = false;
				target = null;
				StartMove(destination);
			}
		}
	}

	public virtual void StartMove(Vector3 destination)
	{
		this.destination = destination;
		agent.SetDestination (destination);
	}

	public void StartMove(Vector3 destination, GameObject destinationTarget)
	{
		this.destination = destination;
		CalculateTargetDestination (destinationTarget);
		StartMove (this.destination);
	}

	private void CalculateTargetDestination(GameObject destinationTarget) {
		//calculate number of unit vectors from unit centre to unit edge of bounds
		Vector3 originalExtents = selectionBounds.extents;
		Vector3 normalExtents = originalExtents;
		normalExtents.Normalize();
		float numberOfExtents = originalExtents.x / normalExtents.x;
		int unitShift = Mathf.FloorToInt(numberOfExtents);
		
		//calculate number of unit vectors from target centre to target edge of bounds
		WorldObject worldObject = destinationTarget.GetComponent< WorldObject >();
		if(worldObject) originalExtents = worldObject.GetSelectionBounds().extents;
		else originalExtents = new Vector3(0.0f, 0.0f, 0.0f);
		normalExtents = originalExtents;
		normalExtents.Normalize();
		numberOfExtents = originalExtents.x / normalExtents.x;
		int targetShift = Mathf.FloorToInt(numberOfExtents);
		
		//calculate number of unit vectors between unit centre and destination centre with bounds just touching
		int shiftAmount = targetShift + unitShift;
		
		//calculate direction unit needs to travel to reach destination in straight line and normalize to unit vector
		Vector3 origin = transform.position;
		Vector3 direction = new Vector3(destination.x - origin.x, 0.0f, destination.z - origin.z);
		direction.Normalize();

		//destination = center of destination - number of unit vectors calculated above
		//this should give us a destination where the unit will not quite collide with the target
		//giving the illusion of moving to the edge of the target and then stopping
		for(int i = 0; i < shiftAmount; i++) destination -= direction;
		destination.y = destinationTarget.transform.position.y;
	}

	public virtual void SetBuilding(Building creator)
	{
	}

	public virtual void GoToBuilding(Building project)
	{
	}

	[RPC] void SyncPosition(Vector3 position)
	{
		transform.position = position;
		destination = position;
		CalculateBounds ();
	}

	[RPC] void SetParent()
	{
		Units buildings = tempPlayer.transform.GetComponentInChildren<Units> ();
		transform.parent = buildings.transform;
		player = transform.root.GetComponent<Player> ();
	}

}
