using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDynamite : ChargingTool
{
	public Item dynamiteItemPrefab;
	public Transform dynamiteSpawner;
	public ResourceContainer dynamiteResource;
	public ResourceContainer timeResource;

	public float explosionDelayGain = 101.0f; //units per second
	public float minExplosionDelay = 1.0f; //seconds
	public float maxExplosionDelay = 6.0f; //seconds
	public float throwForce = 20.0f;

	protected override void Action()
	{
		float explosionCountdown = minExplosionDelay + (maxExplosionDelay - minExplosionDelay) * timeResource.percentage;

		Item spawnedDynamite = Instantiate(dynamiteItemPrefab, dynamiteSpawner.position, dynamiteSpawner.rotation);
		spawnedDynamite.Explode(explosionCountdown);
		//spawnedDynamite.body.AddForce(dynamiteSpawner.forward * throwForce);

		timeResource.Spend(timeResource.maximum);
		dynamiteResource.Spend(1);
	}

	protected override bool ChargeCondition()
	{
		return !dynamiteResource.IsEmpty();
	}

	protected override void ChargeUpdate()
	{
		timeResource.Spend(-explosionDelayGain * Time.deltaTime);
	}
}
