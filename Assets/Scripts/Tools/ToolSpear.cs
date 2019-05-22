using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSpear: RepeatingTool
{
	public MissileMelee spearMissile;
	public ResourceContainer energyResource;

	public string spearActiveAnimationParameter = "SpearActive";
	public string hitAnimation = "SpearHit";

	public float energyCost = 0.75f;

	private int hitAnimationId;
	private int animationParameterId;

	private void Start()
	{
		hitAnimationId = Animator.StringToHash(hitAnimation);

		Debug.Assert(owner.HaveAnimationParameter(spearActiveAnimationParameter),
			"Animation parameter " + spearActiveAnimationParameter +
			" of " + owner.name + " for " + name + " not found");

		animationParameterId = owner.GetAnimatorParameterId(spearActiveAnimationParameter);
	}

	protected override void Action()
	{
		energyResource.Spend(energyCost);

		spearMissile.gameObject.SetActive(true);

		owner.SetAnimatorParameter(animationParameterId, true);
		owner.Play(hitAnimationId);
	}

	protected override bool Condition()
	{
		return energyResource.Have(energyCost);
	}

	protected override void OnReload()
	{
		owner.SetAnimatorParameter(animationParameterId, false);
	}
}