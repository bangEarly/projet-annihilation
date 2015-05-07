using UnityEngine;
using System.Collections;
using RTS;
using System.Collections.Generic;

public class HUD : MonoBehaviour {

	//resource bar and orders bar display
	public GUISkin ResourceSkin, OrdersSkin, selectBoxSkin;
	private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40;

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

	//Resources
	private Dictionary<ResourceType, int> resourceValues, resourceLimits;
	private const int ICON_WIDTH = 32, ICON_HEIGHT = 32, TEXT_WIDTH = 128, TEXT_HEIGHT = 32;
	public Texture2D[] resources;
	private Dictionary<ResourceType, Texture2D> resourceImages;

	//affichage des actions
	private WorldObject lastSelection;
	private float sliderValue;
	public Texture2D buttonHover, buttonClick;
	private const int BUILD_IMAGE_WIDTH = 64, BUILD_IMAGE_HEIGHT = 64, BUTTON_SPACING = 7, SCROLL_BAR_WIDTH = 22;
	private int buildAreaHeight = 0;

	//affichage de la file de construction
	private const int BUILD_IMAGE_PADDING = 8;
	public Texture2D buildFrame, buildMask;
	
	public float notEnoughtResourceTimer;

	public Texture2D healthy, damaged, critical, resourceHealth, construction;

	public RenderTexture minimap;

	// Use this for initialization
	void Start () 
	{
		player = transform.root.GetComponent< Player> ();
		RessourceManager.StoreSelectBoxItems(selectBoxSkin, healthy, damaged, critical, resourceHealth, construction);
		SetCursorState (CursorState.Select);
		resourceValues = new Dictionary<ResourceType, int> ();
		resourceLimits = new Dictionary<ResourceType, int> ();
		resourceImages = new Dictionary<ResourceType, Texture2D> ();
		for (int i = 0; i < resources.Length; i++) 
		{
			switch (resources[i].name) 
			{
			case "Crystalite" :
				resourceImages.Add(ResourceType.Crystalite, resources[i]);
				resourceValues.Add(ResourceType.Crystalite, 0);
				resourceLimits.Add(ResourceType.Crystalite, 0);
				break;
			case "Dilithium" :
				resourceImages.Add(ResourceType.Dilithium, resources[i]);
				resourceValues.Add(ResourceType.Dilithium, 0);
				resourceLimits.Add(ResourceType.Dilithium, 0);
				break;

			case "Power" :
				resourceImages.Add(ResourceType.Power, resources[i]);
				resourceValues.Add(ResourceType.Power, 0);
				resourceLimits.Add(ResourceType.Power, 0);
				break;
			default :
				break;
			}

		}
		buildAreaHeight = Screen.height - RESOURCE_BAR_HEIGHT - SELECTION_NAME_HEIGHT - 2 * BUTTON_SPACING;

	}
	
	// Update is called once per frame
	void OnGUI () 
	{
		if (player && player.human && (!RessourceManager.networkIsConnected() || player.GetComponent<NetworkView>().isMine) ) 
		{
			DrawResourceBar ();
			//DrawMiniMap();
			if (player.SelectedObject)
			{
				DrawOrdersBar ();
				if (notEnoughtResourceTimer > 0)
				{
					PrintNotEnoughResource();
				}
			}
			DrawMouseCursor ();
		}

	}

	private void DrawResourceBar ()
	{
		GUI.skin = ResourceSkin;
		GUI.BeginGroup(new Rect(0, 0, Screen.width, RESOURCE_BAR_HEIGHT));
		GUI.Box (new Rect (0, 0, Screen.width, RESOURCE_BAR_HEIGHT), "");
		int topPos = 4, iconLeft = 4, textLeft = 20;
		DrawResourceIcon (ResourceType.Crystalite, iconLeft, textLeft, topPos, true);
		iconLeft += TEXT_WIDTH;
		textLeft += TEXT_WIDTH;
		DrawResourceIcon (ResourceType.Dilithium, iconLeft, textLeft, topPos, true);
		iconLeft += TEXT_WIDTH;
		textLeft += TEXT_WIDTH;
		DrawResourceIcon (ResourceType.Power, iconLeft, textLeft, topPos, false);
		GUI.EndGroup ();
	}

	private void DrawOrdersBar ()
	{
		GUI.skin = OrdersSkin;
		GUI.BeginGroup(new Rect(Screen.width / 2 - 5, Screen.height - ORDERS_BAR_WIDTH, Screen.width, ORDERS_BAR_WIDTH));
		GUI.Box (new Rect (0, 0, Screen.width, ORDERS_BAR_WIDTH), "");

		string selectionName = player.SelectedObject.objectName;
		Building building = player.SelectedObject.GetComponent<Building>();
		//Debug.Log(player.SelectedObject.IsOwnedBy (player));
		if (player.SelectedObject.IsOwnedBy (player) && ((building && building.isBuilt()) || (!building))) 
		{
			//Debug.Log ("caca");
			if (lastSelection && lastSelection != player.SelectedObject) 
			{
				sliderValue = 0.0f;
			}

			DrawActions (player.SelectedObject.GetActions ());
			lastSelection = player.SelectedObject;
			Building selectedBuilding = lastSelection.GetComponent< Building > ();
			if (selectedBuilding) 
			{
				DrawbuildQueue (selectedBuilding.getBuildQueueValues (), selectedBuilding.getBuildPercentage ()); 
			}
		}

		if (!selectionName.Equals ("")) 
		{
			int leftPos = BUILD_IMAGE_WIDTH + SCROLL_BAR_WIDTH / 2;
			int topPos = buildAreaHeight + BUTTON_SPACING;
			GUI.Label (new Rect (0, 10, 128, SELECTION_NAME_HEIGHT), selectionName);
			GUI.DrawTexture(new Rect(0, SELECTION_NAME_HEIGHT, 128, 128), buildFrame);
			GUI.DrawTexture(new Rect(0, SELECTION_NAME_HEIGHT, 128, 128), player.SelectedObject.buildImage);
		}

		GUI.EndGroup ();
	}

	
	public bool MouseInBounds() //verifie que le clique n'est pas dans le HUD
	{
		Vector3 mousePos = Input.mousePosition;
		bool insideHeight;
		bool insideWidth;

		if (player.SelectedObject) 
		{
			if (mousePos.y >= Screen.height - RESOURCE_BAR_HEIGHT)
			{
				insideHeight = false;
				insideWidth = false;
			}

			else if(mousePos.y >= ORDERS_BAR_WIDTH && mousePos.y <= Screen.height - RESOURCE_BAR_HEIGHT)
			{
				insideHeight = true;
				insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width;
			}
			else 
			{
				insideHeight = true;
				insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width / 2;
			}
		} 

		else 
		{
			insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height - RESOURCE_BAR_HEIGHT;
			insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width;
		}
		return insideHeight && insideWidth;
	}

	public Rect GetPlayingArea () //get the playing area for the selection box
	{								//les coordonnee partent du point en haut a gauche de l'ecran
		return new Rect (0, RESOURCE_BAR_HEIGHT, Screen.width, Screen.height - RESOURCE_BAR_HEIGHT );
	}


	public void DrawMouseCursor()
	{
		bool mouseOverHUD = !MouseInBounds () && activeCursorState != CursorState.PanDown && activeCursorState != CursorState.PanUp && activeCursorState != CursorState.PanRight;

		if (mouseOverHUD) 
		{
			Cursor.visible = true;
		} 
		else 
		{
			Cursor.visible = false;

			if (!player.IsFindingBuildingLocation())
			{
				GUI.skin = mouseCursorSkin;
				GUI.BeginGroup (new Rect (0, 0, Screen.width, Screen.height));
				UpdateCursorAnimation ();
				Rect cursorPosition = GetCursorDrawPosition ();
				GUI.Label (cursorPosition, activeCursor);
				GUI.EndGroup ();
			}
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

		if (activeCursorState == CursorState.PanLeft || activeCursorState == CursorState.PanRight) 
		{
			topPos -= activeCursor.height / 2;
			if (activeCursorState == CursorState.PanRight) 
			{
				leftPos = Screen.width - activeCursor.width; // sinon le curseur s'affiche en dehors de l'ecran (a droite)
			}
		} 

		else if (activeCursorState == CursorState.PanUp || activeCursorState == CursorState.PanDown) 
		{
			leftPos -= activeCursor.width / 2;
			if (activeCursorState == CursorState.PanDown) 
			{
				topPos = Screen.height - activeCursor.height; //sinon il s'affiche en dessous de l'ecran 
			}
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

	public void SetResourcesValues(Dictionary<ResourceType, int> resourceValues, Dictionary<ResourceType, int> resourceLimits)
	{
		this.resourceValues = resourceValues;
		this.resourceLimits = resourceLimits;
	}

	private void DrawResourceIcon(ResourceType type, int iconLeft, int textLeft, int topPos, bool display_limit)
	{
		Texture2D icon = resourceImages [type];
		string text = resourceValues [type].ToString ();
		if (display_limit) 
		{
			text += "/" + resourceLimits[type].ToString();
		}
		GUI.DrawTexture (new Rect (iconLeft, topPos, ICON_WIDTH, ICON_HEIGHT), icon);
		GUI.Label (new Rect (textLeft, topPos, TEXT_WIDTH, TEXT_HEIGHT), text);
	}

	private void DrawActions(string[] actions)
	{
		GUIStyle buttons = new GUIStyle();
		buttons.hover.background = buttonHover;
		buttons.active.background = buttonClick;
		GUI.skin.button = buttons;
		int numActions = actions.Length;
		GUI.BeginGroup(new Rect(Screen.width / 4, 0, BUILD_IMAGE_WIDTH * 4, buildAreaHeight));

		if (numActions >= MaxNumRows (buildAreaHeight))
		{
			DrawSlider (buildAreaHeight, numActions / 2.0f);
		}

		for (int i = 0; i < numActions; i++) 
		{
			int column = i % 3;
			int row = i / 3;
			Rect pos = GetButtonPos (row, column);
			Texture2D action = RessourceManager.GetBuildImage(actions[i]);
			if (action)
			{
				if (GUI.Button(pos, action))
				{
					if (player.SelectedObject)
					{
						player.SelectedObject.PerformAction(actions[i]);
					}
				}
			}
		}
		GUI.EndGroup();

	}

	private int MaxNumRows (int areaHeight)
	{
		return areaHeight / BUILD_IMAGE_HEIGHT;
	}

	private Rect GetButtonPos(int row, int column)
	{
		int left = SCROLL_BAR_WIDTH + column * BUILD_IMAGE_WIDTH;
		float top = row * BUILD_IMAGE_HEIGHT - sliderValue * BUILD_IMAGE_HEIGHT;
		return new Rect (left, top, BUILD_IMAGE_WIDTH, BUILD_IMAGE_HEIGHT);
	}

	private void DrawSlider(int groupHeight, float numRows)
	{
		sliderValue = GUI.VerticalSlider (GetScrollPos (groupHeight), sliderValue, 0.0f, numRows - MaxNumRows (groupHeight));
	}

	private Rect GetScrollPos(int groupHeight)
	{
		return new Rect (BUTTON_SPACING, BUTTON_SPACING, SCROLL_BAR_WIDTH, groupHeight - 2 * BUTTON_SPACING);
	}

	private void DrawbuildQueue(string[] buildQueue, float buildPourcentage)
	{
		for (int i = 0; i < buildQueue.Length; i++) 
		{
			float topPos = i * BUILD_IMAGE_HEIGHT - (i+1) * BUILD_IMAGE_PADDING + BUILD_IMAGE_PADDING;
			Rect buildPos = new Rect(128, topPos, BUILD_IMAGE_WIDTH, BUILD_IMAGE_HEIGHT);
			GUI.DrawTexture(buildPos, RessourceManager.GetBuildImage(buildQueue[i]));
			GUI.DrawTexture(buildPos, buildFrame);
			topPos += BUILD_IMAGE_PADDING;
			float width = BUILD_IMAGE_WIDTH - 2 * BUILD_IMAGE_PADDING;
			float height = BUILD_IMAGE_HEIGHT - 2 * BUILD_IMAGE_PADDING;
			if (i == 0)
			{
				topPos += height * buildPourcentage;
				height *= (1 - buildPourcentage);
			}
			GUI.DrawTexture(new Rect(128 + BUILD_IMAGE_PADDING, topPos, width, height), buildMask);
		}
	}

	public void DrawMiniMap()
	{
		GUI.BeginGroup (new Rect (0, Screen.height - Screen.height / 5, Screen.height / 5, Screen.height / 5));
		GUI.Box (new Rect (0, 0, Screen.height / 5, Screen.height / 5), minimap);
		GUI.EndGroup ();
	}

	public void PrintNotEnoughResource()
	{
		GUI.BeginGroup (GetPlayingArea ());
		GUI.TextArea (new Rect (Screen.width / 2, Screen.height / 3, 160, 20), "Not enough resources!");
		GUI.EndGroup ();
		notEnoughtResourceTimer -= Time.deltaTime;
	}

}

