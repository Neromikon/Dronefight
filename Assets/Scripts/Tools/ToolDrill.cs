using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDrill: RepeatingTool
{
	private MissileMelee drillMissile;
	public ResourceContainer energyResource;

	public float energyConsumption = 0.2f; //units per second

	protected override void Action()
	{
		drillMissile.gameObject.SetActive(true);
	}

	protected override bool Condition()
	{
		return !energyResource.IsEmpty();
	}

	protected override void RepeatUpdate()
	{
		energyResource.Spend(energyConsumption * Time.deltaTime);
	}
}