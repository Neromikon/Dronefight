using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
	public Unit unit;

	private const float enableTime = 0.2f; //[seconds]
	private float timeLeft;

	private void OnEnable()
	{
		timeLeft = enableTime;
		if(unit) { transform.rotation = unit.transform.rotation; }
	}

	private void Update()
	{
		if(timeLeft > 0)
		{
			timeLeft -= Time.deltaTime;
			if(timeLeft < 0)
			{
				gameObject.SetActive(false);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == GameLayer.Items)
		{
			Item targetItem = other.GetComponent<Item>();
			if(targetItem != null)
			{
				unit.Pick(targetItem);
				gameObject.SetActive(false);
			}
		}
	}
}
