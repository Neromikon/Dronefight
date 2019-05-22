using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMouse : Controller
{
	Vector2 currentMovement = Vector2.zero;

	protected override void Update()
	{
		if (!unit) { return; }

		keyTool1.Update(Input.GetKey(tool1));
		keyTool2.Update(Input.GetKey(tool2));
		keyTool3.Update(Input.GetKey(tool3));
		keyPick.Update(Input.GetKey(pick));
		keyJump.Update(Input.GetKey(jump));
		keyMenu.Update(Input.GetKey(menu));

		if (axis1x.Length > 0)
		{
			Vector2 mouseMove = new Vector2(Input.GetAxis(axis1x), Input.GetAxis(axis1y)).normalized;

			if (mouseMove == Vector2.zero)
			{
				unit.Move(currentMovement);
			}
			else
			{
				float difference = Vector2.Dot(currentMovement, mouseMove);
				if (difference >= 0)
				{
					currentMovement = mouseMove;
					unit.Move(currentMovement);
				}
				else
				{
					currentMovement = Vector2.zero;
				}
			}
		}

		base.Update();		
	}
}
