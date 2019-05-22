using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolWelding: ContactTool
{
	public MissileBeam ligthningMissile;
	public ResourceContainer energyResource;
	
	public float energyConsumption = 0.65f; //units per second

	private GameObject lastObject = null;

	protected override void OnStart()
	{
		ligthningMissile.gameObject.SetActive(true);
	}

	protected override void ApplyUpdate()
	{
		if (lastObject != sensor.closestObject)
		{
			ligthningMissile.SetTarget(sensor.closestObject);
		}
		
		energyResource.Spend(energyConsumption * Time.deltaTime);
	}

	protected override void OnStop()
	{
		ligthningMissile.gameObject.SetActive(false);
	}

	protected override bool Condition()
	{
		return !energyResource.IsEmpty();
	}
}