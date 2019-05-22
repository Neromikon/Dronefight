using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpeed : Effect
{
	public float frictionMultiplier = 1.0f;
	public float breakingMultiplier = 1.0f;
	public float accelerationMultiplier = 1.0f;
	public float speedMultiplier = 1.0f;

	public override void Apply(Unit target)
	{
		if(target == null){return;}

		base.Apply(target);

		target.friction *= frictionMultiplier * power;
		target.breaking *= breakingMultiplier * power;
		target.acceleration *= accelerationMultiplier * power;
		target.speed *= speedMultiplier;
	}

	//public override void Cancel(Unit target)
	//{
	//	if(target == null){return;}

	//	base.Cancel(target);

	//	target.friction /= frictionMultiplier * power;
	//	target.breaking /= breakingMultiplier * power;
	//	target.acceleration /= accelerationMultiplier * power;
	//}
}
