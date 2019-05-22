using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolShield: SwitchingTool
{
	public string shieldActiveAnimationParameter = "ShieldActive";
	
	private int animationParameterId;

	private void Start()
	{
		Debug.Assert(owner.HaveAnimationParameter(shieldActiveAnimationParameter),
			"Animation parameter " + shieldActiveAnimationParameter +
			" of " + owner.name + " for " + name + " not found");

		animationParameterId = owner.GetAnimatorParameterId(shieldActiveAnimationParameter);
	}

	protected override void OnActivation()
	{
		owner.SetAnimatorParameter(animationParameterId, true);
	}

	protected override void OnPassivation()
	{
		owner.SetAnimatorParameter(animationParameterId, false);
	}
}