using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCutter: Unit
{
	public enum DefenceState { ATTACK, DEFENSE }

	public DefenceState defenceState;
	
	public float minJumpPower;
	public float maxJumpPower;
	public float jumpPowerGain; //units per second

	private float jumpPower;

	public override void Start()
	{
		base.Start();

		jumpPower = minJumpPower;
	}

	public override void Update()
	{
		base.Update();
	}

	//private void LateUpdate()
	//{
	//	Transform fnd = transform.Find("Model/Armature/Heart/Waist");
	//	if(fnd != null)
	//	{
	//		fnd.rotation = Quaternion.LookRotation(Vector3.down, Vector3.Cross(Vector3.up, legs_direction));
	//	}
	//}

	public override void ReceiveDamage(float damage, DamageType damageType)
	{
		if(tool2 != null)
		if(tool2.active == true)
		{
			//tool2.ReceiveDamage(float damage, DamageType);
		}
		//if(damageType == ACID){;}
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
