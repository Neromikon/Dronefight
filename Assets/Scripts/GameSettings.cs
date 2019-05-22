using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

struct Savedata
{
	public struct ControllerSavedata
	{
		public bool isAvailable;
		public string name;
		public KeyCode up, down, left, right, tool1, tool2, tool3, pick, jump, menu;
		public int layout;
		public int timesBeenUnused;
		public string axis1x, axis1y, axis2x, axis2y;
		public Vector2 axis1Scale, axis2Scale;
		public string visualName;
	}

	public bool isFirstRun;
	public SystemLanguage language;
	public bool soundOn, musicOn;
	public float soundVolume, musicVolume;
	public float UItransparency;
	public string[] controllersSavedata;
	public GameSettings.Quality modelQuality;
	public GameSettings.Quality textureQuality;
}


public static class GameSettings
{
	public enum Quality { LOW, MEDIUM, HIGH }

	private const string SETTINGS_FILE = "settings.json";

	public static bool isInitialized = false;
	public static bool areGamepadsSupported;
	public static bool isTouchscreenSupported;

	public static GameInstance gameInstance;
	
	public static Canvas canvas;
	public static InitMenu initMenu;
	public static MainMenu mainMenu;
	public static PlayMenu playMenu;
	public static OptionsMenu optionsMenu;
	public static InfoMenu infoMenu;
	public static GameMenu gameMenu;
	public static Transform visualSchemesCollection;

	public static ControllerKeyboard keyboardController1;
	public static ControllerKeyboard keyboardController2;
	public static ControllerMouse mouseController;
	public static ControllerGamepad[] gamepadControllers;
	public static ControllerTouchscreen touchscreenController1;
	public static ControllerTouchscreen touchscreenController2;
	public static ControllerAlgorithm[] algorithmControllers;
	public static ControllerNeural[] neuralControllers;
	
	public static bool isFirstRun = true;
	public static SystemLanguage language = SystemLanguage.English;
	public static bool soundOn = true, musicOn = true;
	public static float soundVolume = 0.5f, musicVolume = 0.5f;
	public static float UItransparency = 0.5f;
	public static Quality modelQuality = Quality.MEDIUM;
	public static Quality textureQuality = Quality.MEDIUM;

	public static Controller mainPlayer;
	public static List<Controller> allPlayers = new List<Controller>(10);
	public static List<Controller> localPlayers = new List<Controller>(10);
	public static List<Controller> remotePlayers = new List<Controller>(10);
	public static List<Controller> botPlayers = new List<Controller>(10);
	public static List<Controller> activePlayers = new List<Controller>(10);
	public static List<ControllerKeyboard> keyboardPlayers = new List<ControllerKeyboard>(4);
	public static List<ControllerGamepad> joystickPlayers = new List<ControllerGamepad>(4);
	
	public static SceneDescription currentMap = null;
	public static GameMode gameMode = null;

	public static float loadingProgress { get; private set; }

	public static bool useDebugPlatform;
	public static RuntimePlatform debugPlatform;

	private static string[] scenes;
	
	private static List<PlayerVisualScheme> controllerVisuals = new List<PlayerVisualScheme>();

	private static string settingsFilePath = "";

	public static void Init ()
	{
		FindControllerVisualSchemes();
		FindScenes(); //<?> is that really needed?

		allPlayers.Add(keyboardController1);
		allPlayers.Add(keyboardController2);
		allPlayers.Add(mouseController);

		foreach (Controller player in gamepadControllers) { allPlayers.Add(player); }

		allPlayers.Add(touchscreenController1);
		allPlayers.Add(touchscreenController2);


		RuntimePlatform platform = Application.platform;

		#if DEBUG
		{
			if (useDebugPlatform) { platform = debugPlatform; }
		}
		#endif
		
		switch (platform)
		{
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.LinuxEditor:
			case RuntimePlatform.LinuxPlayer:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
			{
				settingsFilePath = SETTINGS_FILE;

				mainPlayer = keyboardController1;

				localPlayers.Add(keyboardController1);
				localPlayers.Add(keyboardController2);
				localPlayers.Add(mouseController);

				areGamepadsSupported = true;
				isTouchscreenSupported = false;

				FindGamepads();

				break;
			}

			case RuntimePlatform.Android:
			case RuntimePlatform.IPhonePlayer:
			{
				settingsFilePath = Application.persistentDataPath + "/" + SETTINGS_FILE;

				//if (!Input.touchSupported)
				//{
				//	//what to do?
				//	//use phone buttons?
				//	//TODO
				//	Debug.LogError("");
				//}

				mainPlayer = touchscreenController1;

				localPlayers.Add(touchscreenController1);
				localPlayers.Add(touchscreenController2);

				areGamepadsSupported = false;
				isTouchscreenSupported = true;

				break;
			}

			default:
			{
				Debug.LogError("Unsupported platform " + Application.platform.ToString());
				break;
			}
		}
		
		foreach (ControllerAlgorithm bot in algorithmControllers)
		{
			botPlayers.Add(bot);
			allPlayers.Add(bot);
		}

		foreach (ControllerNeural bot in neuralControllers)
		{
			botPlayers.Add(bot);
			allPlayers.Add(bot);
		}

		Load();

		foreach (Controller player in allPlayers)
		{
			if (!player.visual)
			{
				Debug.Log(player.name + " have no visual. Setting one now");

				TakeVisualScheme(player.controlType, ref player.visual);

				Debug.Assert(player.visual, "Found no visual scheme for " + player.name);
			}

			if (player.configPanel)
			{
				player.configPanel.SetOwner(player);
				player.configPanel.gameObject.SetActive(false);
			}
		}

		foreach (Controller player in localPlayers)
		{
			if (player.configPanel)
			{
				player.configPanel.gameObject.SetActive(true);
			}
		}

		Debug.Log(allPlayers.Count + " players in total");
		Debug.Log(localPlayers.Count + " local players");
		Debug.Log(remotePlayers.Count + " remote players");
		Debug.Log(botPlayers.Count + " bot players");
		Debug.Log(activePlayers.Count + " active players");

		SwitchToMenuMode();

		isInitialized = true;
	}

	private static void FindControllerVisualSchemes()
	{
		foreach (Transform child in visualSchemesCollection)
		{
			PlayerVisualScheme visualScheme = child.GetComponent<PlayerVisualScheme>();

			if (!visualScheme) { continue; }

			controllerVisuals.Add(visualScheme);
		}
	}

	private static void FindScenes()
	{
		scenes = new string[SceneManager.sceneCountInBuildSettings];

		for (int sceneIndex = 0; sceneIndex < scenes.Length; sceneIndex++)
		{
			scenes[sceneIndex] = SceneUtility.GetScenePathByBuildIndex(sceneIndex);

			if (sceneIndex == SceneManager.GetActiveScene().buildIndex) { continue; }
		}
	}

	private static void FindGamepads()
	{
		string[] joysticks = Input.GetJoystickNames();

		int currentGamepad = 0;

		foreach (string joystick in joysticks)
		{
			if (joystick.Length == 0) { continue; }

			Controller controller = gamepadControllers[currentGamepad];

			controller.name = joystick;

			currentGamepad++;
			if (currentGamepad > gamepadControllers.Length)
			{
				Debug.LogWarning("There are more gamepads plugged in than application supports");
				break;
			}
		}
	}

	public static void Save()
	{
		Savedata savedata;

		savedata.isFirstRun = isFirstRun;
		savedata.language = language;
		savedata.soundOn = soundOn;
		savedata.musicOn = musicOn;
		savedata.soundVolume = soundVolume;
		savedata.musicVolume = musicVolume;
		savedata.UItransparency = UItransparency;
		savedata.modelQuality = modelQuality;
		savedata.textureQuality = textureQuality;

		savedata.controllersSavedata = new string[allPlayers.Count];
		
		{
			uint i = 0;
			foreach (Controller controller in allPlayers)
			{
				Savedata.ControllerSavedata controllerSavedata;

				controllerSavedata.isAvailable = controller.isAvailable;
				controllerSavedata.name = controller.name;
				controllerSavedata.up = controller.up;
				controllerSavedata.down = controller.down;
				controllerSavedata.left = controller.left;
				controllerSavedata.right = controller.right;
				controllerSavedata.jump = controller.jump;
				controllerSavedata.tool1 = controller.tool1;
				controllerSavedata.tool2 = controller.tool2;
				controllerSavedata.tool3 = controller.tool3;
				controllerSavedata.pick = controller.pick;
				controllerSavedata.menu = controller.menu;
				controllerSavedata.timesBeenUnused = controller.timesBeenUnused;
				controllerSavedata.axis1x = controller.axis1x;
				controllerSavedata.axis1y = controller.axis1y;
				controllerSavedata.axis2x = controller.axis2x;
				controllerSavedata.axis2y = controller.axis2y;
				controllerSavedata.axis1Scale = controller.axis1Scale;
				controllerSavedata.axis2Scale = controller.axis2Scale;
				controllerSavedata.visualName = controller.visual.name;
				controllerSavedata.layout = controller.layout;

				savedata.controllersSavedata[i] = JsonUtility.ToJson(controllerSavedata);

				i++;
			}
		}

		File.WriteAllText(settingsFilePath, JsonUtility.ToJson(savedata));
	}

	private static void Load()
	{
		Debug.Log("Loading settings...");

		if (!File.Exists(settingsFilePath)) { return; }
		
		Savedata savedata = JsonUtility.FromJson<Savedata>(File.ReadAllText(settingsFilePath, System.Text.Encoding.ASCII));

		isFirstRun = savedata.isFirstRun;
		language = savedata.language;
		soundOn = savedata.soundOn;
		musicOn = savedata.musicOn;
		soundVolume = savedata.soundVolume;
		musicVolume = savedata.musicVolume;
		UItransparency = savedata.UItransparency;
		modelQuality = savedata.modelQuality;
		textureQuality = savedata.textureQuality;

		for (int i = 0; i < savedata.controllersSavedata.Length; i++)
		{
			Savedata.ControllerSavedata controllerSavedata = JsonUtility.FromJson<Savedata.ControllerSavedata>(savedata.controllersSavedata[i]);

			bool used = false;

			foreach (Controller controller in allPlayers)
			{
				if (controllerSavedata.name != controller.name) { continue; }
				
				controller.isAvailable = controllerSavedata.isAvailable;
				controller.up = controllerSavedata.up;
				controller.down = controllerSavedata.down;
				controller.left = controllerSavedata.left;
				controller.right = controllerSavedata.right;
				controller.jump = controllerSavedata.jump;
				controller.tool1 = controllerSavedata.tool1;
				controller.tool2 = controllerSavedata.tool2;
				controller.tool3 = controllerSavedata.tool3;
				controller.pick = controllerSavedata.pick;
				controller.menu = controllerSavedata.menu;
				controller.timesBeenUnused = controllerSavedata.timesBeenUnused;
				controller.axis1x = controllerSavedata.axis1x;
				controller.axis1y = controllerSavedata.axis1y;
				controller.axis2x = controllerSavedata.axis2x;
				controller.axis2y = controllerSavedata.axis2y;
				controller.axis1Scale = controllerSavedata.axis1Scale;
				controller.axis2Scale = controllerSavedata.axis2Scale;
				controller.visual = controllerVisuals.Find(visual => visual.name == controllerSavedata.visualName);
				controller.layout = controllerSavedata.layout;

				used = true;

				if (controller.visual)
				{
					controllerVisuals.Remove(controller.visual);
				}
				else
				{
					TakeVisualScheme(controller.controlType, ref controller.visual);
					Debug.Assert(controller.visual, "Found no visual scheme for " + controller.name);
				}

				break;
			}

			if (!used)
			{
				Debug.Log("");
				//todo, instantiate new controller based on its type
				//and reduce its unused counter by 1 if the counter
				//is not 0. otherwise just ignore that object and it
				//will not be saved next time
			}
		}

		Debug.Log("Settings successfully loaded");
	}

	private static IEnumerator AsyncSceneUnloading()
	{
		//AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentMap.name); //not working :(
		AsyncOperation unloadOperation = SceneManager.LoadSceneAsync("Menu");

		Debug.Assert(unloadOperation != null, "Failed to unload scene " + currentMap.name);

		while (!unloadOperation.isDone)
		{
			loadingProgress = unloadOperation.progress;
			yield return null;
		}
	}

	private static IEnumerator AsyncSceneLoading()
	{
		AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentMap.name);

		Debug.Assert(loadOperation != null, "Failed to load scene " + currentMap.name);

		while (!loadOperation.isDone)
		{
			loadingProgress = loadOperation.progress;
			yield return null;
		}

		gameMenu.SwitchMenu(GameMenu.Menu.PLAY);
	}

	public static void SwitchToMenuMode()
	{
		Debug.Assert(!isInitialized || gameMenu.isActiveAndEnabled, "Already in menu mode");

		initMenu.gameObject.SetActive(isFirstRun);
		mainMenu.gameObject.SetActive(!isFirstRun);
		playMenu.gameObject.SetActive(false);
		optionsMenu.gameObject.SetActive(false);
		infoMenu.gameObject.SetActive(false);
		gameMenu.gameObject.SetActive(false);

		if (currentMap)
		{
			gameInstance.StartCoroutine(AsyncSceneUnloading());
			Resources.UnloadUnusedAssets();
			currentMap = null;
		}

		foreach (Controller player in allPlayers)
		{
			player.SetPanel(null);
			player.SetMode(Controller.Mode.MENU);
			player.gameObject.SetActive(false);
		}
	}

	public static void SwitchToGameMode()
	{
		Debug.Assert(!gameMenu.isActiveAndEnabled, "Already in game mode");
		Debug.Assert(currentMap != null, "Can't start a game without map selected");

		mainMenu.gameObject.SetActive(false);
		playMenu.gameObject.SetActive(false);
		optionsMenu.gameObject.SetActive(false);
		infoMenu.gameObject.SetActive(false);
		gameMenu.gameObject.SetActive(true);

		gameInstance.StartCoroutine(AsyncSceneLoading());

		gameMenu.SwitchMenu(GameMenu.Menu.LOAD);

		foreach (Controller player in activePlayers)
		{
			player.gameObject.SetActive(true);

			if (player.type != Controller.PlayerType.LOCAL) { continue; }
			
			player.SetMode(Controller.Mode.GAME);
		}

		gameMenu.DisableVirtualControls();

		if (touchscreenController1.unitPrefab) { gameMenu.EnableVirtualControl(touchscreenController1); }
		if (touchscreenController2.unitPrefab) { gameMenu.EnableVirtualControl(touchscreenController2); }
	}

	public static void TakeVisualScheme(Controller.ControlType type, ref PlayerVisualScheme visualScheme)
	{
		List<PlayerVisualScheme> filteredSchemes = controllerVisuals.FindAll(scheme => scheme.type == type);

		if (filteredSchemes.Count == 0) { return; }

		if (visualScheme)
		{
			controllerVisuals.Add(visualScheme);
		}

		visualScheme = filteredSchemes[Random.Range(0, filteredSchemes.Count)];
		controllerVisuals.Remove(visualScheme);
	}

	public static bool CheckGamepads()
	{
		Debug.Assert(false, "Gamepad checking function is not finished yet");

		//todo: remove gamepads that were plugged out

		bool newGamepadsFound = false;

		string[] joysticks = Input.GetJoystickNames();

		foreach (string joystick in joysticks)
		{
			if (joystick.Length == 0) { continue; }

			bool alreadyKnown = false;

			foreach (Controller player in joystickPlayers)
			{
				if (player.name == joystick)
				{
					newGamepadsFound = true;
					alreadyKnown = true;
					break;
				}
			}

			if (!alreadyKnown)
			{
				newGamepadsFound = true;
				//TODO: register this new gamepad
			}
		}

		return newGamepadsFound;
	}

	public static bool IsLanguageSupported(SystemLanguage language)
	{
		switch (language)
		{
			case SystemLanguage.Chinese:
			case SystemLanguage.English:
			case SystemLanguage.German:
			case SystemLanguage.Greek:
			case SystemLanguage.Russian:
			case SystemLanguage.Ukrainian:
			{
				return true;
			}

			default: return false;
		}
	}
}
