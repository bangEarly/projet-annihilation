using UnityEngine;
using System.Collections;
using RTS;

public class Unit : WorldObject 
{
	//variables movement
	protected bool moving, rotating;
	private Vector3 destination;
	private Quaternion targetRotation;
	public float moveSpeed, rotateSpeed;
	private NavMeshAgent agent;

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
		if (transform.position != destination) 
		{
			CalculateBounds ();
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

		if (player && player.human && currentlySelected) 
		{
			if (hitObject.name == "Ground" && hitPoint != RessourceManager.InvalidPosition)
			{
				float x = hitPoint.x;
				float y = hitPoint.y;
				float z = hitPoint.z;

				Vector3 destination = new Vector3(x, y, z);
				StartMove(destination);
			}
		}
	}

	public void StartMove(Vector3 destination)
	{
		this.destination = destination;
		agent.SetDestination (destination);
	}

}
