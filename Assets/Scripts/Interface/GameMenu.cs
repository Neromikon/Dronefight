using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
	public enum Menu { LOAD, PLAY }
	
	public Menu currentMenu { get; private set; }

	public RectTransform statePanelPrefab;
	
	public RectTransform rectTransform;

	public RectTransform topStateLayout;
	public RectTransform bottomStateLayout;

	public Slider loadingBar;
	public GameObject loadingScreen;

	public RobotStatePanel[] statePanels;

	public VirtualControl[] virtualControls;

	void Start ()
	{
		SwitchMenu(Menu.LOAD);
	}

	private void Update()
	{
		if (rectTransform.hasChanged) { OnOrientationChange(); }

		switch (currentMenu)
		{
			case Menu.LOAD:
			{
				loadingBar.value = GameSettings.loadingProgress;
				break;
			}
		}
	}

	public void SwitchMenu(Menu menu)
	{
		currentMenu = menu;

		loadingBar.gameObject.SetActive(menu == Menu.LOAD);
		loadingScreen.SetActive(menu == Menu.LOAD);

		topStateLayout.gameObject.SetActive(menu == Menu.PLAY);
		bottomStateLayout.gameObject.SetActive(menu == Menu.PLAY && !GameSettings.isTouchscreenSupported);

		switch (menu)
		{
			case Menu.PLAY:
			{
				OnOrientationChange();
				break;
			}
		}
	}
	
	private void OnOrientationChange()
	{
		//note: expected that GameSettings.activePlayers is sorted in displaying priority order

		rectTransform.hasChanged = false;
		
		int fittingElements = (int)(topStateLayout.rect.width / statePanelPrefab.rect.width);
		int topElements = Mathf.Min(fittingElements, Mathf.Min(GameSettings.activePlayers.Count / 2, statePanels.Length));
		int bottomElements = 0;

		//enable top panels
		for (int i = 0; i < topElements; i++)
		{
			statePanels[i].gameObject.SetActive(true);
			statePanels[i].transform.SetParent(topStateLayout);
			statePanels[i].SetOwner(GameSettings.activePlayers[i]);

			GameSettings.activePlayers[i].SetPanel(statePanels[i]);
		}

		//enable bottom panels
		if (bottomStateLayout.gameObject.activeSelf)
		{
			bottomElements = Mathf.Min(fittingElements, Mathf.Min(GameSettings.activePlayers.Count, statePanels.Length) - topElements);

			for (int i = topElements; i < topElements + bottomElements; i++)
			{
				statePanels[i].gameObject.SetActive(true);
				statePanels[i].transform.SetParent(bottomStateLayout);
				statePanels[i].SetOwner(GameSettings.activePlayers[i]);

				GameSettings.activePlayers[i].SetPanel(statePanels[i]);
			}
		}

		Debug.Log("fitting = " + fittingElements + ", top = " + topElements + ", bottom = " + bottomElements + ", players = " + GameSettings.activePlayers.Count);

		//disable rest panels
		for (int i = topElements + bottomElements; i < statePanels.Length; i++)
		{
			statePanels[i].gameObject.SetActive(false);
		}
	}

	public void DisableVirtualControls()
	{
		foreach (VirtualControl virtualControl in virtualControls)
		{
			virtualControl.gameObject.SetActive(false);
		}
	}

	public void EnableVirtualControl(ControllerTouchscreen player)
	{
		Debug.Assert(player.layout < virtualControls.Length, "Inappropriate layout index " + player.layout + " for controller " + player.name);

		VirtualControl virtualControl = virtualControls[player.layout];

		virtualControl.gameObject.SetActive(true);
		player.SetVirtualControl(virtualControl);
	}
}
