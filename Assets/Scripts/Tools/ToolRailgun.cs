using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolRailgun: ChargingTool
{
	//todo: return non-spent energy if minimum power was not reached

	public Missile chargedMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer bulletsResource;
	public ResourceContainer energyResource;
	public ResourceContainer chargeResource;

	public float minimumCharge = 10.0f; //minimum power needed
	public float chargeGain = 25.0f; //units per second
	public float overflowDamage = 2.0f; //damage per second

	public float energyConsumption = 0.7f; //units per second

	protected override void Action()
	{
		if (chargeResource.amount >= minimumCharge)
		{
			Missile newMissile = Instantiate(chargedMissilePrefab, missileSpawner.position, missileSpawner.rotation);
			newMissile.SetOwner(owner);
			//newMissile.SetPower(chargeResource.percentage);

			bulletsResource.Spend(1);
		}

		chargeResource.Spend();
	}

	protected override bool ChargeCondition()
	{
		return !energyResource.IsEmpty() && !bulletsResource.IsEmpty();
	}

	protected override void ChargeUpdate()
	{
		chargeResource.Spend(-chargeGain * Time.deltaTime);
		energyResource.Spend(energyConsumption * Time.deltaTime);
	}
}