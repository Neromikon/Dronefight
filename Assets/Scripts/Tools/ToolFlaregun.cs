using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolFlaregun: ClickingTool
{
	public Missile flareMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer flaresResource;

	protected override void Action()
	{
		Missile newMissile = Instantiate(flareMissilePrefab, missileSpawner.position, missileSpawner.rotation);
		newMissile.SetOwner(owner);

		flaresResource.Spend(1);
	}

	protected override bool Condition()
	{
		return !flaresResource.IsEmpty();
	}
}