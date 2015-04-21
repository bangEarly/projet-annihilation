using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class WorldObject : MonoBehaviour {

	public string objectName;
	public Texture2D buildImage;
	public int /*cost, sellValue,*/ hitPoints, maxHitPoints, costCrystalite, costDilithium, costPower;

	protected Player player;
	protected string[] actions = {};
	public bool currentlySelected = false;

	public Bounds selectionBounds;
	protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

	private List< Material > oldMaterials = new List<Material> ();

	protected GUIStyle healthStyle = new GUIStyle();
	protected float healthPercentage = 1.0f;

	protected WorldObject target = null;
	protected bool attacking = false;

	public float weaponRange = 10f;
	protected bool movingIntoPosition = false;
	protected bool aiming = false;
	private Quaternion aimRotation;
	public float weaponChargeTime = 1.0f;
	private float currentWeaponChargeTime = 0.0f;
	public float weaponAimSpeed = 10.0f;
	public bool isAttacked = false;
	public float onAttackTimer = 20.0f;

	protected virtual void Awake()
	{
		selectionBounds = RessourceManager.InvalidBounds;
		CalculateBounds ();
	}

	// Use this for initialization
	protected virtual void Start () 
	{
		SetPlayer ();
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		if (aiming) 
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, aimRotation, weaponAimSpeed);
			CalculateBounds ();
			Quaternion  inverseAimRotation = new Quaternion (-aimRotation.x, -aimRotation.y, -aimRotation.z, -aimRotation.w);
			if (transform.rotation == aimRotation || transform.rotation == inverseAimRotation)
			{
				aiming = false;
			}
		}
		if (attacking)
		{
			currentWeaponChargeTime += Time.deltaTime;
			if (!movingIntoPosition && !aiming)
			{
				PerformAttack();
			}
		}
	}

	protected virtual void OnGUI()
	{
		if (currentlySelected) 
		{
			DrawSelection ();
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
		if (currentlySelected && hitObject && hitObject.name != "Ground") 
		{
			WorldObject worldObject = hitObject.transform.parent.GetComponent< WorldObject> ();
			if (worldObject && worldObject.name != "Ground") 
			{
				Player owner = hitObject.transform.root.GetComponent< Player > ();
				if (owner) 
				{
					if (player && player.human) 
					{
						if (player.teamColor != owner.teamColor && CanAttack ()) 
						{
							BeginAttack (worldObject);
						}
					}
				}
			}
		}

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

	protected void DrawSelection() //dessine le cercle de selection autour de l'objet selectionne
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
		CalculateCurrentHealth ();
		GUI.Label (new Rect (selectBox.x, selectBox.y - 9, selectBox.width * healthPercentage, 5), "", healthStyle);
	}

	public virtual void SetHoverState(GameObject hoverObject)
	{
		if (player && player.human && currentlySelected) 
		{
			if (hoverObject.name != "Ground")
			{
				Player owner = hoverObject.transform.root.GetComponent< Player > ();
				Unit unit = hoverObject.transform.parent.GetComponent< Unit > ();
				Building building = hoverObject.transform.parent.GetComponent < Building > ();
				if (owner)
				{
					if (owner.username != player.username && CanAttack())
					{
						player.hud.SetCursorState(CursorState.Attack);
					}
					else 
					{
						player.hud.SetCursorState(CursorState.Select);
					}

				}
				else 
				{
					player.hud.SetCursorState(CursorState.Select);
				}

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

	protected virtual void CalculateCurrentHealth()
	{
		healthPercentage = (float)hitPoints / (float)maxHitPoints;
		if (healthPercentage > 0.65f) 
		{
			healthStyle.normal.background = RessourceManager.HealthyTexture;
		} 
		else if (healthPercentage > 0.35f) 
		{
			healthStyle.normal.background = RessourceManager.DamagedTexture;
		} 
		else 
		{
			healthStyle.normal.background = RessourceManager.CriticalTexture;
		}
		     
	}

	public virtual bool CanAttack()
	{
		return false;
	}

	protected virtual void BeginAttack(WorldObject target)
	{
		this.target = target;
		Debug.Log (TargetInRange());
		if (TargetInRange ()) 
		{
			attacking = true;
			PerformAttack ();
		} 
		else 
		{
			AdjustPosition();
		}
		NavMeshAgent agent = transform.GetComponent< NavMeshAgent > (); //si l'unite etait en train de se deplacer, elle s'arrete
		if (agent) 
		{
			agent.Stop();
		}
	}

	private bool TargetInRange()
	{
		Vector3 targetLocation = target.transform.position;
		Vector3 direction = targetLocation - transform.position;

		return (direction.sqrMagnitude < weaponRange * weaponRange);
	}

	private void AdjustPosition()
	{
		Unit self = this as Unit;
		if (self) 
		{
			movingIntoPosition = true;
			Vector3 attackPosition = FindNearestAttackPosition ();
			self.StartMove (attackPosition);
			attacking = true;
		} 
		else 
		{
			attacking = false;
		}
	}

	private Vector3 FindNearestAttackPosition()
	{
		Vector3 targetLocation = target.transform.position;
		Vector3 direction = targetLocation - transform.position;
		float targetDistance = direction.magnitude;
		float distanceToTravel = targetDistance - (0.9f * weaponRange);
		return Vector3.Lerp (transform.position, targetLocation, distanceToTravel / targetDistance);
	}

	private void PerformAttack()
	{
		if (target) 
		{
			if (!TargetInRange ()) 
			{
				AdjustPosition ();
			} 
			else if (!TargetInFrontOfWeapon ()) 
			{
				AimAtTarget();
			} 
			else if (ReadyToFire ()) 
			{
				UseWeapon ();
			}
		} 
		else 
		{
			attacking = false;
		}
	}

	private bool TargetInFrontOfWeapon()
	{
		Vector3 targetLocation = target.transform.position;
		Vector3 direction = targetLocation - transform.position;

		return (direction.normalized == transform.forward.normalized);
	}

	private void AimAtTarget()
	{
		aiming = true;
		aimRotation = Quaternion.LookRotation (target.transform.position - transform.position);
	}

	private bool ReadyToFire()
	{
		return currentWeaponChargeTime >= weaponChargeTime;
	}

	protected virtual void UseWeapon()
	{
		currentWeaponChargeTime = 0.0f;
	}

	public void TakeDamage(int damage)
	{
		hitPoints -= damage;
		if (hitPoints <= 0)
		{
			Destroy(gameObject);
		}
		onAttackTimer = 20.0f;
	}

}
