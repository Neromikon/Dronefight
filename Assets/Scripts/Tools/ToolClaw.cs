using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolClaw: Tool
{
	public MissileClaw missileClaw;

	public override void Push()
	{
		switch (missileClaw.state)
		{
			case MissileClaw.ClawState.READY:
			{
				missileClaw.Shoot();
				break;
			}

			case MissileClaw.ClawState.SHOOT:
			case MissileClaw.ClawState.CLING:
			{
				missileClaw.Return();
				break;
			}

			case MissileClaw.ClawState.RETURN:
			{
				break;
			}
		}
	}
}