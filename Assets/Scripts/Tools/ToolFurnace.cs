using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolFurnace: SwitchingTool
{
	//TODO: spend more resouces if furnace is active while dryer tool is active (do not combine)

	public ResourceContainer heatResource;
	public ResourceContainer fuelResource;

	public float temperatureGain = 0.5f; //units per second
	public float fuelConsumption = 1.05f; //units per second

	//private Effect appliedEffect;

	protected override void ActiveUpdate()
	{
		heatResource.Spend(-temperatureGain * Time.deltaTime);
		fuelResource.Spend(fuelConsumption * Time.deltaTime);
	}

	protected override bool ActivationCondition()
	{
		if (fuelResource.IsEmpty()) { return false; }
		if (heatResource.IsMaxed()) { return false; }

		return true;
	}
}