using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSaw: RepeatingTool
{
	public MissileMelee sawMeleeMissile;
	public MissileMelee sawRangedMissile;
	public ResourceContainer energyResource;
	public ResourceContainer fuelResource;

	public bool useEnergy = true;
	public bool toolLocked = false; //becomes true when shield mode activated

	public float energyConsumption = 0.4f; //units per second
	public float fuelConsumption = 0.9f; //units per second

	public float minDamage = 1.0f;
	public float maxDamage = 3.0f;
	public float damageGain = 2.0f; //units per second
	public float damageLoss = 1.15f; //units per second

	public string sawActiveAnimationParameter = "SawActive";

	private float currentDamage;
	private int animationParameterId;

	private void Start()
	{
		Debug.Assert(owner.HaveAnimationParameter(sawActiveAnimationParameter),
			"Animation parameter " + sawActiveAnimationParameter +
			" of " + owner.name + " for " + name + " not found");

		animationParameterId = owner.GetAnimatorParameterId(sawActiveAnimationParameter);
	}

	protected override void Action()
	{
		sawMeleeMissile.damage = currentDamage;
		sawMeleeMissile.gameObject.SetActive(true);
	}

	protected override void OnPrepare()
	{
		owner.SetAnimatorParameter(animationParameterId, true);
	}

	protected override void OnReload()
	{
		owner.SetAnimatorParameter(animationParameterId, false);
	}

	protected override bool Condition()
	{
		if (useEnergy)
		{
			return !energyResource.IsEmpty();
		}
		else
		{
			return !fuelResource.IsEmpty();
		}
	}

	protected override void ActiveUpdate()
	{
		if (useEnergy)
		{
			energyResource.Spend(energyConsumption * Time.deltaTime);
		}
		else
		{
			fuelResource.Spend(fuelConsumption * Time.deltaTime);
		}

		if (currentDamage != maxDamage)
		{
			currentDamage += damageGain * Time.deltaTime;
			if (currentDamage > maxDamage) { currentDamage = maxDamage; }
		}
	}

	protected override void PassiveUpdate()
	{
		if (currentDamage != minDamage)
		{
			currentDamage -= damageLoss * Time.deltaTime;
			if (currentDamage < minDamage) { currentDamage = minDamage; }
		}
	}
}