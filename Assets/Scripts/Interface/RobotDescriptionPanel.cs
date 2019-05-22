using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RobotDescriptionPanel : MonoBehaviour
{
	public enum Menu { DESCRIPTION, PLAYTEST, VIEW }

	public const float VIEW_HORIZONTAL_ROTATION_ANGULAR_SPEED = 25.0f; //degrees/second
	public const float VIEW_VERTICAL_ROTATION_ANGULAR_SPEED = -25.0f; //degrees/second

	public RobotStatePanel robotStatePanel;

	public GameObject descriptionBackground;
	public Text robotNameText;
	public Text robotDetailsText;

	public DragArea viewDragArea;
	
	public PublicRobotButton[] robotButtons;
	
	public GameObject viewOptionsLeftPanel;
	public GameObject viewOptionsRightPanel;

	public VirtualControl virtualControl;

	public Menu currentMenu { get; private set; }
	public Unit currentRobotPrefab { get; private set; }
	public Unit testingRobot { get; private set; }
	public Unit viewingRobot { get; private set; }

	private MenuScene menuScene;

    void Start()
    {
		for (int i = 0; i < robotButtons.Length; i++)
		{
			Unit randomRobo = robotButtons[i].robotPrefab;
			robotButtons[i].buttonComponent.onClick.AddListener(() => OnRobotSelect(randomRobo));
		}
		
		SwitchMenu(Menu.PLAYTEST);
    }
	
    void Update()
    {
		//if (transform.hasChanged)
		//{
		//	
		//}

		switch (currentMenu)
		{
			case Menu.VIEW: ViewUpdate(); break;
		}
    }

	private void ViewUpdate()
	{
		menuScene.viewCamera.transform.RotateAround(
			menuScene.viewSpawner.transform.position,
			menuScene.viewSpawner.transform.up,
			viewDragArea.delta.x * VIEW_HORIZONTAL_ROTATION_ANGULAR_SPEED * Time.deltaTime);
		
		menuScene.viewCamera.transform.RotateAround(
			menuScene.viewSpawner.transform.position,
			menuScene.viewCamera.transform.transform.right,
			viewDragArea.delta.y * VIEW_VERTICAL_ROTATION_ANGULAR_SPEED * Time.deltaTime);
	}

	private void OnEnable()
	{
		Scene activeScene = SceneManager.GetActiveScene();

		foreach (GameObject root in activeScene.GetRootGameObjects())
		{
			menuScene = root.GetComponent<MenuScene>();

			if (menuScene != null) { break; }
		}

		SwitchMenu(currentMenu);

		Debug.Assert(menuScene, "Testing scene object not found, info menu is not functional");
	}

	private void OnDisable()
	{
		if (GameSettings.mainPlayer)
		{
			GameSettings.mainPlayer.SetMode(Controller.Mode.MENU);

			OnRobotSelect(null);
		}

		menuScene.playtestCamera.gameObject.SetActive(false);
		menuScene.viewCamera.gameObject.SetActive(false);
		menuScene.backgroundCamera.gameObject.SetActive(true);
	}

	private void OnRobotSelect(Unit robotPrefab)
	{
		Debug.Log("RobotSelect of " + (robotPrefab ? robotPrefab.name : "null") + " called");
		
		menuScene.playtestCamera.ClearTargets();

		if (testingRobot) { Destroy(testingRobot.gameObject); }
		if (viewingRobot) { Destroy(viewingRobot.gameObject); }

		currentRobotPrefab = robotPrefab;

		if (robotPrefab)
		{
			testingRobot = menuScene.playtestSpawner.Spawn(robotPrefab);
			viewingRobot = menuScene.viewSpawner.Spawn(robotPrefab);

			GameSettings.mainPlayer.unitPrefab = robotPrefab;
			GameSettings.mainPlayer.unit = testingRobot;

			robotStatePanel.SetOwner(GameSettings.mainPlayer);

			menuScene.playtestCamera.AddTarget(testingRobot.gameObject);

			SwitchMenu(currentMenu);
		}
		else
		{
			GameSettings.mainPlayer.unit = null;
			GameSettings.mainPlayer.unitPrefab = null;
		}
	}

	public void SwitchMenu(Menu menu)
	{
		currentMenu = menu;

		descriptionBackground.SetActive(menu == Menu.DESCRIPTION);
		robotNameText.gameObject.SetActive(menu == Menu.DESCRIPTION);
		robotDetailsText.gameObject.SetActive(menu == Menu.DESCRIPTION);

		viewDragArea.gameObject.SetActive(menu == Menu.VIEW);
		viewOptionsLeftPanel.SetActive(menu == Menu.VIEW);
		viewOptionsRightPanel.SetActive(menu == Menu.VIEW);

		menuScene.backgroundCamera.gameObject.SetActive(menu != Menu.PLAYTEST && menu != Menu.VIEW);
		menuScene.playtestCamera.gameObject.SetActive(menu == Menu.PLAYTEST);
		menuScene.viewCamera.gameObject.SetActive(menu == Menu.VIEW);

		virtualControl.gameObject.SetActive(menu == Menu.PLAYTEST && GameSettings.isTouchscreenSupported);

		if (!currentRobotPrefab) { SelectRandomRobot(); }

		switch (menu)
		{
			case Menu.DESCRIPTION:
			{
				GameSettings.mainPlayer.SetMode(Controller.Mode.MENU);
				robotNameText.text = currentRobotPrefab.localizedName.Get(GameSettings.language);
				robotDetailsText.text = currentRobotPrefab.localizedDetails.Get(GameSettings.language);
				break;
			}

			case Menu.PLAYTEST:
			{
				Debug.Assert(GameSettings.mainPlayer.unit, "Main player unit is not linked to playtest robot");
				GameSettings.mainPlayer.SetMode(Controller.Mode.TEST);

				if (GameSettings.isTouchscreenSupported)
				{
					ControllerTouchscreen touchscreen = GameSettings.mainPlayer.GetComponent<ControllerTouchscreen>();
					if (touchscreen) { touchscreen.SetVirtualControl(virtualControl); }
				}

				break;
			}

			case Menu.VIEW:
			{
				GameSettings.mainPlayer.SetMode(Controller.Mode.VIEW);

				GameSettings.mainPlayer.controllingCamera = menuScene.viewCamera;
				GameSettings.mainPlayer.viewCameraTarget = menuScene.viewSpawner.transform;

				break;
			}
		}
	}

	private void SelectRandomRobot()
	{
		OnRobotSelect(robotButtons[Random.Range(0, robotButtons.Length)].robotPrefab);
	}
}
