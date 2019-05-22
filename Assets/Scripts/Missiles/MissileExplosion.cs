using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : Missile
{
	public float repellPower = 1.0f;

	protected override void OnTriggerEnter(Collider other)
	{
		Vector3 repell = (other.transform.position - transform.position).normalized * repellPower;

		if(other.gameObject.layer == GameLayer.Units)
		{
			Unit targetUnit = other.gameObject.GetComponent<Unit>();
			if(targetUnit != null && targetUnit != owner)
			{
				targetUnit.body.velocity += repell;
				return;
			}
		}

		if(other.gameObject.layer == GameLayer.Items)
		{
			Item targetItem = other.gameObject.GetComponent<Item>();
			if(targetItem != null)
			{
				targetItem.body.velocity += repell;
				return;
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		OnTriggerEnter(other);
	}
}
