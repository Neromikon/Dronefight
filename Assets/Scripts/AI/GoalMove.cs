using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoalMove : Goal
{
	private NavMeshPath path;

	protected override void Update()
	{
		if(transform.childCount > 0) { return; }

		Vector3 destination = path.corners[0];

		unit.Move(Vector2.zero);
	}
}