using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileGhost : Missile
{
	protected override void OnTriggerEnter(Collider other)
	{
		GameObject target = other.gameObject;

		if(target.layer == GameLayer.Units)
		{
			Unit targetUnit = other.GetComponent<Unit>();
			if(targetUnit != null && owner != targetUnit)
			{
				targetUnit.DamageInside(damage, damageType);
			}
		}
	}
}
