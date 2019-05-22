using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConnector : Item
{
	public Unit user = null;

	public DestructibleStation station;

	protected override void Start ()
	{
		base.Start();
	}
	
	protected override void Update ()
	{
		base.Update();

		//if connector is connected then feed target unit with resources
		//Feed();
	}

	void Feed(ResourceContainer refillingResource)
	{
		if (station == null) { return; }
		if (user == null) { return; }
		if (refillingResource.resource != station.supply.resource) { return; }

		float exchange = station.feed * Time.deltaTime;

		if (refillingResource.Have(refillingResource.maximum - exchange)) { return; }
		if (!station.supply.Have(exchange)) { return; }

		refillingResource.Spend(-exchange);
		station.supply.Spend(exchange);
	}

	public override void Pick(Unit unit)
	{
		base.Pick(unit);

		user = unit;
	}

	public override void Drop()
	{
		base.Drop();

		user = null;
	}
}
