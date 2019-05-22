using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHammer: HoldingTool
{
	public MissileMelee hammerMissile;

	public float damage;
	public float energyCost = 0.75f;
	public ResourceContainer energyResource;

	public string prepareAnimation = "CrusherHammerPrepare";
	public string hitAnimation = "CrusherHammerHit";

	private int prepareAnimationId;
	private int hitAnimationId;

	private void Start()
	{
		prepareAnimationId = Animator.StringToHash(prepareAnimation);
		hitAnimationId = Animator.StringToHash(hitAnimation);
	}

	protected override void Prepare()
	{
		energyResource.Spend(energyCost * 0.25f);

		owner.Play(prepareAnimationId);
	}

	protected override void Action()
	{
		hammerMissile.gameObject.SetActive(true);

		energyResource.Spend(energyCost * 0.70f);

		owner.Play(hitAnimationId);
	}

	protected override bool Condition()
	{
		return energyResource.Have(energyCost);
	}

	protected override void OnReload()
	{
		hammerMissile.gameObject.SetActive(false);

		energyResource.Spend(energyCost * 0.05f);
	}
}