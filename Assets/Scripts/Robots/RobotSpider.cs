using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpider: Unit
{
	public DamageReceiver legsDamageReceiver;
	public DamageReceiver tool1DamageReceiver;
	public DamageReceiver tool2DamageReceiver;
	public DamageReceiver tool3DamageReceiver;

	public float minJumpPower;
	public float maxJumpPower;
	public float jumpPowerGain; //units per second

	private float jumpPower;

	public override void Start()
	{
		base.Start();

		if (legsDamageReceiver) { legsDamageReceiver.onDeath.AddListener(OnLegsDestroyed); }
		if (tool1DamageReceiver) { tool1DamageReceiver.onDeath.AddListener(OnTool1Destroyed); }
		if (tool2DamageReceiver) { tool2DamageReceiver.onDeath.AddListener(OnTool2Destroyed); }
		if (tool3DamageReceiver) { tool3DamageReceiver.onDeath.AddListener(OnTool3Destroyed); }

		Debug.Assert(maxJumpPower > minJumpPower, "Minimum required jump power can't be higher than maximum possible");
		Debug.Assert(maxJumpPower > 0, "Maximum possible jump power must be higher than zero");
		Debug.Assert(jumpPowerGain > 0, "Jump power gain must be higher that zero units per second");

		jumpPower = minJumpPower;
	}

	public override void Update()
	{
		base.Update();

		//if(tool2 != null)
		//{
		//	resource2 = tool2.power;
		//}
	}

	//public override void DamageBack(float damage, DamageType damageType)
	//{
	//	if(tool3 != null && tool3.hp > 0){tool3.ReceiveDamage(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageFront(float damage, DamageType damageType)
	//{
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageLeft(float damage, DamageType damageType)
	//{
	//	if(tool2 != null && tool2.hp > 0){tool2.ReceiveDamage(damage, damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageRight(float damage, DamageType damageType)
	//{
	//	if(tool1 != null && tool1.hp > 0){tool1.ReceiveDamage(damage, damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

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
			body.AddForce((transform.up + move_direction).normalized * jumpPower);
		}

		jumpBreaksActive = false;
		jumpPower = minJumpPower;
	}
}