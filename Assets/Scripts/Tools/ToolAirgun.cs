using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAirgun: ChargingTool
{
	public Missile arrowMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer pressureResource;
	public ResourceContainer energyResource;
	public ResourceContainer arrowsResource;

	public float minimumPressure = 20.0f;
	public float minPressureGain = 5.0f; //units per second
	public float maxPressureGain = 25.0f; //units per second
	public float minEnergyConsumption = 0.5f; //units per second
	public float maxEnergyConsumption = 2.5f; //units per second
	public float shootEnergyCost = 0.01f;
	public float baseSpeedCoefficient = 0.5f; //[0..1]

	protected override void Action()
	{
		Missile newMissile = Instantiate(arrowMissilePrefab, missileSpawner.position, missileSpawner.rotation);
		newMissile.SetOwner(owner);
		newMissile.speed *= baseSpeedCoefficient + (1.0f - baseSpeedCoefficient) * pressureResource.percentage;

		arrowsResource.Spend(1);
		pressureResource.Spend();
		energyResource.Spend(shootEnergyCost);

		//unit.Play("SpiderCrossbowShoot");
	}

	protected override bool ChargeCondition()
	{
		if (pressureResource.IsMaxed()) { return false; }
		if (energyResource.IsEmpty()) { return false; }

		return true;
	}

	protected override bool ActionCondition()
	{
		if (arrowsResource.IsEmpty()) { return false; }
		if (!pressureResource.Have(minimumPressure)) { return false; }
		if (!energyResource.Have(shootEnergyCost)) { return false; }

		return true;
	}

	protected override void ChargeUpdate()
	{
		float percentage = pressureResource.percentage;
		float gain = maxPressureGain - (maxPressureGain - minPressureGain) * percentage;
		float cost = minEnergyConsumption + (maxEnergyConsumption - minEnergyConsumption) * percentage;

		pressureResource.Spend(-gain * Time.deltaTime);
		energyResource.Spend(cost * Time.deltaTime);
	}
}