using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHolograph: Tool
{
	//DEPRECATED


	//public GameObject illusion;

	//private GameObject illusion1, illusion2, illusion3;
	//private Vector3 mirrorPoint;

	//private new void Start()
	//{
	//	illusion1 = Instantiate(illusion, transform);
	//	illusion2 = Instantiate(illusion, transform);
	//	illusion3 = Instantiate(illusion, transform);

	//	illusion1.SetActive(false);
	//	illusion2.SetActive(false);
	//	illusion3.SetActive(false);
	//}

	//public override void Singleclick()
	//{
	//	switch(state)
	//	{
	//		case State.READY: Use(); break;
	//		case State.APPLY: Reload(); break;
	//	}
	//}

	//protected override void Apply()
	//{
	//	base.Apply();
	//	illusion1.SetActive(true);
	//	illusion2.SetActive(true);
	//	illusion3.SetActive(true);
	//	mirrorPoint = unit.transform.position;
	//	Refresh();
	//}

	//protected override void Reload()
	//{
	//	base.Reload();
	//	illusion1.SetActive(false);
	//	illusion2.SetActive(false);
	//	illusion3.SetActive(false);
	//}

	//protected override void ApplyUpdate()
	//{
	//	Refresh();
	//}

	//void Refresh()
	//{
	//	Vector3 direction = (mirrorPoint - unit.transform.position);
	//	float distance = direction.magnitude;

	//	if(distance > range)
	//	{
	//		Reload();
	//		return;
	//	}

	//	illusion1.transform.position = mirrorPoint + direction;
	//	illusion2.transform.position = mirrorPoint + Vector3.Cross(Vector3.up, direction).normalized * distance;
	//	illusion3.transform.position = mirrorPoint + Vector3.Cross(Vector3.down, direction).normalized * distance;

	//	illusion1.transform.rotation = Quaternion.LookRotation(-unit.transform.forward);
	//	illusion2.transform.rotation = Quaternion.LookRotation(-unit.transform.right);
	//	illusion3.transform.rotation = Quaternion.LookRotation(unit.transform.right);
	//}
}