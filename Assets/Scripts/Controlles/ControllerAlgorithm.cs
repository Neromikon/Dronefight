using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerAlgorithm : Controller
{
	public NavMeshAgent navMeshAgent;

	private Goal goal;

	protected override void Start()
	{
		base.Start();

		goal = new GoalIdle();
	}

	public void Whatever()
	{
		unit.Move(navMeshAgent.desiredVelocity); //unit.Move accepts Vector2 but desired velocity is Vector3!!!

		Vector3 direction = Vector3.ProjectOnPlane(navMeshAgent.desiredVelocity, unit.transform.up);
	}
}
