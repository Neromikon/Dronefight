using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Controller : MonoBehaviour
{
	public enum PlayerType
	{
		LOCAL,
		REMOTE,
		BOT
	}

	public enum ControlType
	{
		KEYBOARD,
		MOUSE,
		GAMEPAD,
		TOUCHSCREEN,
		ALGORITHM,
		NEURAL_NETWORK
	}

	public enum GamepadMovementType
	{
		ARROWS,
		LEFT_STICK,
		RIGHT_STICK
	}

	public enum Team
	{
		NEUTRAL, BLUE, RED, VIEWERS, TRAINERS
	}

	public enum Mode
	{
		MENU, GAME, MIXED, TEST, VIEW, TRAIN
	}

	public const int UNUSED_LOADS_LIMIT = 10;
	public const float VIEW_ROTATION_ANGULAR_SPEED = 70.0f; //deg/sec

	public PlayerType type;
	public ControlType controlType = ControlType.KEYBOARD;
	public Team team;
	public GamepadMovementType gamepadMovementType = GamepadMovementType.ARROWS;
	public int layout;
	public KeyCode left, right, up, down, jump;
	public KeyCode forward, back, turnLeft, turnRight;
	public KeyCode tool1, tool2, tool3, pick, menu;
	public string axis1x, axis1y, axis2x, axis2y;
	public Vector2 axis1Scale, axis2Scale;
	public PlayerVisualScheme visual;
	public ControllerPanel configPanel;

	public Unit unitPrefab;
	public Unit unit;

	public string localizedName;

	public bool isAvailable = true;
	public int timesBeenUnused; //how many times the controller was not present on device on app start

	public Camera controllingCamera;
	public Transform viewCameraTarget;

	public Mode mode { get; private set; }

	protected Key_class keyLeft, keyRight, keyUp, keyDown, keyJump;
	protected Key_class keyTool1, keyTool2, keyTool3, keyPick, keyMenu;
	protected RobotStatePanel statePanel;

	protected virtual void Start()
	{
		//DontDestroyOnLoad(gameObject);

		localizedName = name; //TEMP
		
		timesBeenUnused = UNUSED_LOADS_LIMIT;

		mode = Mode.MENU;

		keyLeft.Setup();
		keyRight.Setup();
		keyUp.Setup();
		keyDown.Setup();
		keyJump.Setup();
		keyTool1.Setup();
		keyTool2.Setup();
		keyTool3.Setup();
		keyPick.Setup();
		keyMenu.Setup();
	}

	int refresh_rate = 0;
	protected virtual void Update()
	{
		switch (mode)
		{
			case Mode.MENU: MenuUpdate(); break;
			case Mode.GAME: GameUpdate(); break;
			case Mode.MIXED: MixedUpdate(); break;
			case Mode.TEST: TestUpdate(); break;
			case Mode.VIEW: ViewUpdate(); break;
		}
	}

	private void MenuUpdate()
	{
		
	}

	private void GameUpdate()
	{
		if (!unit) { return; }
		if (!unit.isInitialized) { return; }

		if (keyMenu.action == Key_class.Action.PRESS)
		{
			if (statePanel.InMenu)
			{
				statePanel.InactivateMenu();
				SetMode(Mode.GAME);
			}
			else
			{
				statePanel.ActivateMenu();
				SetMode(Mode.MIXED);
			}

			visual.selector.SetArea(statePanel.activeMenu);
		}

		if (unit.tool1) { unit.tool1.React(keyTool1); }
		if (unit.tool2) { unit.tool2.React(keyTool2); }
		if (unit.tool3) { unit.tool3.React(keyTool3); }

		unit.JumpReact(keyJump);
		unit.PickReact(keyPick);
	}

	private void MixedUpdate()
	{
		if (keyMenu.action == Key_class.Action.PRESS)
		{
			if (statePanel.InMenu)
			{
				statePanel.InactivateMenu();
				SetMode(Mode.GAME);
			}
			else
			{
				statePanel.ActivateMenu();
				SetMode(Mode.MIXED);
			}

			visual.selector.SetArea(statePanel.activeMenu);

			return;
		}

		if (statePanel.menuButton1 && keyTool1.action == Key_class.Action.PRESS)
		{
			if (statePanel.menuButton1 == visual.selector.target)
			{
				statePanel.menuButton1.Click(this);
			}
			else
			{
				visual.selector.SetTarget(statePanel.menuButton1);
			}

			return;
		}

		if (statePanel.menuButton2 && keyTool2.action == Key_class.Action.PRESS)
		{
			if (statePanel.menuButton2 == visual.selector.target)
			{
				statePanel.menuButton2.Click(this);
			}
			else
			{
				visual.selector.SetTarget(statePanel.menuButton2);
			}

			return;
		}

		if (statePanel.menuButton3 && keyTool3.action == Key_class.Action.PRESS)
		{
			if (statePanel.menuButton3 == visual.selector.target)
			{
				statePanel.menuButton3.Click(this);
			}
			else
			{
				visual.selector.SetTarget(statePanel.menuButton3);
			}

			return;
		}

		if (statePanel.menuButton4 && keyPick.action == Key_class.Action.PRESS)
		{
			if (statePanel.menuButton4 == visual.selector.target)
			{
				statePanel.menuButton4.Click(this);
			}
			else
			{
				visual.selector.SetTarget(statePanel.menuButton4);
			}

			return;
		}
	}

	private void TestUpdate()
	{
		//special reaction on menu button, all other buttons have normal behavior

		if (unit.tool1) { unit.tool1.React(keyTool1); }
		if (unit.tool2) { unit.tool2.React(keyTool2); }
		if (unit.tool3) { unit.tool3.React(keyTool3); }

		unit.JumpReact(keyJump);
		unit.PickReact(keyPick);
	}

	private void ViewUpdate()
	{
		//depends on controller movement type
	}

	public void SetPanel(RobotStatePanel panel)
	{
		statePanel = panel;
	}

	public void SetMode(Mode newMode)
	{
		mode = newMode;
	}
}