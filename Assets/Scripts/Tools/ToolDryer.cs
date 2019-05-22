using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDryer: RepeatingTool
{
	public Missile hotAirMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer energyResource;
	public ResourceContainer heatResource;
	
	public float energyConsumption = 0.45f;
	public float minHeatConsumption = 2.65f; //units per second
	public float maxHeatConsumption = 6.25f; //units per second
	public float minimumHeat = 15.0f;

	public float baseDamageCoefficient = 0.25f;

	protected override void Action()
	{
		Missile newMissile = Instantiate(hotAirMissilePrefab, missileSpawner.position, missileSpawner.rotation);
		newMissile.SetOwner(owner);
		newMissile.damage *= baseDamageCoefficient + (1.0f - baseDamageCoefficient) * heatResource.percentage;
	}

	protected override bool Condition()
	{
		if (!heatResource.Have(minimumHeat)) { return false; }
		if (energyResource.IsEmpty()) { return false; }

		return true;
	}

	protected override void ActiveUpdate()
	{
		float heatConsumption = minHeatConsumption + (maxHeatConsumption - minHeatConsumption) * heatResource.percentage;

		heatResource.Spend(heatConsumption * Time.deltaTime);
		energyResource.Spend(energyConsumption * Time.deltaTime);
	}
}