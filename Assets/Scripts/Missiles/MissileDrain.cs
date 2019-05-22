using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDrain : Missile
{
	public bool singleHit = true;

	protected override void Start()
	{
		body = GetComponent<Rigidbody>();

		{
			Transform find = transform.parent;

			while(find != null && owner == null)
			{
				owner = find.GetComponent<Unit>();
				find = find.parent;
			}

			if(owner == null)
			{
				Debug.Log("Failed to link unit to drain missile");
			}
		}

		particles = GetComponentInChildren<ParticleSystem>();
		if(particles == null)
		{
			Debug.Log("Missile " + name + " don't have particle system");
		}
	}

	protected override void Update()
	{

	}

	protected void OnTriggerStay(Collider other)
	{
		Unit targetUnit = other.GetComponent<Unit>();
		if(targetUnit != null && targetUnit != owner)
		{
			targetUnit.ReceiveDamage(damage, damageType);

			if(explosion != null)
			{
				GameObject explosionMissile = Instantiate(explosion);
				explosionMissile.transform.position = transform.position;
			}

			if(singleHit && body != null)
			{
				body.detectCollisions = false;
			}
		}
	}
}
