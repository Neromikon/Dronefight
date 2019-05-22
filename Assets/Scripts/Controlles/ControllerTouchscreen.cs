using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerTouchscreen : Controller
{
	public VirtualJoystick virtualJoystick;
	public VirtualKey tool1VirtualKey;
	public VirtualKey tool2VirtualKey;
	public VirtualKey tool3VirtualKey;
	public VirtualKey jumpVirtualKey;
	public VirtualKey pickVirtualKey;
	public VirtualKey menuVirtualKey;

	protected override void Update()
	{
		if (!unit) { return; }

		if (tool1VirtualKey) { keyTool1.Update(tool1VirtualKey.pressed); }
		if (tool2VirtualKey) { keyTool2.Update(tool2VirtualKey.pressed); }
		if (tool3VirtualKey) { keyTool3.Update(tool3VirtualKey.pressed); }
		if (pickVirtualKey) { keyPick.Update(pickVirtualKey.pressed); }
		if (jumpVirtualKey) { keyJump.Update(jumpVirtualKey.pressed); }
		if (menuVirtualKey) { keyMenu.Update(menuVirtualKey.pressed); }

		unit.Move(virtualJoystick.axis);

		base.Update();
	}

	public void SetVirtualControl(VirtualControl virtualControl)
	{
		if (virtualControl)
		{
			virtualJoystick = virtualControl.joystick;
			tool1VirtualKey = virtualControl.tool1;
			tool2VirtualKey = virtualControl.tool2;
			tool3VirtualKey = virtualControl.tool3;
			pickVirtualKey = virtualControl.pick;
			menuVirtualKey = virtualControl.menu;
			jumpVirtualKey = virtualControl.jump;
		}
		else
		{
			virtualJoystick = null;
			tool1VirtualKey = null;
			tool2VirtualKey = null;
			tool3VirtualKey = null;
			pickVirtualKey = null;
			menuVirtualKey = null;
			jumpVirtualKey = null;
		}
	}
}
