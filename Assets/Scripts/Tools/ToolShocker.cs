using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolShocker: ChargingTool
{
	public Missile wasteChargeMissilePrefab;
	public Missile airChargeMissilePrefab;
	public Missile clingChargeMissilePrefab;
	public MissileClaw claw;
	public ImpulseObjectSensor sensor;
	public ResourceContainer energyResource;
	public ResourceContainer chargeResource;

	public float chargeGain = 1.0f; //units per second
	public float chargeGainEnergyCost = 0.1f; //units per second
	public float minimumCharge = 15.0f;

	public float immediateChargeValue = 55.0f;
	public float immediateChargeCost = 2.0f;

	public float chargeExtraEnergyCost = 0.5f; //instantly
	
	public LineRenderer[] lightningsPrefabs;
	
	protected override void Action()
	{
		sensor.gameObject.SetActive(true);
	}

	protected override bool ChargeCondition()
	{
		return !energyResource.IsEmpty();
	}

	protected override void ChargeUpdate()
	{
		chargeResource.Spend(-chargeGain * Time.deltaTime);
		energyResource.Spend(chargeGainEnergyCost * Time.deltaTime);
	}

	protected override void ReloadUpdate()
	{
		if (!chargeResource.IsEmpty()) { ActualAction(); }
	}

	protected override void ReadyUpdate()
	{
		if (!chargeResource.IsEmpty()) { ActualAction(); }
	}

	private void ActualAction()
	{
		chargeResource.Spend();

		switch (claw.state)
		{
			case MissileClaw.ClawState.CLING:
			case MissileClaw.ClawState.SHOOT:
			{
				//todo: if target is conductive then spend extra energy and deal appropriate damage

				Missile newMissile = Instantiate(clingChargeMissilePrefab, claw.transform.position, claw.transform.rotation);
				newMissile.SetOwner(owner);

				energyResource.Spend(chargeExtraEnergyCost);

				break;
			}

			case MissileClaw.ClawState.READY:
			case MissileClaw.ClawState.RETURN:
			{
				Missile newMissile = Instantiate(clingChargeMissilePrefab);
				newMissile.SetOwner(owner);
				newMissile.transform.rotation = sensor.transform.rotation;

				if (sensor.closestObject)
				{
					newMissile.transform.position = sensor.closestObject.transform.position;
					energyResource.Spend(chargeExtraEnergyCost); //todo deal extra damage if there is that extra energy is present

					LineRenderer lightning = Instantiate(lightningsPrefabs[Random.Range(0, lightningsPrefabs.Length)]);
					lightning.SetPosition(0, sensor.transform.position);
					lightning.SetPosition(1, sensor.closestObject.transform.position);
				}
				else
				{
					newMissile.transform.position = sensor.transform.position;
				}

				energyResource.Spend(chargeExtraEnergyCost);

				break;
			}
		}
	}
}