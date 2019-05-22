using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolFlamer: RepeatingTool
{
	public Missile flameMissilePrefab;
	public Transform missileSpawner1, missileSpawner2;
	public ResourceContainer gasResource;

	public float gasConsumption = 1.15f; //units per second

	protected override void Action()
	{
		Missile newMissile1 = Instantiate(flameMissilePrefab, missileSpawner1.position, missileSpawner1.rotation);
		newMissile1.SetOwner(owner);

		Missile newMissile2 = Instantiate(flameMissilePrefab, missileSpawner2.position, missileSpawner2.rotation);
		newMissile2.SetOwner(owner);
	}

	protected override bool Condition()
	{
		return !gasResource.IsEmpty();
	}

	protected override void ActiveUpdate()
	{
		gasResource.Spend(gasConsumption * Time.deltaTime);
	}
}