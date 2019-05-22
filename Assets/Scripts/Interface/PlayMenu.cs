using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
	public enum Menu { MAP, MODE, JOIN }

	private const float ORIENTATION_CHANGE_COEFFICIENT = 1.35f;

	public Menu currentMenu { get; private set; }

	public RectTransform rectTransform;

	public Sprite backToMainMenuSprite;
	public Sprite backToPreviousStepSprite;
	public SceneDescription[] maps;

	public MainMenu mainMenu;
	public GameObject mapSelectionPanel;
	public GameObject modeSelectionPanel;
	public GameObject localPlayersPanel;
	public PlaceManagementPanel placeManagementPanel;

	public Button startButton;
	public Button backButton;
	public Button randomButton;
	public Image mapModePreviewImage;
	public MapSelectionButton[] mapButtons;
	public ModeSelectionButton[] modeButtons;
	public PlayerPanel[] playerPanels;

	//public Button soloModeButton;
	//public Button dualModeButton;
	//public Button ffa3ModeButton;
	//public Button ffa4ModeButton;
	//public Button ffa5ModeButton;
	//public Button ffa6ModeButton;
	public Button ffa7ModeButton;
	public Button ffa8ModeButton;
	public Button ffa9ModeButton;
	public Button ffa10ModeButton;
	//public Button team2x2ModeButton;
	//public Button team3x3ModeButton;
	//public Button team4x4ModeButton;
	//public Button team5x5ModeButton;

	public Transform ffaLayout1;
	public Transform ffaLayout2;


	void Start ()
	{
		startButton.onClick.AddListener(OnStartClick);
		backButton.onClick.AddListener(OnBackClick);
		randomButton.onClick.AddListener(OnRandomClick);

		foreach (MapSelectionButton button in mapButtons)
		{
			Button buttonComponent = button.GetComponent<Button>();

			buttonComponent.image.sprite = button.map.preview;
			buttonComponent.onClick.AddListener(() => OnMapSelect(button.map));
		}

		foreach (ModeSelectionButton button in modeButtons)
		{
			button.buttonComponent.image.sprite = button.mode.icon;
			button.buttonComponent.onClick.AddListener(() => OnModeSelect(button.mode));
		}

		SwitchMenu(Menu.MAP);
	}

	private void Update()
	{
		if (rectTransform.hasChanged) { OnOrientationChange(); }
	}

	public void SwitchMenu(Menu menu)
	{
		currentMenu = menu;

		startButton.gameObject.SetActive(menu == Menu.JOIN);
		mapSelectionPanel.SetActive(menu == Menu.MAP);
		modeSelectionPanel.SetActive(menu == Menu.MODE);
		mapModePreviewImage.gameObject.SetActive(menu == Menu.MODE);
		localPlayersPanel.SetActive(menu == Menu.JOIN);
		placeManagementPanel.gameObject.SetActive(menu == Menu.JOIN);

		randomButton.gameObject.SetActive(menu == Menu.MAP || menu == Menu.MODE);

		if (menu == Menu.MAP)
		{
			backButton.image.sprite = backToMainMenuSprite;
		}
		else
		{
			backButton.image.sprite = backToPreviousStepSprite;
		}

		switch (menu)
		{
			case Menu.MAP:
			{
				GameSettings.currentMap = null;
				break;
			}

			case Menu.MODE:
			{
				mapModePreviewImage.sprite = GameSettings.currentMap.preview;

				foreach (ModeSelectionButton button in modeButtons)
				{
					button.buttonComponent.interactable = GameSettings.currentMap.IsModeEnabled(button.mode.type);
				}

				break;
			}

			case Menu.JOIN:
			{
				foreach (Controller player in GameSettings.allPlayers)
				{
					player.gameObject.SetActive(true);
					player.unitPrefab = null;
				}

				placeManagementPanel.ResetPlaces();
				placeManagementPanel.SwitchMenu(PlaceManagementPanel.Menu.INFO);
				SetupPlayerPanels();
				break;
			}
		}
	}

	private void OnOrientationChange()
	{
		rectTransform.hasChanged = false;

		if (rectTransform.rect.width >= rectTransform.rect.height * ORIENTATION_CHANGE_COEFFICIENT)
		{
			ffaLayout2.gameObject.SetActive(false);
			ffa7ModeButton.transform.SetParent(ffaLayout1);
			ffa8ModeButton.transform.SetParent(ffaLayout1);
			ffa9ModeButton.transform.SetParent(ffaLayout1);
			ffa10ModeButton.transform.SetParent(ffaLayout1);
		}
		else
		{
			ffaLayout2.gameObject.SetActive(true);
			ffa7ModeButton.transform.SetParent(ffaLayout2);
			ffa8ModeButton.transform.SetParent(ffaLayout2);
			ffa9ModeButton.transform.SetParent(ffaLayout2);
			ffa10ModeButton.transform.SetParent(ffaLayout2);
		}
	}

	public void SetupPlayerPanels()
	{
		int availableLocalPlayers = 0;

		foreach (Controller player in GameSettings.localPlayers)
		{
			if (player.isAvailable) { availableLocalPlayers++; }
		}

		Debug.Assert(availableLocalPlayers >= 1, "Must be at least one player!");
		Debug.Assert(availableLocalPlayers <= 4, "Maximum 4 local players supported");

		foreach (PlayerPanel panel in playerPanels)
		{
			if (panel.forNumberOfPlayers == availableLocalPlayers)
			{
				panel.gameObject.SetActive(true);

				if (panel.playerIndex < availableLocalPlayers)
				{
					panel.SetOwner(GameSettings.localPlayers[panel.playerIndex]);
				}

				panel.SwitchMenu(PlayerPanel.Menu.PLACE);
			}
			else
			{
				panel.gameObject.SetActive(false);
			}
		}
	}

	public void RefreshPlayerPanels()
	{
		foreach (PlayerPanel panel in playerPanels)
		{
			panel.SwitchMenu(panel.currentMenu); //refresh
		}
	}

	public void OnStartClick()
	{
		placeManagementPanel.FillFreePlaces();
		GameSettings.activePlayers = placeManagementPanel.GetActiveControllers();
		GameSettings.SwitchToGameMode();
	}

	public void OnBackClick()
	{
		switch (currentMenu)
		{
			case Menu.MAP:
			{
				gameObject.SetActive(false);
				mainMenu.gameObject.SetActive(true);
				break;
			}

			case Menu.MODE: SwitchMenu(Menu.MAP); break;
			case Menu.JOIN: SwitchMenu(Menu.MODE); break;
		}
	}

	private void OnRandomClick()
	{
		switch (currentMenu)
		{
			case Menu.MAP: OnMapSelect(maps[Random.Range(0, maps.Length)]); break;
			case Menu.MODE: OnModeSelect(GameSettings.currentMap.GetRandomMode()); break;
			case Menu.JOIN: Debug.LogError("Random select button have to be disabled at joining stage"); break;
		}
	}

	private void OnMapSelect(SceneDescription selectedMap)
	{
		GameSettings.currentMap = selectedMap;
		SwitchMenu(Menu.MODE);
	}

	private void OnModeSelect(GameMode selectedMode)
	{
		GameSettings.gameMode = selectedMode;
		SwitchMenu(Menu.JOIN);
	}
}
