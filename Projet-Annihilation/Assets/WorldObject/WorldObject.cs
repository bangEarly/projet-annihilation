using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour {

	public string objectName;
	public Texture2D buildImage;
	public int cost, sellValue, hitPoints, maxHitPoints;

	protected Player player;
	protected string[] actions = {};
	public bool currentlySelected = false;

	protected Bounds selectionBounds;

	protected virtual void Awake()
	{

	}

	// Use this for initialization
	protected virtual void Start () 
	{
		player = transform.root.GetComponentInChildren< Player > ();
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	protected virtual void OnGUI()
	{
		/*if (currentlySelected) 
		{
			DrawSelection();
		}*/
	}

	public void SetSelection(bool selected)
	{
		currentlySelected = selected;
	}

	public string[] GetActions()
	{
		return actions;
	}

	public virtual void PerformAction(string actionToPerform)
	{

	}

	public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controlleur) //gestion de l'evenement si le joueur clique
	{
		if (currentlySelected && hitObject && hitObject.name != "Ground") 
		{
			WorldObject worldObject = hitObject.transform.root.GetComponentInChildren< WorldObject >();
			if (worldObject)
			{
				ChangeSelection(worldObject, controlleur);
			}
		}
	}
		
	private void ChangeSelection(WorldObject worldObject, Player controller) //change la selection du joueur pour l'objet selectionne
	{
		SetSelection (false);
		if (controller.SelectedObject) 
		{
			controller.SelectedObject.SetSelection(false);
			controller.SelectedObject = worldObject;
			worldObject.SetSelection(true);
		}
	}

	/*private void DrawSelection() //dessine le cercle de selection autour de l'objet selectionne
	{
		GUI.skin = RessourceManager.SelectBoxskin;
		Rect selectBox = WorkManager.CalculateSelectionBox (selectionBounds, playingArea);
		GUI.BeginGroup (playingArea);
		DrawSelectionBox (selectBox);
		GUI.EndGroup ();
	}*/

	/*private void CalculateBounds()
	{
		selectionBounds = new Bounds (transform.position, Vector3.zero);

	}*/

}
