using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBurner: RepeatingTool
{
	public Missile flameMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer napalmResource;

	public float napalmConsumption = 1.0f; //units per second

	public float minFlameSpeed = 0.25f; //metric units per second
	public float maxFlameSpeed = 4.0f; //metric units per second
	public float flameSpeedGain = 1.1f; //(metric units per second) per second
	public float flameSpeedLoss = 1.5f; //(metric units per second) per second

	private float currentFlameSpeed;

	private void Start()
	{
		currentFlameSpeed = minFlameSpeed;
	}

	protected override void Action()
	{
		Missile newMissile = Instantiate(flameMissilePrefab, missileSpawner.position, missileSpawner.rotation);
		newMissile.SetOwner(owner);
		newMissile.speed = currentFlameSpeed;
	}

	protected override bool Condition()
	{
		return !napalmResource.IsEmpty();
	}

	protected override void DelayUpdate()
	{
		napalmResource.Spend(napalmConsumption * Time.deltaTime);
	}

	protected override void RepeatUpdate()
	{
		napalmResource.Spend(napalmConsumption * Time.deltaTime);

		if (currentFlameSpeed < maxFlameSpeed)
		{
			currentFlameSpeed += flameSpeedGain * Time.deltaTime;
			if (currentFlameSpeed > maxFlameSpeed) { currentFlameSpeed = maxFlameSpeed; }
		}
	}

	protected override void ReadyUpdate()
	{
		if (currentFlameSpeed > minFlameSpeed)
		{
			currentFlameSpeed -= flameSpeedLoss * Time.deltaTime;
			if (currentFlameSpeed < minFlameSpeed) { currentFlameSpeed = minFlameSpeed; }
		}
	}
}