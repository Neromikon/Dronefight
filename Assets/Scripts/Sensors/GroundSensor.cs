using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
	public bool contact { get; private set; }

	private void OnEnable()
	{
		contact = false;
	}

	private void LateUpdate()
	{
		contact = false;
	}

	private void OnTriggerStay(Collider other)
	{
		contact = true;
	}
}
