using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoalWalk : Goal
{
	private float counter = 0;
	private NavMeshPath path;
	private Vector3 destination;

	protected override void Update()
	{
		if(!unit) { return; }
		//if(transform.childCount > 0) { return; }

		counter -= Time.deltaTime;
		if(counter <= 0)
		{
			counter = 5.0f;
			ChangePath();
		}

		if(path != null && path.corners.Length > 1)
		{
			if(Vector3.Distance(unit.transform.position, path.corners[1]) < 0.25f)
			{
				counter = 5.0f;
				ChangePath();
			}
			else
			{
				unit.MoveTo(path.corners[1]);
				Debug.DrawLine(unit.transform.position, path.corners[1], Color.magenta);
			}
		}
	}

	void ChangePath()
	{
		destination = unit.transform.position + new Quaternion(0, Random.value, 0, 0) * new Vector3(0, 0, 5);
		path = FindPath(unit.transform.position, destination);
	}
}