using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSprayer: RepeatingTool
{
	public Missile sprayMissilePrefab;
	public Transform missileSpawner;
	public ResourceContainer liquidResource;

	public float energyConsumption = 0.05f; //units per second
	public float liquidConsumption = 1.2f; //units per second

	public float basicSpeed = 10;
	private float extraSpeed = 0;
	public float extraSpeedGain = 2.5f; //units per second
	public float extraSpeedReduction = 5.0f; //units per second
	public float maxExtraSpeed = 10.0f;

	public float basicRange = 3;
	private float extraRange = 0;
	public float extraRangeGain = 0.75f; //units per second
	public float extraRangeReduction = 1.25f; //units per second
	public float maxExtraRange = 2.0f;

	protected override void Action()
	{
		Missile newMissile = Instantiate(sprayMissilePrefab, missileSpawner.position, missileSpawner.rotation);
		newMissile.SetOwner(owner);
	}

	protected override bool Condition()
	{
		return !liquidResource.IsEmpty();
	}

	protected override void ActiveUpdate()
	{
		liquidResource.Spend(liquidConsumption * Time.deltaTime);
	}

	protected override void ConstantUpdate()
	{
		if(owner.move_direction != Vector3.zero)
		{
			if(extraSpeed < maxExtraSpeed)
			{
				extraSpeed += extraSpeedGain * Time.deltaTime;
				if(extraSpeed > maxExtraSpeed)
				{
					extraSpeed = maxExtraSpeed;
				}
			}

			if(extraRange < maxExtraRange)
			{
				extraRange += extraRangeGain * Time.deltaTime;
				if(extraRange > maxExtraRange)
				{
					extraRange = maxExtraRange;
				}
			}
		}
		else
		{
			if(extraSpeed > 0)
			{
				extraSpeed -= extraSpeedReduction * Time.deltaTime;
				if(extraSpeed < 0)
				{
					extraSpeed = 0;
				}
			}

			if(extraRange > 0)
			{
				extraRange -= extraRangeReduction * Time.deltaTime;
				if(extraRange < 0)
				{
					extraRange = 0;
				}
			}
		}
	}
}