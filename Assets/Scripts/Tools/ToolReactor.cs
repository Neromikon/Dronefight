using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolReactor: SwitchingTool
{
	public MissileField radiationFieldMissile;
	public ResourceContainer radiationResource;

	public float radiationConsumption = 1.0f; //units per second

	protected override void OnActivation()
	{
		radiationFieldMissile.gameObject.SetActive(true);
	}

	protected override void OnPassivation()
	{
		radiationFieldMissile.gameObject.SetActive(false);
	}

	protected override bool ActivationCondition()
	{
		return !radiationResource.IsEmpty();
	}

	protected override bool KeepActiveCondition()
	{
		return !radiationResource.IsEmpty();
	}

	protected override void ActiveUpdate()
	{
		radiationResource.Spend(radiationConsumption * Time.deltaTime);
	}
}