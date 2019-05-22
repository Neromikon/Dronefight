using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotZeppelin: Unit
{
	public float flightPower;

	public float flightEnergyConsumption;

	public override void Start()
	{
		base.Start();
	}

	public override void JumpHold()
	{
		if (motionResource.IsEmpty()) { return; }

		motionResource.Spend(flightEnergyConsumption * Time.deltaTime);

		body.AddForce(Physics.gravity.normalized * (-flightPower));
	}
}