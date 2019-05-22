using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGenerator: SwitchingTool
{
	public float constantGain = 0.5f; //units per second
	public ResourceContainer energyResource;

	protected override void ActiveUpdate()
	{
		energyResource.Spend(-constantGain);
	}
}