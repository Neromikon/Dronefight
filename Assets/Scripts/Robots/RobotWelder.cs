using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotWelder: Unit
{
	public GroundSensor[] teleportDestinations;
	public ResourceContainer energyResource;

	public float teleportEnergyCost;
	public float teleportDelay = 0.1f; //seconds
	public float teleportReload = 0.5f; //seconds

	public Missile teleportationSourceEffect;
	public Missile teleportationDestinationEffect;

	private Support.Timer teleportDelayTimer;
	private Support.Timer teleportReloadTimer;
	private List<GroundSensor> candidateDestinations = new List<GroundSensor>(8);

	public override void Start()
	{
		base.Start();

		Debug.Assert(teleportDelay > 0, "Teleportation delay for " + name +" must be greater than zero for technical reasons");
		Debug.Assert(teleportationSourceEffect, "Effect for teleportation at source position is not assigned for " + name);
		Debug.Assert(teleportationSourceEffect, "Effect for teleportation at destination position is not assigned for " + name);

		foreach (GroundSensor destination in teleportDestinations)
		{
			destination.gameObject.SetActive(false);
		}
	}

	public override void Update()
	{
		base.Update();
		
		if (!teleportDelayTimer.Expired)
		{
			teleportDelayTimer.Update();

			if (teleportDelayTimer.Expired) { PerformTeleportation(); }
		}

		teleportReloadTimer.Update();
	}

	public override void JumpPress()
	{
		if (!teleportReloadTimer.Expired) { return; }
		if (!energyResource.Have(teleportEnergyCost)) { return; }

		foreach (GroundSensor destination in teleportDestinations)
		{
			destination.transform.rotation = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), transform.up);
			destination.gameObject.SetActive(true);
		}

		teleportDelayTimer.Start(teleportDelay);
	}

	private void PerformTeleportation()
	{
		candidateDestinations.Clear();

		foreach (GroundSensor destination in teleportDestinations)
		{
			destination.gameObject.SetActive(false);

			if (!destination.contact)
			{
				candidateDestinations.Add(destination);
			}
		}

		if (candidateDestinations.Count == 0) { return; }

		energyResource.Spend(teleportEnergyCost);

		GroundSensor randomDestination = candidateDestinations[Random.Range(0, candidateDestinations.Count)];

		Missile sourceEffect = Instantiate(teleportationSourceEffect, transform.position, transform.rotation);

		transform.position = randomDestination.transform.position;
		transform.rotation = randomDestination.transform.rotation;

		Missile destinationEffect = Instantiate(teleportationSourceEffect, transform.position, transform.rotation);

		teleportReloadTimer.Start(teleportReload);
	}
}