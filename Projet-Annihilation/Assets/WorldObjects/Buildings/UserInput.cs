using UnityEngine;
using System.Collections;
using RTS;

public class UserInput : MonoBehaviour 
{

	private Player player;


	// Use this for initialization
	void Start () 
	{
	
		player = transform.root.GetComponent< Player >();

	}
	
	// Update is called once per frame
	void Update () 
	{

		if (player.human || ((Network.isClient || Network.isServer) && player.GetComponent<NetworkView>().isMine)) 
		{
			MoveCamera();
			RotateCamera();
			MouseActivity(); 
			if (Input.GetMouseButtonUp(0))
			{
				player.test ++;
			}
		}
	}

	private void MoveCamera()
	{
		float xpos = Input.mousePosition.x;
		float ypos = Input.mousePosition.y;
		Vector3 movement = new Vector3 (0, 0, 0);

		bool mouseScroll = false;

		//horizontal camera movement
		if (xpos >= 0 && xpos < RessourceManager.ScrollWidth) 
		{
			movement.x -= RessourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanLeft);
			mouseScroll = true;
		} 
		else if (xpos > Screen.width - RessourceManager.ScrollWidth && xpos <= Screen.width) 
		{
			movement.x += RessourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanRight);
			mouseScroll = true;
		}

		//vertical camera movement
		if (ypos >= 0 && ypos < RessourceManager.ScrollWidth) 
		{
			movement.z -= RessourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanDown);
			mouseScroll = true;
		}
		else if (ypos > Screen.height - RessourceManager.ScrollWidth && ypos <= Screen.height)
		{
			movement.z += RessourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanUp);
			mouseScroll = true;
		}

		movement = Camera.main.transform.TransformDirection (movement);
		movement.y = 0;

		//away from the ground movement
		movement.y -= RessourceManager.ScrollSpeed * Input.GetAxis ("Mouse ScrollWheel");

		//calcul of the desirate position (with received input)
		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;

		//Vertical movement limits
		if (destination.y > RessourceManager.MaxCameraHeight) 
		{
			destination.y = RessourceManager.MaxCameraHeight;
		} 
		else if (destination.y < RessourceManager.MinCameraHeight) 
		{
			destination.y = RessourceManager.MinCameraHeight;
		}

		//make the move
		if (destination != origin)
		{
			Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * RessourceManager.ScrollSpeed);
		}

		if (!mouseScroll) 
		{
			player.hud.SetCursorState(CursorState.Select);
		}

	}
	
	private void RotateCamera()
	{
		Vector3 origin = Camera.main.transform.eulerAngles;
		Vector3 destination = origin;

		if ((Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt)) && Input.GetMouseButton(1)) 
		{
			destination.x -= Input.GetAxis("Mouse Y");
			destination.y += Input.GetAxis("Mouse X");
		}

		if (destination != origin) 
		{
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, 45);
		}

	}

	private void MouseActivity() //check if the player clicked
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			LeftMouseClick ();
		}
		else if(Input.GetMouseButtonDown (1))
		{
			RightMouseClick();
		}

		MouseHover ();

	}

	private GameObject FindHitObject() //recherche de l'objet sur lequel le joueur a clique
	{
		return WorkManager.FindHitObject (Input.mousePosition);

	}

	private Vector3 FindHitPoint() //verification que le point clique vise un "point cliquable"
	{
		return WorkManager.FindHitPoint (Input.mousePosition);
	}
	
	 private void LeftMouseClick()
	{
		if (player.hud.MouseInBounds ()) {
			if (player.IsFindingBuildingLocation ()) {
				if (player.CanPlaceBuilding ()) {
					player.StartConstruction ();
				}
			} else {
				GameObject hitObject = FindHitObject ();
				Vector3 hitPoint = FindHitPoint ();
				if (hitObject && hitPoint != RessourceManager.InvalidPosition) {

					if (hitObject.name != "Ground") {
						WorldObject worldObject = hitObject.transform.parent.GetComponent< WorldObject > ();
						if (worldObject) {
							if (player.SelectedObject) {
								player.SelectedObject.SetSelection (false, player.hud.GetPlayingArea ());
								player.SelectedObject = null;
							}
							player.SelectedObject = worldObject;
							worldObject.SetSelection (true, player.hud.GetPlayingArea ());
						}
					} else {
						if (player.hud.MouseInBounds () && player.SelectedObject) {
							player.SelectedObject.SetSelection (false, player.hud.GetPlayingArea ());
							player.SelectedObject = null;
						}
					}
				}
			}
		} 
		else 
		{
			if (player.IsFindingBuildingLocation())
			{
				player.CancelBuildingPlacement();
			}
		}
	}

	private void RightMouseClick()
	{

		GameObject hitObject = FindHitObject();
		Vector3 hitPoint = FindHitPoint();

		if (player.IsFindingBuildingLocation ()) 
		{
			player.CancelBuildingPlacement();
		}
		else if (player.SelectedObject && player.hud.MouseInBounds () && hitObject && hitPoint != RessourceManager.InvalidPosition)
		{
			player.SelectedObject.MouseClick(hitObject, hitPoint, player);
		}

	}

	private void MouseHover()
	{
		if (player.hud.MouseInBounds ()) 
		{
			if (player.IsFindingBuildingLocation()) 
			{
				player.FindBuildingLocation();
			}
			else 
			{
				GameObject hoverObject = FindHitObject ();
				if (hoverObject) 
				{
					if (player.SelectedObject) 
					{
						player.SelectedObject.SetHoverState (hoverObject);
					} 
					else if (hoverObject.name == "Ground") 
					{
						Player owner = hoverObject.transform.root.GetComponent< Player > ();
						if (owner) 
						{
							Unit unit = hoverObject.transform.root.parent.GetComponent< Unit > ();
							Building building = hoverObject.transform.parent.GetComponent< Building > ();
							if (owner.username == player.username && (unit || building)) 
							{
								player.hud.SetCursorState (CursorState.Select);
							}
						}
					}
					
				}
			}
		}
	}



}
