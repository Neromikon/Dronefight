using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBulldozer: Unit
{
	public ToolBucket bucket;
	public ToolDynamite dynamite;

	public float jumpPower;

	public string throwAnimation;
	public float throwDuration; //seconds
	public float throwReload; //seconds

	private Support.Timer throwReloadTimer;

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();

		throwReloadTimer.Update();
	}

	public override void JumpPress()
	{
		//todo
	}

	public override void JumpRelease()
	{
		if (!bottomGroundSensor.contact) { return; }

		body.AddForce(transform.up * jumpPower);
		//body.AddForceAtPosition(-Physics.gravity * jumpPower, transform.position);
	}

	public override void PickDoubleClick()
	{
		//todo item presence check and actual item throwing

		if (!throwReloadTimer.Expired) { return; }

		PlayTransition(throwAnimation, throwDuration);
		
		throwReloadTimer.Start(throwReload);
	}
}