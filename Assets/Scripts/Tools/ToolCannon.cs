using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCannon: ClickingTool
{
	public Missile cannonballMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer cannonballsResource;
	//public ResourceContainer powderResource;

	protected override void Action()
	{
		Missile newMissile = Instantiate(cannonballMissilePrefab, missileSpawner.position, missileSpawner.rotation);
		newMissile.SetOwner(owner);

		cannonballsResource.Spend(1);
	}

	protected override bool Condition()
	{
		return !cannonballsResource.IsEmpty();
	}
}