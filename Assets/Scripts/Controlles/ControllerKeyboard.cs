using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerKeyboard : Controller
{
	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		keyLeft.Update(Input.GetKey(left));
		keyRight.Update(Input.GetKey(right));
		keyUp.Update(Input.GetKey(up));
		keyDown.Update(Input.GetKey(down));
		keyTool1.Update(Input.GetKey(tool1));
		keyTool2.Update(Input.GetKey(tool2));
		keyTool3.Update(Input.GetKey(tool3));
		keyPick.Update(Input.GetKey(pick));
		keyJump.Update(Input.GetKey(jump));
		keyMenu.Update(Input.GetKey(menu));

		switch (mode)
		{
			case Mode.GAME: GameUpdate(); break;
			case Mode.MENU: MenuUpdate(); break;
			case Mode.MIXED: MixedUpdate(); break;
			case Mode.TEST: TestUpdate(); break;
			case Mode.VIEW: ViewUpdate(); break;
		}

		base.Update();
	}

	private void GameUpdate()
	{
		if (!unit) { return; }

		unit.Move(Support.GetMovementDirection(keyUp.state, keyDown.state, keyLeft.state, keyRight.state));
	}

	private void MenuUpdate()
	{
		if (!visual) { return; }
		if (!visual.selector) { return; }

		if (keyLeft.action == Key_class.Action.PRESS) { visual.selector.Move4D(Vector2.left); }
		if (keyRight.action == Key_class.Action.PRESS) { visual.selector.Move4D(Vector2.right); }
		if (keyUp.action == Key_class.Action.PRESS) { visual.selector.Move4D(Vector2.up); }
		if (keyDown.action == Key_class.Action.PRESS) { visual.selector.Move4D(Vector2.down); }

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
		//if (!visual.selector) { return; }
	}

	private void TestUpdate()
	{
		unit.Move(Support.GetMovementDirection(keyUp.state, keyDown.state, keyLeft.state, keyRight.state));
	}

	private void ViewUpdate()
	{
		if (!controllingCamera) { return; }
		if (!viewCameraTarget) { return; }

		Vector2 rotationVector = Support.GetMovementDirection(keyUp.state, keyDown.state, keyLeft.state, keyRight.state);

		controllingCamera.transform.RotateAround(
			viewCameraTarget.position,
			viewCameraTarget.up,
			rotationVector.x * VIEW_ROTATION_ANGULAR_SPEED * Time.deltaTime);

		controllingCamera.transform.RotateAround(
			viewCameraTarget.position,
			controllingCamera.transform.right,
			rotationVector.y * VIEW_ROTATION_ANGULAR_SPEED * Time.deltaTime);
	}
}
