using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
	public enum Menu { GAME, CONTROLS, BOTS, LANGUAGE }
	
	public Sprite soundOnSprite, soundOffSprite;
	public Sprite musicOnSprite, musicOffSprite;

	public GameObject transparencyPanel;

	public Button backButton;
	public Button gameOptionsButton;
	public Button controlsOptionsButton;
	public Button botsOptionsButton;
	public Button languageOptionsButton;
	public Button gamepadsCheckButton;
	
	public MainMenu mainMenu;
	public GameObject gameOptionsPanel;
	public GameObject controlsOptionsPanel;
	public GameObject botsOptionsPanel;
	public GameObject languageOptionsPanel;
	public Button soundButton;
	public Slider soundSlider;
	public Button musicButton;
	public Slider musicSlider;
	public Slider transparencySlider;
	public GridLayoutGroup controlsPanelGrid;
	public Button lowModelsButton, mediumModelsButton, highModelsButton;
	public Button lowTexturesButton, mediumTexturesButton, highTexturesButton;

	public Transform languageButtonsDirectParent;

	public Image demoVirtualJoystickArea;
	public Image demoVirtualJoystickButton;

	private Menu currentMenu;

	void Start ()
	{
		//transparencyPanel.SetActive(GameSettings.isTouchscreenSupported); //DISABLED TEMPORARY, TEST

		backButton.onClick.AddListener(OnBackClick);
		gameOptionsButton.onClick.AddListener(OnGameOptionsClick);
		controlsOptionsButton.onClick.AddListener(OnControlsOptionsClick);
		languageOptionsButton.onClick.AddListener(OnLanguageOptionsClick);
		gamepadsCheckButton.onClick.AddListener(OnCheckGamepadsClick);
		
		soundButton.onClick.AddListener(OnSoundButtonClick);
		soundButton.image.sprite = GameSettings.soundOn ? soundOnSprite : soundOffSprite;
		
		musicButton.onClick.AddListener(OnMusicButtonClick);
		musicButton.image.sprite = GameSettings.musicOn ? musicOnSprite : musicOffSprite;
		
		soundSlider.onValueChanged.AddListener(OnSoundSliderMove);
		soundSlider.value = GameSettings.soundVolume;

		musicSlider.onValueChanged.AddListener(OnMusicSliderMove);
		musicSlider.value = GameSettings.musicVolume;

		transparencySlider.onValueChanged.AddListener(OnTransparencySliderMove);
		transparencySlider.value = GameSettings.UItransparency;
		SetDemoTransparency(GameSettings.UItransparency);

		lowModelsButton.onClick.AddListener(() => OnModelQualitySelect(GameSettings.Quality.LOW));
		mediumModelsButton.onClick.AddListener(() => OnModelQualitySelect(GameSettings.Quality.MEDIUM));
		highModelsButton.onClick.AddListener(() => OnModelQualitySelect(GameSettings.Quality.HIGH));

		lowTexturesButton.onClick.AddListener(() => OnTextureQualitySelect(GameSettings.Quality.LOW));
		mediumTexturesButton.onClick.AddListener(() => OnTextureQualitySelect(GameSettings.Quality.MEDIUM));
		highTexturesButton.onClick.AddListener(() => OnTextureQualitySelect(GameSettings.Quality.HIGH));

		RectTransform controlOptionsPanelRectTransform = controlsOptionsPanel.GetComponent<RectTransform>();

		foreach (Transform child in languageButtonsDirectParent)
		{
			LanguageButton languageButton = child.GetComponent<LanguageButton>();

			if (!languageButton) { continue; }

			if (GameSettings.IsLanguageSupported(languageButton.language))
			{
				languageButton.buttonComponent.image.color = Color.white;
				languageButton.buttonComponent.onClick.AddListener(() => ChangeLanguage(languageButton.language));
			}
			else
			{
				//languageButton.gameObject.SetActive(false);
				languageButton.buttonComponent.image.color = Color.yellow;
			}
		}

		Support.ShuffleChildrenOrder(languageButtonsDirectParent);

		SwitchMenu(Menu.GAME);
	}
	
	void Update ()
	{
		
	}

	public void SwitchMenu(Menu menu)
	{
		currentMenu = menu;

		gameOptionsPanel.SetActive(menu == Menu.GAME);
		controlsOptionsPanel.SetActive(menu == Menu.CONTROLS);
		botsOptionsPanel.SetActive(menu == Menu.BOTS);
		languageOptionsPanel.SetActive(menu == Menu.LANGUAGE);
	}

	private void OnBackClick()
	{
		gameObject.SetActive(false);
		mainMenu.gameObject.SetActive(true);
	}

	private void OnGameOptionsClick()
	{
		SwitchMenu(Menu.GAME);
	}

	private void OnControlsOptionsClick()
	{
		SwitchMenu(Menu.CONTROLS);
	}

	private void OnBotsOptionsClick()
	{
		SwitchMenu(Menu.BOTS);
	}

	private void OnLanguageOptionsClick()
	{
		SwitchMenu(Menu.LANGUAGE);
	}

	private void OnSoundButtonClick()
	{
		GameSettings.soundOn = !GameSettings.soundOn;
		soundButton.image.sprite = GameSettings.soundOn ? soundOnSprite : soundOffSprite;
		GameSettings.Save();
	}

	private void OnMusicButtonClick()
	{
		GameSettings.musicOn = !GameSettings.musicOn;
		musicButton.image.sprite = GameSettings.musicOn ? musicOnSprite : musicOffSprite;
		GameSettings.Save();
	}

	private void OnSoundSliderMove(float newValue)
	{
		GameSettings.soundVolume = newValue;
		GameSettings.Save();
	}

	private void OnMusicSliderMove(float newValue)
	{
		GameSettings.musicVolume = newValue;
		GameSettings.Save();
	}

	private void OnTransparencySliderMove(float newValue)
	{
		GameSettings.UItransparency = newValue;
		GameSettings.Save();

		SetDemoTransparency(newValue);
	}

	private void OnModelQualitySelect(GameSettings.Quality quality)
	{
		GameSettings.modelQuality = quality;
		GameSettings.Save();
	}

	private void OnTextureQualitySelect(GameSettings.Quality quality)
	{
		GameSettings.textureQuality = quality;
		GameSettings.Save();
	}

	private void OnCheckGamepadsClick()
	{
		if (GameSettings.CheckGamepads())
		{
			Debug.Assert(false, "Todo gamepad checking ui refresh");
			//RefreshControllerConfigPanels();
		}
	}

	private void ChangeLanguage(SystemLanguage language)
	{
		GameSettings.language = language;
		GameSettings.Save();
	}

	private void SetDemoTransparency(float value)
	{
		float opacity = 1.0f - value;
		Color color;

		color = demoVirtualJoystickArea.color;
		color.a = opacity;
		demoVirtualJoystickArea.color = color;

		color = demoVirtualJoystickButton.color;
		color.a = opacity;
		demoVirtualJoystickButton.color = color;
	}
}
