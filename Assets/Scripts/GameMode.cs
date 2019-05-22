using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
	public enum Type
	{
		SOLO, DUEL,
		FFA3, FFA4, FFA5, FFA6, FFA7, FFA8, FFA9, FFA10,
		TEAM2x2, TEAM3x3, TEAM4x4, TEAM5x5
	}

	public Type type;
	public Sprite icon;
	public int playersCount;
	public bool isTeamMode;
}
