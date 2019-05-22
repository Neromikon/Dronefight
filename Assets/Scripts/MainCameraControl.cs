using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraControl : MonoBehaviour
{
	public enum Mode
	{
		SINGLE_PLAYER,
		MULTI_PLAYER,
		FREE_VIEW,
		SIDE_VIEW
	}

	public float includeAngleDegrees;
	public float cameraDistance;

	private List<GameObject> targets = new List<GameObject>(4);
	private float includeAngleCosinus;
	//private float includeAngleReverseCosinus;
	//private float includeAngleReverseSinus;
	//private float includeAngleReverseTangens;
	private float includeAngleCotangens;

	void Start ()
	{
		includeAngleCosinus = Mathf.Cos(includeAngleDegrees * Mathf.Deg2Rad);
		//includeAngleReverseCosinus = 1.0f / Mathf.Cos(includeAngleDegrees * Mathf.Deg2Rad);
		//includeAngleReverseSinus = 1.0f / Mathf.Sin(includeAngleDegrees * Mathf.Deg2Rad);
		includeAngleCotangens = 1.0f / Mathf.Tan(includeAngleDegrees * Mathf.Deg2Rad);
	}
	
	void Update ()
	{
		if (targets == null) { return; }
		if (targets.Count == 0) { return; }

		Vector4 sum = Vector4.zero;
		foreach (GameObject target in targets)
		{
			sum.x += target.transform.position.x;
			sum.y += target.transform.position.y;
			sum.z += target.transform.position.z;
			sum.w += 1.0f;
		}

		Vector3 maxCameraPoint = transform.position;
		float maxCameraDistance = float.MaxValue;
		Vector3 C = new Vector3(sum.x, sum.y, sum.z) / sum.w - transform.forward;
		
		foreach (GameObject target in targets)
		{
			Vector3 D = target.transform.position;
			Vector3 p = Vector3.Project(D - C, transform.forward);
			float n = p.magnitude;
			float h = (C + p - D).magnitude;

			float actualAngleCosinus = Vector3.Dot(transform.forward, (D - C).normalized);

			float d = (n - h * includeAngleCotangens) * Mathf.Sign(actualAngleCosinus - includeAngleCosinus);

			if (actualAngleCosinus < 0)
			{
				d = -n - h * includeAngleCotangens;
			}
			else if (actualAngleCosinus < includeAngleCosinus)
			{
				d = n - h * includeAngleCotangens;
			}
			else
			{
				d = n - h * includeAngleCotangens;
			}

			//Debug.Log("calculated camera distance change " + d +
			//	" C:" + C + " D:" + D + " fwd:" + transform.forward + " P:" + (C+p) + " n:" + n + " h:" + h + " cos:" + actualAngleCosinus);

			if (d < maxCameraDistance)
			{
				maxCameraDistance = d;
				maxCameraPoint = C + transform.forward * (d - cameraDistance);
			}

			//Debug.Log("Calculated camera position " + maxCameraPoint);
			transform.position = maxCameraPoint;
		}

		
	}

	public void AddTarget(GameObject newTarget)
	{
		targets.Add(newTarget);
	}

	public void AddTargets(ICollection newTargets)
	{
		foreach (GameObject newTarget in newTargets)
		{
			targets.Add(newTarget);
		}
	}

	public void ClearTargets()
	{
		targets.Clear();
	}
}
