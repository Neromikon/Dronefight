using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCrusher: Unit
{
	public float jumpPower;
	public float jumpReloadDuration; //seconds

	private Support.Timer jumpReloadTimer;

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();

		jumpReloadTimer.Update();
	}

	//public override void DamageBack(float damage, DamageType damageType)
	//{
	//	if(storageState > 0){DamageStorage(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageFront(float damage, DamageType damageType)
	//{
	//	if(tool1 != null && tool1.hp > 0)
	//	{
	//		tool1.ReceiveDamage(damage, damageType);

	//		if (tool1.hp == 0)
	//		{
	//			hammerModel.gameObject.SetActive(false);
	//		}
	//	}
	//	else if(bodyState > 0)
	//	{
	//		DamageBody(damage,damageType);
	//	}
	//}

	//public override void DamageLeft(float damage, DamageType damageType)
	//{
	//	if(tool2 != null && tool2.hp > 0)
	//	{
	//		tool2.ReceiveDamage(damage, damageType);

	//		if (tool2.hp == 0)
	//		{
	//			shotgunModel.gameObject.SetActive(false);
	//		}
	//	}
	//	else if(bodyState > 0)
	//	{
	//		DamageBody(damage,damageType);
	//	}
	//}

	//public override void DamageRight(float damage, DamageType damageType)
	//{
	//	if(tool3 != null && tool3.hp > 0)
	//	{
	//		tool3.ReceiveDamage(damage, damageType);

	//		if (tool3.hp == 0)
	//		{
	//			magnetModel.gameObject.SetActive(false);
	//		}
	//	}
	//	else if(bodyState > 0)
	//	{
	//		DamageBody(damage,damageType);
	//	}
	//}

	public override void JumpPress()
	{
		if (!bottomGroundSensor.contact) { return; }
		if (!jumpReloadTimer.Expired) { return; }

		body.AddForce(transform.up * jumpPower);
		//body.AddForceAtPosition(-Physics.gravity * jumpPower, transform.position);

		jumpReloadTimer.Start(jumpReloadDuration);
	}
}