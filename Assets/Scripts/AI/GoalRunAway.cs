using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoalRunAway : Goal
{
	public Unit target;
	public float minDistance = 4.0f;

	GoalRunAway(Unit target)
	{
		this.target = target;
	}

	protected override void Update()
	{
		//if(transform.childCount > 0) { return; }

		Vector3 direction = target.transform.position - unit.transform.position;
		float distance = direction.magnitude;
		if(distance > minDistance) { return; }

		unit.Move(new Vector2(-direction.x, -direction.z));
	}
}