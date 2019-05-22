using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceContainer : MonoBehaviour
{
	public bool active;
	public Resource resource;
	public float amount = 100.0f;
	public float maximum = 100.0f;
	public float passiveConsumption = 0.0f;
	public float activeConsumption = 0.0f;

	public float percentage { get { return amount / maximum; } }
	
	void Update()
	{
		if (active)
		{
			Spend(activeConsumption * Time.deltaTime);
		}
		else
		{
			Spend(passiveConsumption * Time.deltaTime);
		}
	}

	public void Spend()
	{
		amount = 0;
	}

	public bool Spend(float value)
	{
		amount -= value;
		if (amount < 0) { amount = 0; return false; }
		if (amount > maximum) { amount = maximum; return false; }
		return true;
	}

	public bool Have(float value)
	{
		return amount >= value;
	}

	public bool IsMaxed()
	{
		return amount >= maximum;
	}

	public bool IsEmpty()
	{
		return amount <= 0;
	}

	public void Activate()
	{
		active = true;
	}

	public void Passivate()
	{
		active = false;
	}
}
