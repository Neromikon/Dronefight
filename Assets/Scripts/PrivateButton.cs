using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PrivateButton : Button
{
	public Controller owner;
	public ButtonClickedEvent privateOnClick;

	protected override void Start()
    {
		base.Start();

		onClick.AddListener(StandardOnClick);
	}

	private void StandardOnClick()
	{
		Click(GameSettings.mainPlayer);
	}

	public void Click(Controller sender)
	{
		if (owner == GameSettings.mainPlayer || owner == null)
		{
			privateOnClick.Invoke();
		}
		else switch (owner.controlType)
		{
			case Controller.ControlType.MOUSE:
			case Controller.ControlType.TOUCHSCREEN:
			case Controller.ControlType.ALGORITHM:
			case Controller.ControlType.NEURAL_NETWORK:
			{
				privateOnClick.Invoke();
				break;
			}

			case Controller.ControlType.GAMEPAD:
			case Controller.ControlType.KEYBOARD:
			{
				if (owner == sender) { privateOnClick.Invoke(); }
				break;
			}
		}
		
	}

	//protected virtual void OnClick()
	//{
	//	privateOnClick.Invoke();
	//}
}