using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


// Note: I really-really wanted a static class (I prefer it instead of singletone)
// for global game control but Unity does not support (yet?) setting static
// variables from editor so I had to make those instance to transfer all variables
// to static class. Instance variable is not used mostly just for syntax shortage
// (i.e. "GameSettings.variable" instead of "GameSettings.gameInstance.variable")


public class GameInstance : MonoBehaviour
{
	public bool useDebugPlatform = false;
	public RuntimePlatform debugPlatform;

	public Canvas canvas;
	public InitMenu initMenu;
	public MainMenu mainMenu;
	public PlayMenu playMenu;
	public OptionsMenu optionsMenu;
	public InfoMenu infoMenu;
	public GameMenu gameMenu;
	public Transform visualSchemesCollection;

	public ControllerKeyboard keyboardController1;
	public ControllerKeyboard keyboardController2;
	public ControllerMouse mouseController;
	public ControllerGamepad[] gamepadControllers;
	public ControllerTouchscreen touchscreenController1;
	public ControllerTouchscreen touchscreenController2;
	public ControllerAlgorithm[] algorithmControllers;
	public ControllerNeural[] neuralControllers;

	private void Start()
	{
		if (GameSettings.isInitialized)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		GameSettings.gameInstance = this;

		GameSettings.useDebugPlatform = useDebugPlatform;
		GameSettings.debugPlatform = debugPlatform;

		GameSettings.canvas = canvas;
		GameSettings.initMenu = initMenu;
		GameSettings.mainMenu = mainMenu;
		GameSettings.playMenu = playMenu;
		GameSettings.optionsMenu = optionsMenu;
		GameSettings.infoMenu = infoMenu;
		GameSettings.gameMenu = gameMenu;
		GameSettings.visualSchemesCollection = visualSchemesCollection;

		GameSettings.keyboardController1 = keyboardController1;
		GameSettings.keyboardController2 = keyboardController2;
		GameSettings.mouseController = mouseController;
		GameSettings.gamepadControllers = gamepadControllers;
		GameSettings.touchscreenController1 = touchscreenController1;
		GameSettings.touchscreenController2 = touchscreenController2;
		GameSettings.algorithmControllers = algorithmControllers;
		GameSettings.neuralControllers = neuralControllers;

		GameSettings.Init();
	}
}