using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour
{
	public enum Menu { ABOUT, ROBOTS, MAPS }

	public MainMenu mainMenu;
	public Button aboutButton;
	public Button robotsButton;
	public Button mapsButton;
	public Button backButton;

	public GameObject aboutPanel;
	public RobotDescriptionPanel robotsPanel;
	public GameObject mapsPanel;

	public Button descriptionButton;
	public Button playtestButton;
	public Button viewButton;

	private Menu currentMenu;

    void Start()
    {
		aboutButton.onClick.AddListener(OnAboutClick);
		robotsButton.onClick.AddListener(OnRobotsClick);
		mapsButton.onClick.AddListener(OnMapsClick);
		backButton.onClick.AddListener(OnBackClick);

		descriptionButton.onClick.AddListener(OnDescriptionClick);
		playtestButton.onClick.AddListener(OnPlaytestClick);
		viewButton.onClick.AddListener(OnViewClick);

		SwitchMenu(Menu.ABOUT);
    }
	
    void Update()
    {
        
    }

	public void SwitchMenu(Menu menu)
	{
		currentMenu = menu;

		aboutPanel.SetActive(menu == Menu.ABOUT);
		robotsPanel.gameObject.SetActive(menu == Menu.ROBOTS);
		mapsPanel.SetActive(menu == Menu.MAPS);

		descriptionButton.gameObject.SetActive(menu == Menu.ROBOTS);
		playtestButton.gameObject.SetActive(menu == Menu.ROBOTS);
		viewButton.gameObject.SetActive(menu == Menu.ROBOTS);

		switch (menu)
		{
			case Menu.ROBOTS:
			{
				GameSettings.mainPlayer.gameObject.SetActive(true);
				break;
			}
		}
	}

	private void OnAboutClick()
	{
		SwitchMenu(Menu.ABOUT);
	}

	private void OnRobotsClick()
	{
		SwitchMenu(Menu.ROBOTS);
	}

	private void OnMapsClick()
	{
		SwitchMenu(Menu.MAPS);
	}

	private void OnBackClick()
	{
		gameObject.SetActive(false);
		mainMenu.gameObject.SetActive(true);
	}

	private void OnDescriptionClick()
	{
		robotsPanel.SwitchMenu(RobotDescriptionPanel.Menu.DESCRIPTION);
	}

	private void OnPlaytestClick()
	{
		robotsPanel.SwitchMenu(RobotDescriptionPanel.Menu.PLAYTEST);
	}

	private void OnViewClick()
	{
		robotsPanel.SwitchMenu(RobotDescriptionPanel.Menu.VIEW);
	}
}
