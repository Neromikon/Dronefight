using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSpitter : ClickingTool
{
	public Missile spitMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer acidResource;

	public float acidCost = 1.5f;

	protected override void Action()
	{
		Missile newMissile = Instantiate(spitMissilePrefab, missileSpawner.position, missileSpawner.rotation);
		newMissile.SetOwner(owner);

		acidResource.Spend(acidCost);
	}

	protected override bool Condition()
	{
		if (!acidResource.Have(acidCost)) { return false; }

		return true;
	}
}
