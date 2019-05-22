using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualScheme : MonoBehaviour
{
	public Controller.ControlType type;
	public Sprite icon;
	public ControllerSelector selector;
	public Color color;

	private void Start()
	{
		selector = Instantiate(selector);
	}
}
