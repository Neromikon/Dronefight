using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolRadiator: RepeatingTool
{
	public Missile radiowaveMissile;
	public Transform missileSpawner;
	public ResourceContainer energyResource;

	public float energyConsumption;

	protected override void Action()
	{
		Missile newMissile = Instantiate(radiowaveMissile, missileSpawner.position, missileSpawner.rotation);
		newMissile.SetOwner(owner);
	}

	protected override bool Condition()
	{
		return !energyResource.IsEmpty();
	}
}