using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGamepad : Controller
{
	private Vector2 globalAxis, localAxis;
	private bool stepHisteresisFlag = true;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		keyTool1.Update(Input.GetKey(tool1));
		keyTool2.Update(Input.GetKey(tool2));
		keyTool3.Update(Input.GetKey(tool3));
		keyPick.Update(Input.GetKey(pick));
		keyJump.Update(Input.GetKey(jump));
		keyMenu.Update(Input.GetKey(menu));

		globalAxis = Vector2.zero;
		localAxis = Vector2.zero;

		if (axis1x.Length > 0)
		{
			globalAxis = new Vector2(Input.GetAxis(axis1x), Input.GetAxis(axis1y));
		}

		if (axis2x.Length > 0)
		{
			//this would be wrong if robot view is turned up/down
			Vector2 r = new Vector2(unit.transform.right.x, unit.transform.right.z);
			Vector2 f = new Vector2(unit.transform.forward.x, unit.transform.forward.z);
			localAxis = r * Input.GetAxis(axis2x) + f * Input.GetAxis(axis2y);
		}

		switch (mode)
		{
			case Mode.GAME: GameUpdate(); break;
			case Mode.MENU: MenuUpdate(); break;
			case Mode.MIXED: MixedUpdate(); break;
		}
		
		base.Update();
	}

	private void GameUpdate()
	{
		if (!unit) { return; }

		if (localAxis != Vector2.zero && globalAxis != Vector2.zero)
		{
			unit.Move((localAxis + globalAxis).normalized);
		}
		else
		{
			if (localAxis != Vector2.zero)
			{
				unit.Move(localAxis);
			}
			else
			{
				unit.Move(globalAxis);
			}
		}
	}

	private void MenuUpdate()
	{
		if (!visual) { return; }
		if (!visual.selector) { return; }

		//Debug.Log(globalAxis.ToString() + " of length " + globalAxis.magnitude.ToString());

		if (stepHisteresisFlag)
		{
			if (globalAxis.magnitude > 0.75f)
			{
				visual.selector.Move8D(globalAxis);
				stepHisteresisFlag = false;
			}
		}
		else
		{
			if (globalAxis.magnitude < 0.1f)
			{
				stepHisteresisFlag = true;
			}
		}

		//switch (Support.DefineDirectoin(globalAxis))
		//{
		//	case Support.Direction.NONE: visual.selector.Move8D(Vector2.zero); break;
		//	case Support.Direction.UP: visual.selector.Move8D(Vector2.up); break;
		//	case Support.Direction.DOWN: visual.selector.Move8D(Vector2.down); break;
		//	case Support.Direction.LEFT: visual.selector.Move8D(Vector2.left); break;
		//	case Support.Direction.RIGHT: visual.selector.Move8D(new Vector2(1,1)); break;
		//	case Support.Direction.UP_LEFT: visual.selector.Move8D(new Vector2(1,1)); break;
		//	case Support.Direction.UP_RIGHT: visual.selector.Move8D(new Vector2(1,1)); break;
		//	case Support.Direction.DOWN_LEFT: visual.selector.Move8D(new Vector2(1,1)); break;
		//	case Support.Direction.DOWN_RIGHT: visual.selector.Move8D(new Vector2(1,1)); break;
		//}

		if (keyTool1.action == Key_class.Action.PRESS ||
			keyTool2.action == Key_class.Action.PRESS ||
			keyTool3.action == Key_class.Action.PRESS)
		{
			if (visual.selector.target)
			{
				visual.selector.target.Click(this);
			}
		}

		if (keyJump.action == Key_class.Action.PRESS ||
			keyMenu.action == Key_class.Action.PRESS ||
			keyPick.action == Key_class.Action.PRESS)
		{
			//back or cancel
		}
	}

	private void MixedUpdate()
	{
		
	}
}
