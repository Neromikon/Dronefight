using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolIonbeam: ContactTool
{
	public MissileBeam beamMissile1, beamMissile2;
	public ResourceContainer plasmaResource;
	public ResourceContainer energyResource;

	public float plasmaConsumption = 0.55f; //units per second
	public float energyConsumption = 0.65f; //units per second

	private GameObject lastObject = null;

	protected override void OnStart()
	{
		beamMissile1.gameObject.SetActive(true);
		beamMissile2.gameObject.SetActive(true);
	}

	protected override void ApplyUpdate()
	{
		if (lastObject != sensor.closestObject)
		{
			beamMissile1.SetTarget(sensor.closestObject);
			beamMissile2.SetTarget(sensor.closestObject);
		}

		plasmaResource.Spend(plasmaConsumption * Time.deltaTime);
		energyResource.Spend(energyConsumption * Time.deltaTime);
	}

	protected override void OnStop()
	{
		beamMissile1.gameObject.SetActive(false);
		beamMissile2.gameObject.SetActive(false);
	}

	protected override bool Condition()
	{
		if (plasmaResource.IsEmpty()) { return false; }
		if (energyResource.IsEmpty()) { return false; }

		return true;
	}
}