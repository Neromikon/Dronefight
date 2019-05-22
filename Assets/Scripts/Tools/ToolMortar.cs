using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMortar: ChargingTool
{
	//todo: consider spending some uncontable resource for charging as well (not sure if energy is actual for this robot)

	public Missile grenadeMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer grenadesResource;
	public ResourceContainer sprungResource;
	
	public float sprungGain = 80.0f;

	public float minimumSpeedPercentage = 0.1f;

	protected override void Action()
	{
		Missile newMissile = Instantiate(grenadeMissilePrefab, missileSpawner.position, missileSpawner.rotation);
		newMissile.SetOwner(owner);
		newMissile.speed *= minimumSpeedPercentage + sprungResource.percentage * (1.0f - minimumSpeedPercentage);

		grenadesResource.Spend(1);

		sprungResource.Spend();
	}

	protected override bool ChargeCondition()
	{
		return !grenadesResource.IsEmpty();
	}

	protected override void ChargeUpdate()
	{
		if (!sprungResource.IsMaxed())
		{
			sprungResource.Spend(-sprungGain * Time.deltaTime);
		}
	}
}