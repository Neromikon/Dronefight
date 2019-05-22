using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileField : Missile
{
	public float distanceDependency = 0.0f; //[0..1]
	public float range = 1.0f;

	protected override void Start()
	{
		base.Start();
		
		gameObject.SetActive(false);
	}

	protected override void Update()
	{
		if(owner) { transform.rotation = owner.transform.rotation; }
	}

	protected override void OnTriggerEnter(Collider other)
	{
		//do nothing
	}

	private void OnTriggerStay(Collider other)
	{
		float distance = Vector3.Distance(transform.position, other.transform.position);
		float distanceCoefficient = 1.0f - distanceDependency * distance / range;

		switch (other.gameObject.layer)
		{
			case GameLayer.Units:
			{
				if (other.gameObject == owner.gameObject) { break; }
				Unit targetUnit = other.GetComponent<Unit>();
				targetUnit.ReceiveDamage(damage * distanceCoefficient * Time.deltaTime, damageType);
				break;
			}

			case GameLayer.Items:
			{
				Item targetItem = other.GetComponent<Item>();
				targetItem.ReceiveDamage(damage * distanceCoefficient * Time.deltaTime, damageType);
				break;
			}

			case GameLayer.Destructibles:
			{
				Destructible targetDestructible = other.GetComponent<Destructible>();
				targetDestructible.ReceiveDamage(damage * distanceCoefficient * Time.deltaTime, damageType);
				break;
			}
		}
	}
}
