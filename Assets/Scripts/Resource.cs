using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
	public enum Type { UNCOUNTABLE, COUNTABLE }

	public Type type;
	public Sprite icon;
	public Color barColor;
}
