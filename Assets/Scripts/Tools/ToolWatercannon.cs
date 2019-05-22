using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolWatercannon: ContactTool
{
	public MissileBeam waterJet;
	public ResourceContainer waterResource;
	public ResourceContainer energyResource;
	public ResourceContainer heatResource;

	public float waterConsumption = 0.25f; //units per second
	public float energyConsumption = 0.45f; //units per second
	public float temperatureGain = 0.25f; //units per second

	private GameObject lastObject = null;

	protected override void OnStart()
	{
		waterJet.gameObject.SetActive(true);
	}

	protected override void ApplyUpdate()
	{
		if(lastObject != sensor.closestObject)
		{
			waterJet.SetTarget(sensor.closestObject);
		}
		
		waterResource.Spend(waterConsumption * Time.deltaTime);
		energyResource.Spend(energyConsumption * Time.deltaTime);
		heatResource.Spend(-temperatureGain * Time.deltaTime);
	}

	protected override void OnStop()
	{
		waterJet.gameObject.SetActive(false);
	}

	protected override bool Condition()
	{
		if (waterResource.IsEmpty()) { return false; }
		if (energyResource.IsEmpty()) { return false; }
		if (heatResource.IsMaxed()) { return false; }

		return true;
	}
}