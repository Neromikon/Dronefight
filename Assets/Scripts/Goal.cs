using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Goal : MonoBehaviour
{
	public Unit unit;

	protected virtual void Start()
	{
		Debug.Log("Goal " + name + " start() called");

		unit = Support.GetOwner(transform);
	}

	protected virtual void Update()
	{
		if(transform.childCount > 0) { return; }
	}

	public static NavMeshPath FindPath(Vector3 from, Vector3 to)
	{
		NavMeshPath path = new NavMeshPath();

		if(NavMesh.CalculatePath(from, to, 1, path))
		{
			return path;
			//return path[0];
		}
		else
		{
			Debug.Log("Path to " + " does not exist");
			return null;
			//return from;
		}
	}
}