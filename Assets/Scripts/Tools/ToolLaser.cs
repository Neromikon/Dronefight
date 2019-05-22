using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolLaser: RepeatingTool
{
	//TODO, not finished

	public MissileBeam[] possibleLaserBeams;
	public ResourceContainer energyResource;
	public ImpulseObjectSensor sensor;

	public float energyCost = 0.5f; //energy per shot

	private const float MAX_BEAM_LENGTH = 100.0f;

	private MissileBeam laserBeam;
	private bool activateOnNextFrame = false;

	private void Start()
	{
		laserBeam = possibleLaserBeams[Random.Range(0, possibleLaserBeams.Length)];
	}

	protected override void Action()
	{
		sensor.gameObject.SetActive(true);
		activateOnNextFrame = true;
	}

	protected override bool Condition()
	{
		return energyResource.Have(energyCost);
	}

	protected override void RepeatUpdate()
	{
		if (activateOnNextFrame) { ActualAction(); }
	}

	protected override void ReadyUpdate()
	{
		if (activateOnNextFrame) { ActualAction(); }
	}

	private void ActualAction()
	{		
		activateOnNextFrame = false;

		energyResource.Spend(energyCost);

		laserBeam.lineRenderer.SetPosition(0, sensor.transform.position);

		if (sensor.closestObject)
		{
			laserBeam.lineRenderer.SetPosition(1, sensor.closestObject.transform.position);
		}
		else
		{
			laserBeam.lineRenderer.SetPosition(1, sensor.transform.position + sensor.transform.forward * MAX_BEAM_LENGTH);
		}

		laserBeam.gameObject.SetActive(true);
	}
}