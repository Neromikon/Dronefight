using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InitMenu : MonoBehaviour
{
	public enum Menu { DEFAULT_LANGUAGE, LANGUAGE, CONTROL }

	private const float ORIENTATION_CHANGE_COEFFICIENT = 1.75f;

	public RectTransform rectTransform;

	public MainMenu mainMenu;

	public GameObject languagePanel;
	public GameObject controlPanel;

	public Transform languageButtonsDirectParent;

	public Button soloKeyboardButton;
	public Button dualKeyboardButton;
	public Button dualKeyboardAndMouseButton;
	public Button soloTouchscreenButton;
	public Button dualTouchscreenButton;

	public Transform verticalLayout;
	public Transform horizontalLayout;

	public Menu currentMenu { get; private set; }

	private void Start()
	{
		foreach (Transform child in languageButtonsDirectParent)
		{
			LanguageButton languageButton = child.GetComponent<LanguageButton>();

			if (!languageButton) { continue; }

			if (GameSettings.IsLanguageSupported(languageButton.language))
			{
				languageButton.buttonComponent.onClick.AddListener(() => OnLanguageSelect(languageButton.language));
			}
			else
			{
				languageButton.gameObject.SetActive(false);
			}
		}

		Support.ShuffleChildrenOrder(languageButtonsDirectParent);

		RuntimePlatform platform = Application.platform;

		#if DEBUG
		{
			if (GameSettings.useDebugPlatform) { platform = GameSettings.debugPlatform; }
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
				soloKeyboardButton.onClick.AddListener(OnSoloKeyboardClick);
				dualKeyboardButton.onClick.AddListener(OnDualKeyboardClick);
				dualKeyboardAndMouseButton.onClick.AddListener(OnDualKeyboardAndMouseClick);

				soloKeyboardButton.gameObject.SetActive(true);
				dualKeyboardButton.gameObject.SetActive(true);
				dualKeyboardAndMouseButton.gameObject.SetActive(true);

				soloTouchscreenButton.gameObject.SetActive(false);
				dualTouchscreenButton.gameObject.SetActive(false);

				break;
			}

			case RuntimePlatform.Android:
			case RuntimePlatform.IPhonePlayer:
			{
				soloTouchscreenButton.onClick.AddListener(OnSoloTouchscreenClick);
				dualTouchscreenButton.onClick.AddListener(OnDualTouchscreenClick);

				soloTouchscreenButton.gameObject.SetActive(true);
				dualTouchscreenButton.gameObject.SetActive(true);

				soloKeyboardButton.gameObject.SetActive(false);
				dualKeyboardButton.gameObject.SetActive(false);
				dualKeyboardAndMouseButton.gameObject.SetActive(false);

				break;
			}
		}

		if (GameSettings.IsLanguageSupported(Application.systemLanguage))
		{
			OnLanguageSelect(Application.systemLanguage);
		}
		else
		{
			SwitchMenu(Menu.LANGUAGE);
		}
	}

	private void Update()
	{
		if (rectTransform.hasChanged) { OnOrientationChange(); }
	}

	public void SwitchMenu(Menu menu)
	{
		currentMenu = menu;

		languagePanel.SetActive(menu == Menu.LANGUAGE);
		controlPanel.SetActive(menu == Menu.CONTROL);
	}

	private void OnOrientationChange()
	{
		rectTransform.hasChanged = false;

		if (rectTransform.rect.width >= rectTransform.rect.height * ORIENTATION_CHANGE_COEFFICIENT)
		{
			soloKeyboardButton.transform.SetParent(horizontalLayout);
			dualKeyboardButton.transform.SetParent(horizontalLayout);
			dualKeyboardAndMouseButton.transform.SetParent(horizontalLayout);
			soloTouchscreenButton.transform.SetParent(horizontalLayout);
			dualTouchscreenButton.transform.SetParent(horizontalLayout);
		}
		else
		{
			soloKeyboardButton.transform.SetParent(verticalLayout);
			dualKeyboardButton.transform.SetParent(verticalLayout);
			dualKeyboardAndMouseButton.transform.SetParent(verticalLayout);
			soloTouchscreenButton.transform.SetParent(verticalLayout);
			dualTouchscreenButton.transform.SetParent(verticalLayout);
		}
	}

	private void GoToMainMenu()
	{
		GameSettings.isFirstRun = false;
		GameSettings.Save();
		
		gameObject.SetActive(false);
		mainMenu.gameObject.SetActive(true);
	}

	private void OnLanguageSelect(SystemLanguage language)
	{
		GameSettings.language = language;

		SwitchMenu(Menu.CONTROL);
	}

	private void OnSoloKeyboardClick()
	{
		GameSettings.keyboardController1.isAvailable = true;
		GameSettings.keyboardController2.isAvailable = false;
		GameSettings.mouseController.isAvailable = false;

		GoToMainMenu();
	}

	private void OnDualKeyboardClick()
	{
		GameSettings.keyboardController1.isAvailable = true;
		GameSettings.keyboardController2.isAvailable = true;
		GameSettings.mouseController.isAvailable = false;

		GoToMainMenu();
	}

	private void OnDualKeyboardAndMouseClick()
	{
		GameSettings.keyboardController1.isAvailable = true;
		GameSettings.keyboardController2.isAvailable = true;
		GameSettings.mouseController.isAvailable = true;

		GoToMainMenu();
	}

	private void OnSoloTouchscreenClick()
	{
		GameSettings.touchscreenController1.isAvailable = true;
		GameSettings.touchscreenController2.isAvailable = false;
		
		GoToMainMenu();
	}

	private void OnDualTouchscreenClick()
	{
		GameSettings.touchscreenController1.isAvailable = true;
		GameSettings.touchscreenController2.isAvailable = true;
		
		GoToMainMenu();
	}
}
