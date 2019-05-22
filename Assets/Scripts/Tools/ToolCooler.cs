using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCooler: RepeatingTool
{
	public Missile coldAirMissilePrefab;
	public ResourceContainer heatResource;
	public ResourceContainer energyResource;
	public ResourceContainer waterResource;
	public Transform missileSpawner1, missileSpawner2;

	public float temperatureGain = 1.25f; //units per second
	public float energyConsumption = 0.5f; //units per second
	public float waterConsumption = 0.5f; //units per second

	protected override void Action()
	{
		Missile newMissile1 = Instantiate(coldAirMissilePrefab);
		newMissile1.transform.position = missileSpawner1.position;
		newMissile1.transform.rotation = missileSpawner1.rotation;
		newMissile1.SetOwner(owner);

		Missile newMissile2 = Instantiate(coldAirMissilePrefab);
		newMissile2.transform.position = missileSpawner2.position;
		newMissile2.transform.rotation = missileSpawner2.rotation;
		newMissile2.SetOwner(owner);
	}

	protected override bool Condition()
	{
		if (heatResource.IsMaxed()) { return false; }
		if (energyResource.IsEmpty()) { return false; }
		if (waterResource.IsEmpty()) { return false; }

		return true;
	}

	protected override void ActiveUpdate()
	{
		energyResource.Spend(energyConsumption * Time.deltaTime);
		waterResource.Spend(waterConsumption * Time.deltaTime);
		heatResource.Spend(-temperatureGain * Time.deltaTime);
	}
}