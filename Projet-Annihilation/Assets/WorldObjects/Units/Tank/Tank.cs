using UnityEngine;
using System.Collections;
using RTS;

public class Tank : Unit {

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();	
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();
	}

	public override bool CanAttack ()
	{
		return true;
	}

	protected override void UseWeapon ()
	{
		base.UseWeapon ();
		Vector3 spawnpoint = transform.position;
		spawnpoint.x += (2.1f * transform.forward.x);
		spawnpoint.y += 1.4f;
		spawnpoint.z += (2.1f * transform.forward.z);
		GameObject gameObject = (GameObject)Instantiate (RessourceManager.GetWorldObject ("LaserProjectile"), spawnpoint, transform.rotation);
		Projectile projectile = gameObject.GetComponentInChildren< Projectile > ();
		projectile.SetRange (0.9f * weaponRange);
		projectile.SetTarget (target);
	}

}
