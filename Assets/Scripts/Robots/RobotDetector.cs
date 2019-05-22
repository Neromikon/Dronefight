using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDetector: Unit
{
	public float flightPower;
	public float jumpPower;
	public ResourceContainer rocketFuelResource;
	public float rocketFuelConsumption; //units per second
	public float jumpFuelCost;

	public ParticleSystem continuousRocketFlameParticleSystem;
	public ParticleSystem impulseRocketFlameParticleSystem;
	public int flameParticlesOnJump = 300;
	
	private ParticleSystem.EmissionModule rocketFlameEmissionModule;

	public override void Start()
	{
		base.Start();

		rocketFlameEmissionModule = continuousRocketFlameParticleSystem.emission;
		rocketFlameEmissionModule.enabled = false;
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
		if (rocketFuelResource.IsEmpty()) { return; }

		rocketFuelResource.Spend(rocketFuelConsumption * Time.deltaTime);

		body.AddForce((transform.up + move_direction).normalized * flightPower);

		rocketFlameEmissionModule.enabled = true;
	}

	public override void JumpRelease()
	{
		rocketFlameEmissionModule.enabled = false;
	}

	public override void JumpDoubleClick()
	{
		if (!rocketFuelResource.Have(jumpFuelCost)) { return; }

		rocketFuelResource.Spend(jumpFuelCost);

		body.AddForce(transform.up * jumpPower);

		impulseRocketFlameParticleSystem.Emit(flameParticlesOnJump);
	}
}