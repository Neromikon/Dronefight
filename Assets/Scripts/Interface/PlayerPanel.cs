using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
	public enum Menu { PLACE, ROBOT, INFO }

	private const float ORIENTATION_CHANGE_COEFFICIENT = 1.25f;

	public Menu currentMenu { get; private set; }

	public RectTransform rectTransform;

	public int forNumberOfPlayers;
	public int playerIndex;

	public GameObject placeSelectionPanel;
	public GameObject robotSelectionPanel;
	public RobotInfoPanel robotInfoPanel;
	//public GridLayoutGroup robotsGridLayout;
	public PlaceManagementPanel placeManagement;

	public Image controllerIcon;
	public Text controllerName;
	public PrivateButton backButton;
	public PrivateButton randomButton;
	public PrivateButton joinNeutralButton;
	public PrivateButton joinBlueButton;
	public PrivateButton joinRedButton;
	public PrivateButton joinViewButton;
	public PrivateButton joinTrainButton;

	//public PlaceSelectionButton[] placeButtons;
	public PrivateRobotButton[] robotButtons;

	private Controller owner;

	void Start()
	{
		foreach (PrivateRobotButton button in robotButtons)
		{
			button.buttonComponent.privateOnClick.AddListener(() => OnRobotSelect(button.robotPrefab));
		}

		//robotsGridLayout.cellSize = Vector2.Scale(rectTransform.rect.size, new Vector2(0.2f, 0.25f));

		backButton.privateOnClick.AddListener(OnBackClick);
		randomButton.privateOnClick.AddListener(OnRandomClick);

		joinNeutralButton.privateOnClick.AddListener(OnJoinNeutralClick);
		joinBlueButton.privateOnClick.AddListener(OnJoinBlueClick);
		joinRedButton.privateOnClick.AddListener(OnJoinRedClick);
		joinViewButton.privateOnClick.AddListener(OnJoinViewersClick);
		joinTrainButton.privateOnClick.AddListener(OnJoinTrainersClick);

		SwitchMenu(Menu.PLACE);
	}

	private void Update()
	{
		if (rectTransform.hasChanged) { OnOrientationChange(); }
	}

	public void SetOwner(Controller newOwner)
	{
		Debug.Assert(newOwner != null); //disabling through that method not supported for now, todo

		owner = newOwner;
		
		if (robotInfoPanel) { robotInfoPanel.SetOwner(newOwner); }

		if (newOwner.visual.selector)
		{
			newOwner.visual.selector.SetArea(transform);
		}
		
		foreach (PrivateRobotButton button in robotButtons)
		{
			button.buttonComponent.owner = newOwner;
		}

		backButton.owner = newOwner;
		randomButton.owner = newOwner;

		joinNeutralButton.owner = newOwner;
		joinBlueButton.owner = newOwner;
		joinRedButton.owner = newOwner;
		joinViewButton.owner = newOwner;
		joinTrainButton.owner = newOwner;

		controllerIcon.sprite = newOwner.visual.icon;
		controllerName.text = newOwner.localizedName;
	}

	public void SwitchMenu(Menu menu)
	{
		currentMenu = menu;

		if (owner)
		{
			owner.visual.selector.SetTarget(null);

			joinViewButton.gameObject.SetActive(owner.type == Controller.PlayerType.REMOTE);
		}
		else
		{
			joinViewButton.gameObject.SetActive(false);
		}
		
		placeSelectionPanel.SetActive(menu == Menu.PLACE);
		robotSelectionPanel.gameObject.SetActive(menu == Menu.ROBOT);
		robotInfoPanel.gameObject.SetActive(menu == Menu.INFO);

		backButton.gameObject.SetActive(menu != Menu.PLACE);

		joinTrainButton.gameObject.SetActive(false); //todo! neural network training is not implemented yet

		switch (menu)
		{
			case Menu.PLACE:
			{
				joinNeutralButton.gameObject.SetActive(!GameSettings.gameMode.isTeamMode);
				joinBlueButton.gameObject.SetActive(GameSettings.gameMode.isTeamMode);
				joinRedButton.gameObject.SetActive(GameSettings.gameMode.isTeamMode);

				joinNeutralButton.interactable = placeManagement.HaveFreeNeutralPlaces;
				joinBlueButton.interactable = placeManagement.HaveFreeBluePlaces;
				joinRedButton.interactable = placeManagement.HaveFreeRedPlaces;

				bool randomButtonAllowed = GameSettings.gameMode.isTeamMode;
				randomButtonAllowed &= placeManagement.HaveFreeBluePlaces;
				randomButtonAllowed &= placeManagement.HaveFreeRedPlaces;
				randomButton.gameObject.SetActive(randomButtonAllowed);

				break;
			}

			case Menu.ROBOT:
			{
				randomButton.gameObject.SetActive(true);

				break;
			}

			case Menu.INFO:
			{
				randomButton.gameObject.SetActive(false);

				robotInfoPanel.Setup(owner.unitPrefab);

				break;
			}
		}
	}

	private void OnOrientationChange()
	{
		rectTransform.hasChanged = false;

		//float ratio = robotSelectionRectTransform.rect.width / robotSelectionRectTransform.rect.height;

		//float cellSize;
		//Vector2 spacing;

		//Debug.Log("Ratio is " + ratio);

		//if (ratio >= 5.0f)
		//{
		//	robotsGridLayout.constraintCount = 2;

		//	cellSize = Mathf.Min(robotSelectionRectTransform.rect.width / 10.0f, robotSelectionRectTransform.rect.height / 2.0f);
		//	spacing.x = (robotSelectionRectTransform.rect.width % cellSize) / 9.0f;
		//	spacing.y = robotSelectionRectTransform.rect.width % cellSize;
		//}
		//else if (ratio >= 1.25f)
		//{
		//	robotsGridLayout.constraintCount = 4;

		//	cellSize = Mathf.Min(robotSelectionRectTransform.rect.width / 5.0f, robotSelectionRectTransform.rect.height / 4.0f);
		//	spacing.x = (robotSelectionRectTransform.rect.width % cellSize) / 4.0f;
		//	spacing.y = (robotSelectionRectTransform.rect.width % cellSize) / 3.0f;
		//}
		//else
		//{
		//	robotsGridLayout.constraintCount = 5;

		//	cellSize = Mathf.Min(robotSelectionRectTransform.rect.width / 4.0f, robotSelectionRectTransform.rect.height / 5.0f);
		//	spacing.x = (robotSelectionRectTransform.rect.width % cellSize) / 3.0f;
		//	spacing.y = (robotSelectionRectTransform.rect.width % cellSize) / 4.0f;
		//}
		
		//robotsGridLayout.cellSize = new Vector2(cellSize, cellSize);
		//robotsGridLayout.spacing = spacing;
	}

	private void OnBackClick()
	{
		switch (currentMenu)
		{
			case Menu.PLACE: Debug.LogError("Back button must be disabled at place selection"); break;
			case Menu.ROBOT: SwitchMenu(Menu.PLACE); break;
			case Menu.INFO: SwitchMenu(Menu.ROBOT); break;
		}
	}

	private void OnRandomClick()
	{
		switch (currentMenu)
		{
			case Menu.PLACE:
			{
				//placeButtons[Random.Range(0, placeButtons.Length)].Click(owner); break;

				if (Random.Range(0, 2) == 1)
				{
					OnJoinRedClick();
				}
				else
				{
					OnJoinBlueClick();
				}

				break;
			}

			case Menu.ROBOT: robotButtons[Random.Range(0, robotButtons.Length)].buttonComponent.Click(owner); break;

			case Menu.INFO: Debug.LogError("Random button must be disabled at robot info"); break;
		}
	}

	private void OnRobotSelect(Unit robotPrefab)
	{
		owner.unitPrefab = robotPrefab;
		placeManagement.RefreshPlaces();
		SwitchMenu(Menu.INFO);
	}

	private void OnJoinNeutralClick()
	{
		placeManagement.AddPlayer(owner, Controller.Team.NEUTRAL);
		SwitchMenu(Menu.ROBOT);
	}

	private void OnJoinBlueClick()
	{
		placeManagement.AddPlayer(owner, Controller.Team.BLUE);
		SwitchMenu(Menu.ROBOT);
	}

	private void OnJoinRedClick()
	{
		placeManagement.AddPlayer(owner, Controller.Team.RED);
		SwitchMenu(Menu.ROBOT);
	}

	private void OnJoinViewersClick()
	{
		placeManagement.AddPlayer(owner, Controller.Team.VIEWERS);
		SwitchMenu(Menu.INFO);
	}

	private void OnJoinTrainersClick()
	{
		Debug.LogError("This feature is WIP, don't use for now");
		placeManagement.AddPlayer(owner, Controller.Team.TRAINERS);
		//SwitchMenu(Menu.); //switch to neural controller selection
	}
}
