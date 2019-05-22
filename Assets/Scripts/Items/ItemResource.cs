using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemResource : Item
{
	public float amount;
	public Resource resourceType;

	public override bool Use(Unit unit)
	{
		Debug.LogAssertion("NEED TO REDO RESOURCE ITEMS CONSUMPTION");
		//if(!unit.Have(0, resourceType)) { return false; }
		//unit.Spend(-amount, resourceType);
		return true;
	}
}
