using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMinigun : RepeatingTool
{
	public Missile bulletMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer bulletsResource;

	public float minRepeatPeriod = 0.15f;
	public float maxRepeatPeriod = 0.35f;
	public float repeatSpeedGain = 0.1f; //seconds per second
	public float repeatSpeedLoss = 0.075f; //seconds per second

	protected override void Action()
	{
		Missile newMissile = Instantiate(bulletMissilePrefab, missileSpawner.position, missileSpawner.rotation);

		newMissile.SetOwner(owner);

		bulletsResource.Spend(1);
	}

	protected override bool Condition()
	{
		return !bulletsResource.IsEmpty();
	}

	protected override void RepeatUpdate()
	{
		if (repeatDelay > minRepeatPeriod)
		{
			repeatDelay -= repeatSpeedGain * Time.deltaTime;
			if (repeatDelay < minRepeatPeriod) { repeatDelay = minRepeatPeriod; }
		}
	}

	protected override void ReloadUpdate()
	{
		if (repeatDelay < maxRepeatPeriod)
		{
			repeatDelay += repeatSpeedLoss * Time.deltaTime;
			if (repeatDelay > maxRepeatPeriod) { repeatDelay = maxRepeatPeriod; }
		}
	}

	protected override void ReadyUpdate()
	{
		if (repeatDelay < maxRepeatPeriod)
		{
			repeatDelay += repeatSpeedLoss * Time.deltaTime;
			if (repeatDelay > maxRepeatPeriod) { repeatDelay = maxRepeatPeriod; }
		}
	}
}