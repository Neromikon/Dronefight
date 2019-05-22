using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public float reload;

	private float reloadRemain;

	void Start ()
	{
		MeshRenderer renderer = GetComponent<MeshRenderer>();

		if (renderer)
		{
			renderer.enabled = false;
		}
	}
	
	void Update ()
	{
		if (reloadRemain != 0)
		{
			reloadRemain -= Time.deltaTime;

			if (reloadRemain < 0) { reloadRemain = 0; }
		}
	}

	public bool IsReady()
	{
		return reloadRemain == 0;
	}

	public Unit Spawn(Unit unitPrefab)
	{
		if (reloadRemain > 0) { return null; }
		
		Quaternion randomOrientation = Quaternion.AngleAxis(Random.value * 360.0f, Vector3.up);

		Unit newUnit = Instantiate(unitPrefab, transform.position, randomOrientation);

		reloadRemain = reload;

		return newUnit;
	}

	public Item Spawn(Item itemPrefab)
	{
		return null;
	}
}
