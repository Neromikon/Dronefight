using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBurn : Effect
{
	public float damagePerSecond = 1.0f;
	public DamageType damageType = DamageType.NONE;

	public override void Update()
	{
		base.Update();

		if(target != null)
		{
			target.DamageAllSides(damagePerSecond * Time.deltaTime, damageType);
		}
	}
}
