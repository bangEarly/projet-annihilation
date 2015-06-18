using UnityEngine;
using System.Collections;
using RTS;

public class Projectile : MonoBehaviour {

	public float velocity = 1;
	public int damage = 1;

	private float range = 1;
	private WorldObject target;

	// Update is called once per frame
	void Update () 
	{
		if (HitSomething ()) 
		{
			if (!RessourceManager.networkIsConnected() || transform.GetComponent<NetworkView>().isMine)
			{
				InflictDamage();
			}
			Destroy(gameObject);
		}
		if (range > 0) 
		{
			float positionChange = Time.deltaTime * velocity;
			range -= positionChange;
			transform.position += (positionChange * transform.forward);
		} 
		else 
		{
			Destroy(gameObject);
		}
	}

	public void SetRange(float range)
	{
		this.range = range;
	}

	public void SetTarget(WorldObject target)
	{
		this.target = target;
	}

	private bool HitSomething()
	{
		return (target && target.GetSelectionBounds ().Contains (transform.position));
	}

	private void InflictDamage ()
	{
		if (target) 
		{
			target.TakeDamage(damage);
		}
	}

	[RPC] void initializeProjectile(float range)
	{
		this.range = range;
	}

}
