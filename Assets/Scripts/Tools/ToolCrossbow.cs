using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCrossbow: ChargingTool
{
	public Missile arrowMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer pullingResource;
	public ResourceContainer energyResource;
	public ResourceContainer arrowsResource;

	public float minimumPulling = 25.0f;
	public float pullingGain = 10.0f; //units per second
	public float energyConsumption = 0.2f; //units per second
	public float baseSpeedCoefficient = 0.5f; //[0..1]

	public Missile specialCharge = null;

	public string shootAnimation = "CrossbowShoot";
	public string loadAnimation = "CrossbowLoad";
	public int animationLayer;

	private int shootAnimationId;
	private int loadAnimationId;

	private void Start()
	{
		shootAnimationId = Animator.StringToHash(shootAnimation);
		loadAnimationId = Animator.StringToHash(loadAnimation);
	}

	protected override void Action()
	{
		Missile newMissile = Instantiate(arrowMissilePrefab, missileSpawner.position, missileSpawner.rotation);
		newMissile.SetOwner(owner);
		newMissile.speed *= baseSpeedCoefficient + (1.0f - baseSpeedCoefficient) * pullingResource.percentage;

		arrowsResource.Spend(1);
		pullingResource.Spend();

		owner.Play(shootAnimationId);
	}

	protected override bool ChargeCondition()
	{
		if (pullingResource.IsMaxed()) { return false; }
		if (energyResource.IsEmpty()) { return false; }

		return true;
	}

	protected override bool ActionCondition()
	{
		if (arrowsResource.IsEmpty()) { return false; }
		if (!pullingResource.Have(minimumPulling)) { return false; }

		return true;
	}

	protected override void ChargeUpdate()
	{
		pullingResource.Spend(-pullingGain * Time.deltaTime);
		energyResource.Spend(energyConsumption * Time.deltaTime);
		owner.PlayMoment(loadAnimationId, animationLayer, pullingResource.percentage);
	}
}