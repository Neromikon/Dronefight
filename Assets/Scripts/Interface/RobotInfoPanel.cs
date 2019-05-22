using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotInfoPanel : MonoBehaviour
{
	public Controller owner;

	public PlayerPanel playerPanel;

	public Image robotImage;
	//public Button backButton;
	public Button tool1Button, tool2Button, tool3Button;
	public Image tool1Image, tool2Image, tool3Image;
	public Text nameText;
	public Text detailsText;
	public RectTransform detailsRect;

	private Unit robotPrefab;

	void Start ()
	{
		RectTransform rectTransform = playerPanel.GetComponent<RectTransform>();

		robotImage.rectTransform.sizeDelta = rectTransform.rect.size * 0.45f;

		Refresh();
	}
	
	void Update ()
	{
		
	}

	public void SetOwner(Controller panelOwner)
	{
		owner = panelOwner;
	}

	public void Setup(Unit robot)
	{
		robotPrefab = robot;

		Refresh();
	}

	private void Refresh()
	{
		if (!robotPrefab) { return; }
		if (!robotImage) { return; }

		robotImage.sprite = robotPrefab.avatar;
		if (robotPrefab.tool1) { tool1Image.sprite = robotPrefab.tool1.sprite; }
		if (robotPrefab.tool2) { tool2Image.sprite = robotPrefab.tool2.sprite; }
		if (robotPrefab.tool3) { tool3Image.sprite = robotPrefab.tool3.sprite; }

		nameText.text = robotPrefab.localizedName.Get(GameSettings.language);

		detailsText.text = robotPrefab.localizedDetails.Get(GameSettings.language);
	}
}
