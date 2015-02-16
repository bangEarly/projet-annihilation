using UnityEngine;
using System.Collections;
using RTS;

public class HUD : MonoBehaviour {
	
	public GUISkin ResourceSkin, OrdersSkin, selectBoxSkin;
	private const float ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40;
	private const int SELECTION_NAME_HEIGHT = 15;
	private Player player;

	// Use this for initialization
	void Start () 
	{
		player = transform.root.GetComponent< Player> ();
		RessourceManager.StoreSelectBoxItems(selectBoxSkin);
	}
	
	// Update is called once per frame
	void OnGUI () 
	{
		if (player && player.human) 
		{
			DrawResourceBar ();
			DrawOrdersBar ();
		}
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

}
