using UnityEngine;
using System.Collections;
using RTS;

public class HUD : MonoBehaviour {

	//resource bar and orders bar display
	public GUISkin ResourceSkin, OrdersSkin, selectBoxSkin;
	private const float ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40;

	//infos on the selection
	private const int SELECTION_NAME_HEIGHT = 15;

	//player that "own" the HUD
	private Player player;

	//Cursors
	public Texture2D activeCursor;
	public Texture2D selectCursor, leftCursor, rightCursor, upCursor, downCursor;
	public Texture2D[] moveCursors, attackCursors, harvestCursors;
	public GUISkin mouseCursorSkin;
	private CursorState activeCursorState;
	private int currentFrame = 0; 

	// Use this for initialization
	void Start () 
	{
		player = transform.root.GetComponent< Player> ();
		RessourceManager.StoreSelectBoxItems(selectBoxSkin);
		SetCursorState (CursorState.Select);

	}
	
	// Update is called once per frame
	void OnGUI () 
	{
		if (player && player.human) 
		{
			DrawResourceBar ();
			DrawOrdersBar ();
		}
		DrawMouseCursor ();
	}

	private void DrawResourceBar ()
	{
		GUI.skin = ResourceSkin;
		GUI.BeginGroup(new Rect(0, 0, Screen.width, RESOURCE_BAR_HEIGHT));
		GUI.Box (new Rect (0, 0, Screen.width, RESOURCE_BAR_HEIGHT), "");
		GUI.EndGroup ();
	}

	private void DrawOrdersBar ()
	{
		GUI.skin = OrdersSkin;
		GUI.BeginGroup (new Rect (Screen.width - ORDERS_BAR_WIDTH, RESOURCE_BAR_HEIGHT, ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT));
		GUI.Box (new Rect (0, 0, ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT), "");

		string selectionName = "";
		if (player.SelectedObject) 
		{
			selectionName = player.SelectedObject.objectName;
		}
		if (!selectionName.Equals (""))
		{
			GUI.Label(new Rect(0, 10, ORDERS_BAR_WIDTH, SELECTION_NAME_HEIGHT), selectionName);
		}

		GUI.EndGroup ();
	}

	
	public bool MouseInBounds() //verifie que le clique n'est pas dans le HUD
	{
		Vector3 mousePos = Input.mousePosition;
		bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width - ORDERS_BAR_WIDTH;
		bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height - RESOURCE_BAR_HEIGHT;
		return insideHeight && insideWidth;
	}

	public Rect GetPlayingArea () //get the playing area for the selection box
	{								//les coordonnee partent du point en haut a gauche de l'ecran
		return new Rect (0, RESOURCE_BAR_HEIGHT, Screen.width - ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT);
	}


	public void DrawMouseCursor()
	{
		bool mouseOverHUD = !MouseInBounds () && activeCursorState != CursorState.PanRight && activeCursorState != CursorState.PanUp;

		if (mouseOverHUD) 
		{
			Screen.showCursor = true;
		} 
		else 
		{
			Screen.showCursor = false;
			GUI.skin = mouseCursorSkin;
			GUI.BeginGroup (new Rect (0, 0, Screen.width, Screen.height));
			UpdateCursorAnimation ();
			Rect cursorPosition = GetCursorDrawPosition ();
			GUI.Label (cursorPosition, activeCursor);
			GUI.EndGroup ();
		}
	}

	private void UpdateCursorAnimation()
	{
		if (activeCursorState == CursorState.Move) 
		{
						currentFrame = (int)Time.time % moveCursors.Length;
						activeCursor = moveCursors [currentFrame];
		} 
		else if (activeCursorState == CursorState.Attack) 
		{
						currentFrame = (int)Time.time % attackCursors.Length;
						activeCursor = attackCursors [currentFrame];
		} 
		else if (activeCursorState == CursorState.Harvest) 
		{
			currentFrame = (int)Time.time % harvestCursors.Length;
			activeCursor = harvestCursors [currentFrame];
		}
	}

	private Rect GetCursorDrawPosition()
	{
		float leftPos = Input.mousePosition.x;
		float topPos = Screen.height - Input.mousePosition.y;

		if (activeCursorState == CursorState.PanRight) 
		{
			leftPos = Screen.width - activeCursor.width; // sinon le curseur s'affiche en dehors de l'ecran (a droite)
		}
		else if (activeCursorState == CursorState.PanDown) 
		{
			topPos = Screen.height - activeCursor.height; //sinon il s'affiche en dessous de l'ecran 
		} 
		else if (activeCursorState == CursorState.Move || activeCursorState == CursorState.Select || activeCursorState == CursorState.Harvest) 
		{
			topPos -= activeCursor.height / 2; //correction du placement
			leftPos -= activeCursor.width / 2;
		}
		return new Rect(leftPos, topPos, activeCursor.width, activeCursor.height);
	}

	public void SetCursorState(CursorState newState)
	{
		activeCursorState = newState;
		switch (newState) 
		{
			case CursorState.Select:
					activeCursor = selectCursor;
					break;
			case CursorState.Attack:
					currentFrame = (int)Time.time % attackCursors.Length;
					activeCursor = attackCursors [currentFrame];
					break;
			case CursorState.Harvest:
					currentFrame = (int)Time.time % harvestCursors.Length;
					activeCursor = harvestCursors [currentFrame];
					break;
			case CursorState.Move:
					currentFrame = (int)Time.time % moveCursors.Length;
					activeCursor = moveCursors [currentFrame];
					break;
			case CursorState.PanLeft:
					activeCursor = leftCursor;
					break;
			case CursorState.PanRight:
					activeCursor = rightCursor;
					break;
			case CursorState.PanUp:
					activeCursor = upCursor;
					break;
			case CursorState.PanDown:
					activeCursor = downCursor;
					break;
			default : 
					break;
		}
	}



}

