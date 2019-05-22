using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAccelerator: SwitchingTool
{
	public Effect accelerationEffectPrefab;

	private Effect appliedEffect;

	protected override void OnActivation()
	{
		appliedEffect = owner.AddEffect(accelerationEffectPrefab.gameObject); //REWORK AddEffect() cuntion and send actual effect instead of game object
	}

	protected override void OnPassivation()
	{
		appliedEffect.Cancel();
	}
}