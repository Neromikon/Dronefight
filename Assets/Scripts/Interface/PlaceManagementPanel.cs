using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceManagementPanel : MonoBehaviour
{
	public enum Menu { INFO, CONTROLLER, ROBOT }

	public const float TEAM_BUTTONS_LAYOUT_ANGLE_4_PLAYERS = 0.25f; //units
	public const float TEAM_BUTTONS_LAYOUT_ANGLE_6_PLAYERS = 0.30f; //units
	public const float TEAM_BUTTONS_LAYOUT_ANGLE_8_PLAYERS = 0.35f; //units
	public const float TEAM_BUTTONS_LAYOUT_ANGLE_10_PLAYERS = 0.40f; //units

	public Menu currentMenu { get; private set; }

	public Sprite randomRobotNeutralSprite;
	public Sprite randomRobotBlueSprite;
	public Sprite randomRobotRedSprite;
	public Sprite randomControllerSprite;
	public Sprite emptyPlaceSprite;

	public PlayMenu playMenu;
	public GameObject placesPanel;
	public GameObject infoPanel;
	public GameObject controllerPanel;
	public GameObject robotPanel;
	public Button backButton;
	public Button randomButton;
	public Image mapInfoPreviewImage;
	public Image mapInfoModeIcon;

	public RectTransform duelLayout;
	public RectTransform ffaLayout;
	public RectTransform teamLayout;

	public PlayerPlaceButton[] playerPlaceButtons;
	public ViewerPlaceButton[] viewerPlaceButtons;
	public BotButton[] botButtons;
	public PublicRobotButton[] robotButtons;

	public bool HaveFreeNeutralPlaces { get; private set; }
	public bool HaveFreeBluePlaces { get; private set; }
	public bool HaveFreeRedPlaces { get; private set; }
	public bool HaveFreeViewerPlaces { get; private set; }
	public bool HaveFreeTrainerPlaces { get; private set; }

	private GameMode currentMode = null;
	private PlayerPlaceButton selectedPlace;

	private void Start()
	{
		backButton.onClick.AddListener(OnBackClick);
		randomButton.onClick.AddListener(OnRandomClick);

		foreach (PlayerPlaceButton button in playerPlaceButtons)
		{
			button.buttonComponent.onClick.AddListener(() => OnPlaceSelect(button));
		}

		foreach (BotButton button in botButtons)
		{
			button.buttonComponent.onClick.AddListener(() => OnControllerSelect(button.owner));
		}

		foreach (PublicRobotButton button in robotButtons)
		{
			button.buttonComponent.onClick.AddListener(() => OnRobotSelect(button.robotPrefab));
		}
	}

	public void SwitchMenu(Menu menu)
	{
		currentMenu = menu;

		placesPanel.SetActive(menu == Menu.INFO);
		infoPanel.SetActive(menu == Menu.INFO);
		mapInfoPreviewImage.gameObject.SetActive(menu == Menu.INFO);
		controllerPanel.SetActive(menu == Menu.CONTROLLER);
		robotPanel.SetActive(menu == Menu.ROBOT);

		switch (menu)
		{
			case Menu.INFO:
			{
				selectedPlace = null;
				mapInfoPreviewImage.sprite = GameSettings.currentMap.preview;
				mapInfoModeIcon.sprite = GameSettings.gameMode.icon;

				if (currentMode == GameSettings.gameMode)
				{
					RefreshPlaces();
				}
				else
				{
					SwitchMode(GameSettings.gameMode);
					RefreshPlaces();
				}

				break;
			}

			case Menu.CONTROLLER:
			{
				selectedPlace.owner = null;

				foreach (BotButton button in botButtons)
				{
					button.gameObject.SetActive(button.owner.unitPrefab == null);
				}

				break;
			}
		}
	}

	public void SwitchMode(GameMode mode)
	{
		currentMode = mode;

		switch (mode.type)
		{
			case GameMode.Type.SOLO: SetupSoloLayout(); break;
			case GameMode.Type.DUEL: SetupDuelLayout(); break;

			case GameMode.Type.FFA3:
			case GameMode.Type.FFA4:
			case GameMode.Type.FFA5:
			case GameMode.Type.FFA6:
			case GameMode.Type.FFA7:
			case GameMode.Type.FFA8:
			case GameMode.Type.FFA9:
			case GameMode.Type.FFA10:
			{
				SetupDeathmatchLayout(mode);
				break;
			}

			case GameMode.Type.TEAM2x2: SetupTeamLayout(mode, TEAM_BUTTONS_LAYOUT_ANGLE_4_PLAYERS); break;
			case GameMode.Type.TEAM3x3: SetupTeamLayout(mode, TEAM_BUTTONS_LAYOUT_ANGLE_6_PLAYERS); break;
			case GameMode.Type.TEAM4x4: SetupTeamLayout(mode, TEAM_BUTTONS_LAYOUT_ANGLE_8_PLAYERS); break;
			case GameMode.Type.TEAM5x5: SetupTeamLayout(mode, TEAM_BUTTONS_LAYOUT_ANGLE_10_PLAYERS); break;
		}
	}

	private void SetupSoloLayout()
	{
		playerPlaceButtons[0].gameObject.SetActive(true);
		playerPlaceButtons[0].transform.SetParent(duelLayout.transform);
		playerPlaceButtons[0].team = Controller.Team.NEUTRAL;

		for (int i = 1; i < playerPlaceButtons.Length; i++)
		{
			playerPlaceButtons[i].gameObject.SetActive(false);
		}
	}

	private void SetupDuelLayout()
	{
		playerPlaceButtons[0].gameObject.SetActive(true);
		playerPlaceButtons[0].transform.SetParent(duelLayout.transform);
		playerPlaceButtons[0].team = Controller.Team.NEUTRAL;

		playerPlaceButtons[1].gameObject.SetActive(true);
		playerPlaceButtons[1].transform.SetParent(duelLayout.transform);
		playerPlaceButtons[1].team = Controller.Team.NEUTRAL;

		for (int i = 2; i < playerPlaceButtons.Length; i++)
		{
			playerPlaceButtons[i].gameObject.SetActive(false);
		}
	}

	private void SetupDeathmatchLayout(GameMode mode)
	{
		RectTransform placeButtonRectTransform = playerPlaceButtons[0].GetComponent<RectTransform>();
		Vector2 ellipseSize = ffaLayout.rect.size - placeButtonRectTransform.sizeDelta;

		for (int i = 0; i < mode.playersCount; i++)
		{
			playerPlaceButtons[i].gameObject.SetActive(true);
			playerPlaceButtons[i].transform.SetParent(ffaLayout);
			playerPlaceButtons[i].team = Controller.Team.NEUTRAL;

			float unitAngle = 0.25f + (float)i / (float)mode.playersCount;

			playerPlaceButtons[i].transform.localPosition = Support.CirclePoint(unitAngle, ellipseSize);
		}

		for (int i = mode.playersCount; i < playerPlaceButtons.Length; i++)
		{
			playerPlaceButtons[i].gameObject.SetActive(false);
		}
	}

	private void SetupTeamLayout(GameMode mode, float unitAnglePerTeam)
	{
		for (int i = 0; i < mode.playersCount; i++)
		{
			playerPlaceButtons[i].gameObject.SetActive(true);
			playerPlaceButtons[i].transform.SetParent(teamLayout);
		}

		int playersPerTeam = mode.playersCount / 2;
		float deltaAngle = unitAnglePerTeam / (playersPerTeam - 1);
		RectTransform placeButtonRectTransform = playerPlaceButtons[0].GetComponent<RectTransform>();
		Vector2 ellipseSize = ffaLayout.rect.size - placeButtonRectTransform.sizeDelta;

		for (int i = 0; i < playersPerTeam; i++)
		{
			{
				float unitAngle = -unitAnglePerTeam * 0.5f + deltaAngle * i;

				playerPlaceButtons[i].transform.localPosition = Support.CirclePoint(unitAngle, ellipseSize);
				playerPlaceButtons[i].team = Controller.Team.RED;

				ColorBlock colorBlock = playerPlaceButtons[i].buttonComponent.colors;
				colorBlock.normalColor = Color.blue;
			}

			{
				float unitAngle = 0.5f + unitAnglePerTeam * 0.5f - deltaAngle * i;

				playerPlaceButtons[i + playersPerTeam].transform.localPosition = Support.CirclePoint(unitAngle, ellipseSize);
				playerPlaceButtons[i + playersPerTeam].team = Controller.Team.BLUE;

				ColorBlock colorBlock = playerPlaceButtons[i + playersPerTeam].buttonComponent.colors;
				colorBlock.normalColor = Color.red;
			}
		}

		for (int i = mode.playersCount; i < playerPlaceButtons.Length; i++)
		{
			playerPlaceButtons[i].gameObject.SetActive(false);
		}
	}

	public void AddPlayer(Controller player, Controller.Team team)
	{
		player.team = team;

		switch (team)
		{
			case Controller.Team.NEUTRAL:
			case Controller.Team.BLUE:
			case Controller.Team.RED:
			{
				foreach (PlayerPlaceButton place in playerPlaceButtons)
				{
					if (!place.gameObject.activeSelf) { continue; }
					if (place.owner) { continue; }
					if (place.team != team) { continue; }

					place.owner = player;
					break;
				}

				break;
			}

			case Controller.Team.VIEWERS:
			{
				foreach (ViewerPlaceButton place in viewerPlaceButtons)
				{
					if (!place.gameObject.activeSelf) { continue; }
					if (place.owner) { continue; }

					place.owner = player;
					break;
				}

				break;
			}

			case Controller.Team.TRAINERS:
			{
				Debug.LogError("TODO: trainers joining is not supported yet");

				break;
			}
		}

		RefreshPlaces();
	}

	public void ResetPlaces()
	{
		foreach (ViewerPlaceButton button in viewerPlaceButtons)
		{
			button.owner = null;
		}

		foreach (PlayerPlaceButton button in playerPlaceButtons)
		{
			button.owner = null;
		}

		//RefreshPlaces(); //to prevent double refreshing, this function for now is used only in PlayMenu.SwitchMenu(JOIN)
	}

	public void RefreshPlaces()
	{
		foreach (ViewerPlaceButton button in viewerPlaceButtons)
		{
			if (button.owner)
			{
				button.nameText.text = button.owner.localizedName;
				button.playerControllerIcon.sprite = button.owner.visual.icon;
			}
			else
			{
				button.nameText.text = string.Empty;
				button.playerControllerIcon.sprite = emptyPlaceSprite;
			}
		}

		foreach (PlayerPlaceButton button in playerPlaceButtons)
		{
			button.trainerControllerIcon.gameObject.SetActive(false); //todo, not implemented yet

			if (button.owner)
			{
				button.nameText.text = button.owner.localizedName;
				button.playerControllerIcon.sprite = button.owner.visual.icon;
			}
			else
			{
				button.nameText.text = string.Empty;
				button.playerControllerIcon.sprite = randomControllerSprite;
			}

			if (button.owner && button.owner.unitPrefab)
			{
				button.buttonComponent.image.sprite = button.owner.unitPrefab.avatar;
			}
			else
			{
				switch (button.team)
				{
					case Controller.Team.NEUTRAL: button.buttonComponent.image.sprite = randomRobotNeutralSprite; break;
					case Controller.Team.BLUE: button.buttonComponent.image.sprite = randomRobotBlueSprite; break;
					case Controller.Team.RED: button.buttonComponent.image.sprite = randomRobotRedSprite; break;
				}
			}
		}

		HaveFreeNeutralPlaces = false;
		HaveFreeBluePlaces = false;
		HaveFreeRedPlaces = false;
		HaveFreeViewerPlaces = false;
		HaveFreeTrainerPlaces = false;

		foreach (PlayerPlaceButton place in playerPlaceButtons)
		{
			if (place.owner) { continue; }
			if (place.team != Controller.Team.NEUTRAL) { continue; }

			HaveFreeNeutralPlaces = true;
			break;
		}

		foreach (PlayerPlaceButton place in playerPlaceButtons)
		{
			if (place.owner) { continue; }
			if (place.team != Controller.Team.BLUE) { continue; }

			HaveFreeBluePlaces = true;
			break;
		}

		foreach (PlayerPlaceButton place in playerPlaceButtons)
		{
			if (place.owner) { continue; }
			if (place.team != Controller.Team.RED) { continue; }

			HaveFreeRedPlaces = true;
			break;
		}

		foreach (ViewerPlaceButton place in viewerPlaceButtons)
		{
			if (place.owner) { continue; }

			HaveFreeViewerPlaces = true;
			break;
		}

		foreach (PlayerPlaceButton place in playerPlaceButtons)
		{
			if (!place.owner) { continue; }
			if (place.owner.controlType != Controller.ControlType.NEURAL_NETWORK) { continue; }
			if (place.trainer) { continue; }

			HaveFreeTrainerPlaces = true;
			break;
		}
	}

	public void FillFreePlaces()
	{
		List<Controller> availableBots = new List<Controller>(GameSettings.botPlayers.Count);

		foreach (Controller bot in GameSettings.botPlayers)
		{
			if (bot.unitPrefab) { continue; }

			availableBots.Add(bot);
		}

		foreach (PlayerPlaceButton place in playerPlaceButtons)
		{
			if (place.owner) { continue; }

			int randomBotIndex = Random.Range(0, availableBots.Count);

			place.owner = availableBots[randomBotIndex];

			availableBots.RemoveAt(randomBotIndex);

			place.owner.unitPrefab = robotButtons[Random.Range(0, robotButtons.Length)].robotPrefab;
		}
	}

	public List<Controller> GetActiveControllers()
	{
		List<Controller> result = new List<Controller>(playerPlaceButtons.Length);

		foreach (PlayerPlaceButton place in playerPlaceButtons)
		{
			if (!place.gameObject.activeSelf) { continue; }
			if (!place.owner) { continue; }

			result.Add(place.owner);
		}

		return result;
	}

	private void OnBackClick()
	{
		switch (currentMenu)
		{
			case Menu.INFO: Debug.LogError("Back button have to be disabled at info menu"); break;

			case Menu.CONTROLLER:
			{
				selectedPlace.owner = null;
				SwitchMenu(Menu.INFO);
				break;
			}

			case Menu.ROBOT:
			{
				selectedPlace.owner = GameSettings.mainPlayer;
				SwitchMenu(Menu.CONTROLLER);
				break;
			}
		}
	}

	private void OnRandomClick()
	{
		switch (currentMenu)
		{
			case Menu.INFO: Debug.LogError("Random button have to be disabled at info menu"); break;
			case Menu.CONTROLLER: botButtons[Random.Range(0, botButtons.Length)].buttonComponent.onClick.Invoke(); break;
			case Menu.ROBOT: robotButtons[Random.Range(0, robotButtons.Length)].buttonComponent.onClick.Invoke(); break;
		}
	}

	private void OnPlaceSelect(PlayerPlaceButton place)
	{
		selectedPlace = place;
		selectedPlace.owner = GameSettings.mainPlayer;
		playMenu.RefreshPlayerPanels();
		SwitchMenu(Menu.CONTROLLER);
	}

	private void OnControllerSelect(Controller controller)
	{
		selectedPlace.owner = controller;
		SwitchMenu(Menu.ROBOT);
	}

	private void OnRobotSelect(Unit unitPrefab)
	{
		selectedPlace.owner.unitPrefab = unitPrefab;
		SwitchMenu(Menu.INFO);
	}
}
