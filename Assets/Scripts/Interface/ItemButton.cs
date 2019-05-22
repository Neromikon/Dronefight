using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
	public Button buttonComponent;
	public Item itemPrefab;

	private void Start()
	{
		buttonComponent.image.sprite = itemPrefab.icon;
	}
}
