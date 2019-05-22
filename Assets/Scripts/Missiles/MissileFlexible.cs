using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileFlexible : Missile
{
	//Note: I used here ParticleSystem curves as general tool for managing parameterized values
	//(it is not related to particles at all)

	public ParticleSystem.MinMaxCurve velocityCurve; //depends on normalized power
	public ParticleSystem.MinMaxCurve damageCurve; //depends on normalized power
	public ParticleSystem.MinMaxCurve scaleCurve; //depends on normalized power
	public ParticleSystem.MinMaxCurve emissionCurve; //depends on normalized power

	public float powerLoss = 0;
	
	private float currentPower;

	protected override void Update()
	{
		base.Update();

		if(distanceToTravel <= 0){return;}

		if (powerLoss > 0 && currentPower > 0)
		{
			currentPower -= powerLoss * Time.deltaTime;

			if (currentPower < 0) { currentPower = 0; }

			Refresh(currentPower);
		}

		float distance = (startingPoint - transform.position).magnitude;
		float percentage = distance / distanceToTravel;
	}

	public void SetPower(float power)
	{
		currentPower = power;
		Refresh(power);

		Debug.Log(name + " speed is " + body.velocity.magnitude + ", damage is " + damage + ", scale is " + transform.localScale.x);
	}

	private void Refresh(float normalizedPower)
	{
		body.velocity = transform.forward * speed * velocityCurve.Evaluate(normalizedPower);
		damage = damageCurve.Evaluate(normalizedPower);
		transform.localScale = Vector3.one * scaleCurve.Evaluate(normalizedPower);

		if (particles)
		{
			ParticleSystem.EmissionModule emissionModule = particles.emission;
			emissionModule.rateOverTime = emissionCurve.Evaluate(normalizedPower);
		}
	}
}
