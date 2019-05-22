using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMelee : Missile
{
	public uint maxHits = 1;

	private uint hits = 0;

	protected override void Start()
	{
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		lifeTimer.Start(timeToLive);
	}

	private void Hit()
	{
		if(explosion != null)
		{
			GameObject explosionMissile = Instantiate(explosion);
			explosionMissile.transform.position = transform.position;
		}

		hits++;

		if (hits >= maxHits)
		{
			gameObject.SetActive(false);
		}
	}

	protected override void Update()
	{
		if (lifeTimer.Expired)
		{
			gameObject.SetActive(false);
		}
	}

	protected override void OnTriggerEnter(Collider other)
	{
		switch(other.gameObject.layer)
		{
			case GameLayer.Units:
			{
				Unit targetUnit = other.GetComponent<Unit>();
				if(!targetUnit) { return; }
				if(targetUnit == owner){ return; }

				//targetUnit.ReceiveDamage(damage, damageType);
				targetUnit.DamageSide(transform.position, damage, damageType);
				Hit();
				Debug.Log("Missile " + name + " hits " + other.name);
				return;
			}

			case GameLayer.Destructibles:
			{
				Destructible targetDestructible = other.GetComponent<Destructible>();
				if(!targetDestructible) { return; }

				targetDestructible.ReceiveDamage(damage, damageType);
				Hit();
				return;
			}
		}
	}
}
