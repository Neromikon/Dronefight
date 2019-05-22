using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCompresser: ClickingTool
{
	public Missile hotVaporMissilePrefab1;
	public Missile hotVaporMissilePrefab2;
	public Missile hotVaporMissilePrefab3;
	public ResourceContainer heatResource;
	public ResourceContainer energyResource;
	public ResourceContainer waterResource;
	public Transform missileSpawner1, missileSpawner2;

	public float waterConsumption = 2.0f;
	public float energyConsumption = 1.0f;

	protected override void Action()
	{
		Missile missile = null;

		if (heatResource.amount >= 25.0f)
		{
			if (heatResource.amount < 50.0f)
			{
				missile = hotVaporMissilePrefab1;
			}
			else if (heatResource.amount < 75.0f)
			{
				missile = hotVaporMissilePrefab2;
			}
			else
			{
				missile = hotVaporMissilePrefab3;
			}

			Missile newMissile1 = Instantiate(missile);
			newMissile1.transform.position = missileSpawner1.position;
			newMissile1.transform.rotation = missileSpawner1.rotation;
			newMissile1.SetOwner(owner);

			Missile newMissile2 = Instantiate(missile);
			newMissile2.transform.position = missileSpawner2.position;
			newMissile2.transform.rotation = missileSpawner2.rotation;
			newMissile2.SetOwner(owner);
		}

		heatResource.Spend();
		waterResource.Spend(waterConsumption);
		energyResource.Spend(energyConsumption);
	}

	protected override bool Condition()
	{
		if (!waterResource.Have(waterConsumption)) { return false; }
		if (!energyResource.Have(energyConsumption)) { return false; }

		return true;
	}
}