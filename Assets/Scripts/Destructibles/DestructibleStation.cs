using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleStation : Destructible
{
	public GameObject connector;
	public GameObject rope;

	public float feed = 0.0f; //units per second
	public float gain = 0.0f;
	public ResourceContainer supply;
	public float maxStorage = 0.0f;

	private float storage = 0.0f;

	private Transform clamp;
	private ItemConnector connectorData;

	void Start ()
	{
		if(!connector) { Debug.LogError("Connector prefab for " + name + " not set"); }

		connector = Instantiate(connector);
		connectorData = connector.GetComponent<ItemConnector>();
		connectorData.station = this;

		if(!connector) { Debug.LogError("Failed to create connector for " + name); }
		if(!connectorData) { Debug.LogError("Failed to get connector data for " + name); }

		rope = Instantiate(rope, transform);

		clamp = Support.FindRecursive(transform, "Clamp");
	}
	
	void Update ()
	{
		if(rope && connector && clamp)
		{
			Vector3 direction = connector.transform.position - clamp.transform.position;
			rope.transform.position = clamp.transform.position + direction * 0.5f;
			rope.transform.rotation = Quaternion.LookRotation(direction);
			rope.transform.localScale = new Vector3(1.0f, 1.0f, direction.magnitude);
		}

		if(gain != 0 && storage < maxStorage)
		{
			storage += gain * Time.deltaTime;
			if(storage > maxStorage) { storage = maxStorage; }
			if(storage < 0) { storage = 0; }
		}
	}

	public float Get(float request)
	{
		if(storage > request)
		{
			storage -= request;
			return request;
		}
		else
		{
			request = storage;
			storage = 0;
			return request;
		}
	}
}
