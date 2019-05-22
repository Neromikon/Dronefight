using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFollow : Goal
{
	protected override void Update()
	{
		if(transform.childCount > 0) { return; }
	}
}