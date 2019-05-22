using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSpinner : ClickingTool
{
	public MissileWeb webMissilePrefab;
	public Transform missileSpawner1, missileSpawner2;
	public ResourceContainer webResource;
	public ResourceContainer energyResource;

	public float webCost = 3.25f;
	public float energyCost = 0.35f;

	protected override void Action()
	{
		MissileWeb newMissile1 = Instantiate(webMissilePrefab, missileSpawner1.position, missileSpawner1.rotation);
		newMissile1.SetOwner(owner);

		MissileWeb newMissile2 = Instantiate(webMissilePrefab, missileSpawner2.position, missileSpawner2.rotation);
		newMissile2.SetOwner(owner);

		webResource.Spend(webCost);
		energyResource.Spend(energyCost);
	}

	protected override bool Condition()
	{
		if (!webResource.Have(webCost)) { return false; }
		if (!energyResource.Have(energyCost)) { return false; }

		return true;
	}
}
