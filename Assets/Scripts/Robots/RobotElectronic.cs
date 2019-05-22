using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotElectronic: Unit
{
	public float minJumpPower;
	public float maxJumpPower;
	public float jumpPowerGain; //units per second

	private float jumpPower;

	public override void Start()
	{
		base.Start();
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
			body.AddForce(transform.up * jumpPower);
		}

		jumpPower = minJumpPower;
	}
}
