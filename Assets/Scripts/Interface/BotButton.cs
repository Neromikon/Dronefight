using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotButton : MonoBehaviour
{
	public Button buttonComponent;
	public Controller owner;

	private void Start()
	{
		buttonComponent.image.sprite = owner.visual.icon;
	}
}
