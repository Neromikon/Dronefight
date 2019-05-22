using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFlamer: Unit
{
	public float flightPower;
	public float flightFuelConsumption; //units per second

	public ParticleSystem[] turbinesParticleSystems;

	public float flightParticlesPerSecondMultiplier = 1.5f;
	public float flightParticlesLifetimeMiltiplier = 1.5f;

	private ParticleSystem.EmissionModule[] turbinesParticleEmitters;
	private ParticleSystem.MainModule[] turbinesParticlesMainModules;

	private float basicParticlesPerSecondMultiplier;
	private float basicParticlesLifetimeMultiplier;

	public override void Start()
	{
		base.Start();

		turbinesParticleEmitters = new ParticleSystem.EmissionModule[turbinesParticleSystems.Length];
		turbinesParticlesMainModules = new ParticleSystem.MainModule[turbinesParticleSystems.Length];

		for (int i = 0; i < turbinesParticleSystems.Length; i++)
		{
			turbinesParticleEmitters[i] = turbinesParticleSystems[i].emission;
			turbinesParticlesMainModules[i] = turbinesParticleSystems[i].main;
		}

		basicParticlesPerSecondMultiplier = turbinesParticleEmitters[0].rateOverTimeMultiplier;
		basicParticlesLifetimeMultiplier = turbinesParticlesMainModules[0].startLifetimeMultiplier;
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
		if (motionResource.IsEmpty()) { return; }

		motionResource.Spend(flightFuelConsumption * Time.deltaTime);

		body.AddForce((transform.up + move_direction).normalized * flightPower);

		for (int i = 0; i < turbinesParticleSystems.Length; i++)
		{
			turbinesParticleEmitters[i].rateOverTimeMultiplier = basicParticlesPerSecondMultiplier * flightParticlesPerSecondMultiplier;
			turbinesParticlesMainModules[i].startLifetimeMultiplier = basicParticlesLifetimeMultiplier * flightParticlesLifetimeMiltiplier;
		}
	}

	public override void JumpRelease()
	{
		for (int i = 0; i < turbinesParticleSystems.Length; i++)
		{
			turbinesParticleEmitters[i].rateOverTimeMultiplier = basicParticlesPerSecondMultiplier;
			turbinesParticlesMainModules[i].startLifetimeMultiplier = basicParticlesLifetimeMultiplier;
		}
	}
}
