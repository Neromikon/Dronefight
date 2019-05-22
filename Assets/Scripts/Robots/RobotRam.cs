using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRam: Unit
{
	public float minJumpPower;
	public float maxJumpPower;
	public float jumpPowerGain; //units per second

	private float jumpPower;

	public override void Start()
	{
		base.Start();

		Debug.Assert(maxJumpPower > minJumpPower, "Minimum required jump power can't be higher than maximum possible");
		Debug.Assert(maxJumpPower > 0, "Maximum possible jump power must be higher than zero");
		Debug.Assert(jumpPowerGain > 0, "Jump power gain must be higher that zero units per second");
	}

	public override void Update()
	{
		base.Update();

		//if(tool2 != null)
		//{
		//	resource2 = tool2.power;
		//}
	}

	public override void JumpHold()
	{
		jumpBreaksActive = true;

		if (jumpPower < maxJumpPower)
		{
			jumpPower += Time.deltaTime * jumpPowerGain;

			if (jumpPower > maxJumpPower) { jumpPower = maxJumpPower; }
		}
	}

	public override void JumpRelease()
	{
		if (bottomGroundSensor.contact)
		{
			body.AddForce((Vector3.up + move_direction).normalized * jumpPower);
		}

		jumpBreaksActive = false;
		jumpPower = minJumpPower;
	}
}