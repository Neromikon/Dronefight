using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBlaster: ChargingTool
{
	public MissileFlexible plasmaMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer plasmaResource;
	public ResourceContainer energyResource;
	public ResourceContainer powerResource;

	public float minPower = 10.0f; //minimum power needed
	public float powerGain = 25.0f; //units per second
	public float overflowDamage = 2.0f; //damage per second

	public float minPlasmaCost = 0.5f;
	public float maxPlasmaCost = 2.5f;

	public float chargingEnergyConsumption = 0.5f; //units per second
	public float minEnergyCost = 0.25f;
	public float maxEnergyCost = 1.0f;

	protected override void Action()
	{
		if (powerResource.amount >= minPower)
		{
			float percentage = powerResource.percentage;

			MissileFlexible newMissile = Instantiate(plasmaMissilePrefab, missileSpawner.position, missileSpawner.rotation);
			newMissile.SetOwner(owner);
			newMissile.SetPower(percentage);

			plasmaResource.Spend(minPlasmaCost + (maxPlasmaCost - minPlasmaCost) * percentage);
			energyResource.Spend(minEnergyCost + (maxEnergyCost - minEnergyCost) * percentage);
		}

		powerResource.Spend();
	}

	protected override bool ChargeCondition()
	{
		if (!plasmaResource.Have(minPlasmaCost)) { return false; }
		if (!energyResource.Have(minEnergyCost)) { return false; }

		return true;
	}

	protected override void ChargeUpdate()
	{
		if (powerResource.IsMaxed())
		{
			ReceiveDamage(overflowDamage * Time.deltaTime, DamageType.PLASMA);
		}
		else
		{
			powerResource.Spend(-powerGain * Time.deltaTime);
		}

		energyResource.Spend(chargingEnergyConsumption * Time.deltaTime);
	}	
}