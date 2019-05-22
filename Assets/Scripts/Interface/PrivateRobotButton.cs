using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PrivateRobotButton : MonoBehaviour
{
	public PrivateButton buttonComponent;
	public Unit robotPrefab;

	private void Start()
	{
		buttonComponent.image.sprite = robotPrefab.avatar;
	}
}