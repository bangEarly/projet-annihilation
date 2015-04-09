using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class WorldObject : MonoBehaviour {

	public string objectName;
	public Texture2D buildImage;
	public int /*cost, sellValue,*/ hitPoints, maxHitPoints;

	public Dictionary<ResourceType, int> cost = new Dictionary<ResourceType, int> (), sellValue = new Dictionary<ResourceType, int> ();

	protected Player player;
	protected string[] actions = {};
	public bool currentlySelected = false;

	protected Bounds selectionBounds;
	protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

	private List< Material > oldMaterials = new List<Material> ();

	protected virtual void Awake()
	{
		selectionBounds = RessourceManager.InvalidBounds;
		CalculateBounds ();
	}

	// Use this for initialization
	protected virtual void Start () 
	{
		//cost = new Dictionary<ResourceType, int> ();
		SetPlayer ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	protected virtual void OnGUI()
	{
		if (currentlySelected) 
		{
			DrawSelection();
		}
	}

	public void SetSelection(bool selected, Rect playingArea)
	{
		currentlySelected = selected;
		if (selected) 
		{
			this.playingArea = playingArea;
		}
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
		/*if (currentlySelected && hitObject && hitObject.name != "Ground") 
		{
			WorldObject worldObject = hitObject.transform.parent.GetComponent< WorldObject >();
			if (worldObject)
			{
				ChangeSelection(worldObject, controlleur);
			}
		}*/
	}
		
	private void ChangeSelection(WorldObject worldObject, Player controller) //change la selection du joueur pour l'objet selectionne
	{
		SetSelection (false, playingArea);
		if (controller.SelectedObject) 
		{
			controller.SelectedObject.SetSelection(false, playingArea);
			controller.SelectedObject = worldObject;
			worldObject.SetSelection(true, controller.hud.GetPlayingArea());
		}
	}

	private void DrawSelection() //dessine le cercle de selection autour de l'objet selectionne
	{
		GUI.skin = RessourceManager.SelectBoxSkin;
		Rect selectBox = WorkManager.CalculateSelectionBox (selectionBounds, playingArea);
		GUI.BeginGroup (playingArea);
		DrawSelectionBox (selectBox);
		GUI.EndGroup ();
	}

	public void CalculateBounds()
	{
		selectionBounds = new Bounds (transform.position, Vector3.zero);
		foreach (Renderer r in GetComponentsInChildren< Renderer >() ) 
		{
			selectionBounds.Encapsulate(r.bounds);
		}
	}

	protected virtual void DrawSelectionBox(Rect selectBox)
	{
		GUI.Box (selectBox, "");
	}

	public virtual void SetHoverState(GameObject hoverObject)
	{
		if (player && player.human && currentlySelected) 
		{
			if (hoverObject.name != "Ground")
			{
				player.hud.SetCursorState(CursorState.Select);
			}
		}
	}

	public bool IsOwnedBy(Player owner)
	{
		if (player && player.Equals (owner))
		{
			return true;
		} 
		else 
		{
			return false;
		}
	}

	public Bounds GetSelectionBounds()
	{
		return selectionBounds;
	}

	public void SetColliders(bool enabled)
	{
		Collider[] colliders = GetComponentsInChildren< Collider > ();
		foreach (Collider collider in colliders) 
		{
			collider.enabled = enabled;
		}
	}

	public void SetTransparentMaterial(Material material, bool storeExistingMaterial)
	{
		if (storeExistingMaterial) 
		{
			oldMaterials.Clear();
		}
		Renderer[] renderers = GetComponentsInChildren< Renderer > ();
		foreach (Renderer renderer in renderers) 
		{
			if (storeExistingMaterial)
			{
				oldMaterials.Add(renderer.material);
			}
			renderer.material = material;
		}
	}

	public void RestoreMaterials()
	{
		Renderer[] renderers = GetComponentsInChildren< Renderer > ();
		if (oldMaterials.Count == renderers.Length) 
		{
			for (int i = 0; i < renderers.Length; i++) 
			{
				renderers[i].material = oldMaterials[i];
			}
		}
	}

	public void SetPlayingArea(Rect playingArea)
	{
		this.playingArea = playingArea;
	}

	public void SetPlayer()
	{
		player = transform.root.GetComponent/*InChildren*/< Player > ();
	}

}
