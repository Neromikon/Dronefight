using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PublicRobotButton : MonoBehaviour
{
	public Button buttonComponent;
	public Unit robotPrefab;

	private void Start()
	{
		buttonComponent.image.sprite = robotPrefab.avatar;
	}
}