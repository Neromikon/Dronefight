using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class KeyBind : MonoBehaviour
{
	public enum TargetKey
	{
		UP, DOWN, LEFT, RIGHT,
		TOOL1, TOOL2, TOOL3,
		PICK, JUMP, MENU,
		AXIS1, AXIS2
	}

	public const string INPUT_WAIT_TEXT = "[...]";
	private const float AXIS_DETECT_TRESHOLD = 0.75f;

	public TargetKey targetKey;
	public Controller targetPlayer;
	public Text valueText;

	private bool trackOn = false;
	private string originalValueText;
	private int joystickIndex;
	private int xaxis, yaxis;
	private Vector2 axisScale;

	public void Start()
	{
		valueText = Support.GetComponentRecursive<Text>(transform, "Value");

		Button button = GetComponent<Button>();
		button.onClick.AddListener(StartTrack);

		Refresh();
	}

	private void Update()
	{
		TrackUpdate();
	}

	private void OnEnable()
	{
		Refresh();
	}

	private void TrackUpdate()
	{
		if (!trackOn) { return; }

		if (Input.GetKeyDown(KeyCode.Escape)) { SetKey(KeyCode.Escape); return; }

		switch (targetPlayer.controlType)
		{
			case Controller.ControlType.KEYBOARD:
			{
				if (!Input.anyKey) { return; }

				for (KeyCode i = KeyCode.A; i <= KeyCode.Z; i++)
				{
					if (Input.GetKeyDown(i))
					{
						SetKey(i);
						return;
					}
				}

				if (Input.GetKeyDown(KeyCode.LeftArrow)) { SetKey(KeyCode.LeftArrow); return; }
				if (Input.GetKeyDown(KeyCode.RightArrow)) { SetKey(KeyCode.RightArrow); return; }
				if (Input.GetKeyDown(KeyCode.UpArrow)) { SetKey(KeyCode.UpArrow); return; }
				if (Input.GetKeyDown(KeyCode.DownArrow)) { SetKey(KeyCode.DownArrow); return; }

				for (KeyCode i = KeyCode.Alpha0; i <= KeyCode.Alpha9; i++)
				{
					if (Input.GetKeyDown(i))
					{
						SetKey(i);
						return;
					}
				}

				for (KeyCode i = KeyCode.Keypad0; i <= KeyCode.Keypad9; i++)
				{
					if (Input.GetKeyDown(i))
					{
						SetKey(i);
						return;
					}
				}

				if (Input.GetKeyDown(KeyCode.KeypadDivide)) { SetKey(KeyCode.KeypadDivide); return; }
				if (Input.GetKeyDown(KeyCode.KeypadEnter)) { SetKey(KeyCode.KeypadEnter); return; }
				if (Input.GetKeyDown(KeyCode.KeypadEquals)) { SetKey(KeyCode.KeypadEquals); return; }
				if (Input.GetKeyDown(KeyCode.KeypadMinus)) { SetKey(KeyCode.KeypadMinus); return; }
				if (Input.GetKeyDown(KeyCode.KeypadMultiply)) { SetKey(KeyCode.KeypadMultiply); return; }
				if (Input.GetKeyDown(KeyCode.KeypadPeriod)) { SetKey(KeyCode.KeypadPeriod); return; }
				if (Input.GetKeyDown(KeyCode.KeypadPlus)) { SetKey(KeyCode.KeypadPlus); return; }

				if (Input.GetKeyDown(KeyCode.Semicolon)) { SetKey(KeyCode.Semicolon); return; }
				if (Input.GetKeyDown(KeyCode.Less)) { SetKey(KeyCode.Less); return; }
				if (Input.GetKeyDown(KeyCode.Greater)) { SetKey(KeyCode.Greater); return; }
				if (Input.GetKeyDown(KeyCode.LeftBracket)) { SetKey(KeyCode.LeftBracket); return; }
				if (Input.GetKeyDown(KeyCode.RightBracket)) { SetKey(KeyCode.RightBracket); return; }
				if (Input.GetKeyDown(KeyCode.Return)) { SetKey(KeyCode.Return); return; }
				if (Input.GetKeyDown(KeyCode.Space)) { SetKey(KeyCode.Space); return; }
				if (Input.GetKeyDown(KeyCode.Tab)) { SetKey(KeyCode.Tab); return; }
				if (Input.GetKeyDown(KeyCode.LeftShift)) { SetKey(KeyCode.LeftShift); return; }
				if (Input.GetKeyDown(KeyCode.RightShift)) { SetKey(KeyCode.RightShift); return; }

				break;
			}

			case Controller.ControlType.MOUSE:
			{
				if (!Input.anyKey) { return; }

				if (Input.GetKeyDown(KeyCode.Mouse0)) { SetKey(KeyCode.Mouse0); return; }
				if (Input.GetKeyDown(KeyCode.Mouse1)) { SetKey(KeyCode.Mouse1); return; }
				if (Input.GetKeyDown(KeyCode.Mouse2)) { SetKey(KeyCode.Mouse2); return; }
				if (Input.GetKeyDown(KeyCode.Mouse3)) { SetKey(KeyCode.Mouse3); return; }
				if (Input.GetKeyDown(KeyCode.Mouse4)) { SetKey(KeyCode.Mouse4); return; }
				if (Input.GetKeyDown(KeyCode.Mouse5)) { SetKey(KeyCode.Mouse5); return; }
				if (Input.GetKeyDown(KeyCode.Mouse6)) { SetKey(KeyCode.Mouse6); return; }

				break;
			}

			case Controller.ControlType.GAMEPAD:
			{
				if (targetKey >= TargetKey.AXIS1 && targetKey <= TargetKey.AXIS2)
				{
					for (int a = 1; a <= 10; a++)
					{
						if (a == xaxis) { continue; }

						float value = Support.GetJoystickAxis(joystickIndex, a);

						if (value < AXIS_DETECT_TRESHOLD) { continue; }

						if (xaxis == -1)
						{
							xaxis = a;
							axisScale.x = (value > 0) ? 1.0f : -1.0f;
						}
						else
						{
							yaxis = a;
							axisScale.y = (value > 0) ? 1.0f : -1.0f;

							SetAxis("Joystick" + joystickIndex + "Axis" + xaxis, "Joystick" + joystickIndex + "Axis" + yaxis, axisScale);
							return;
						}
					}
				}
				else
				{
					if (!Input.anyKey) { return; }

					KeyCode first = KeyCode.Joystick1Button0 + (KeyCode.Joystick2Button0 - KeyCode.Joystick1Button0) * joystickIndex;
					KeyCode last = first + (KeyCode.Joystick2Button0 - KeyCode.Joystick1Button0);

					for (KeyCode i = first; i < last; i++)
					{
						if (Input.GetKeyDown(i))
						{
							SetKey(i);
							return;
						}
					}
				}
				break;
			}

			default: break;
		}
	}

	public void Refresh()
	{
		if(valueText == null) { return; }
		if(targetPlayer == null) { return; }

		Regex axisNumberRegex = new Regex(@"\d+");

		switch (targetKey)
		{
			case TargetKey.UP:      valueText.text = targetPlayer.up.ToString();      break;
			case TargetKey.DOWN:    valueText.text = targetPlayer.down.ToString();    break;
			case TargetKey.LEFT:    valueText.text = targetPlayer.left.ToString();    break;
			case TargetKey.RIGHT:   valueText.text = targetPlayer.right.ToString();   break;
			case TargetKey.JUMP:    valueText.text = targetPlayer.jump.ToString();    break;
			case TargetKey.TOOL1:   valueText.text = targetPlayer.tool1.ToString();   break;
			case TargetKey.TOOL2:   valueText.text = targetPlayer.tool2.ToString();   break;
			case TargetKey.TOOL3:   valueText.text = targetPlayer.tool3.ToString();   break;
			case TargetKey.PICK:    valueText.text = targetPlayer.pick.ToString();    break;
			case TargetKey.MENU:    valueText.text = targetPlayer.menu.ToString();    break;

			case TargetKey.AXIS1:
			{
				if (targetPlayer.axis1x.Length > 0 && targetPlayer.axis1y.Length > 0)
				{
					valueText.text =
						(targetPlayer.axis1Scale.x >= 0 ? "+axis" : "-axis") +
						axisNumberRegex.Match(targetPlayer.axis1x, 9).Value +
						(targetPlayer.axis1Scale.y >= 0 ? " & +axis" : " & -axis") +
						axisNumberRegex.Match(targetPlayer.axis1y, 9).Value;
				}
				else
				{
					valueText.text = "[-]";
				}
				break;
			}

			case TargetKey.AXIS2:
			{
				if (targetPlayer.axis2x.Length > 0 && targetPlayer.axis2y.Length > 0)
				{
					valueText.text =
						(targetPlayer.axis2Scale.x >= 0 ? "+axis" : "-axis") +
						axisNumberRegex.Match(targetPlayer.axis2x, 9).Value +
						(targetPlayer.axis2Scale.y >= 0 ? " & +axis" : " & -axis") +
						axisNumberRegex.Match(targetPlayer.axis2y, 9).Value;
				}
				else
				{
					valueText.text = "[-]";
				}
				break;
			}
		}

		if(valueText.text.Length > 6)
		{
			valueText.fontSize = 10;
		}
		else
		{
			valueText.fontSize = 14;
		}
	}

	public void StartTrack()
	{
		if (trackOn)
		{
			trackOn = false;

			if (targetKey >= TargetKey.AXIS1 && targetKey <= TargetKey.AXIS2)
			{
				SetAxis(string.Empty, string.Empty, Vector2.zero);
			}
			else
			{
				SetKey(KeyCode.None);
			}
		}
		else
		{
			trackOn = true;
			originalValueText = valueText.text;
			valueText.text = INPUT_WAIT_TEXT;
			xaxis = -1;
			yaxis = -1;
		}
	}

	public void SetKey(KeyCode keyCode)
	{
		trackOn = false;

		if(keyCode == KeyCode.Escape)
		{
			valueText.text = originalValueText;
			return;
		}

		switch(targetKey)
		{
			case TargetKey.UP:      targetPlayer.up = keyCode;      break;
			case TargetKey.DOWN:    targetPlayer.down = keyCode;    break;
			case TargetKey.LEFT:    targetPlayer.left = keyCode;    break;
			case TargetKey.RIGHT:   targetPlayer.right = keyCode;   break;
			case TargetKey.JUMP:    targetPlayer.jump = keyCode;    break;
			case TargetKey.TOOL1:   targetPlayer.tool1 = keyCode;   break;
			case TargetKey.TOOL2:   targetPlayer.tool2 = keyCode;   break;
			case TargetKey.TOOL3:   targetPlayer.tool3 = keyCode;   break;
			case TargetKey.PICK:    targetPlayer.pick = keyCode;    break;
			case TargetKey.MENU:    targetPlayer.menu = keyCode;    break;
		}

		valueText.text = keyCode.ToString();

		GameSettings.Save();
	}

	void SetAxis(string xaxis, string yaxis, Vector2 multiplier)
	{
		switch (targetKey)
		{
			case TargetKey.AXIS1:
			{
				targetPlayer.axis1x = xaxis;
				targetPlayer.axis1y = yaxis;
				break;
			}

			case TargetKey.AXIS2:
			{
				targetPlayer.axis2x = xaxis;
				targetPlayer.axis2y = yaxis;
				break;
			}

			default: Debug.Assert(false, "Setting axis for non-axis control"); break;
		}

		//valueText.text = xaxis;

		Refresh();

		GameSettings.Save();
	}
}
