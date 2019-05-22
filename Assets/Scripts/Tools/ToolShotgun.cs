using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolShotgun: RepeatingTool
{
	public Missile bulletsMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer bulletsResource;

	protected override void Action()
	{
		Missile newMissile = Instantiate(bulletsMissilePrefab, missileSpawner.position, missileSpawner.rotation);
		newMissile.SetOwner(owner);

		bulletsResource.Spend(1);
	}

	protected override bool Condition()
	{
		return !bulletsResource.IsEmpty();
	}
}