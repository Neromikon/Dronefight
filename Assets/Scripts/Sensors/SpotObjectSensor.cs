using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotObjectSensor : MonoBehaviour
{
	public Unit owner;

	public bool detectUnits = true;
	public bool detectItems = true;
	public bool detectDestructibles = true;
	public bool detectMissiles = true;

	public GameObject closestObject { get; private set; }
	public float closestObjectSqrDistance { get; private set; }

	public GameObject closestUnit { get; private set; }
	public float closestUnitSqrDistance { get; private set; }

	public GameObject closestItem { get; private set; }
	public float closestItemSqrDistance { get; private set; }

	public GameObject closestDestructible { get; private set; }
	public float closestDestructibleSqrDistance { get; private set; }

	public GameObject closestMissile { get; private set; }
	public float closestMissileSqrDistance { get; private set; }
	
	private void LateUpdate()
	{
		ResetTargets();
	}

	public void OnEnable()
	{
		ResetTargets();
	}

	private void ResetTargets()
	{
		closestObject = null;
		closestObjectSqrDistance = float.MaxValue;

		if (detectUnits)
		{
			closestUnit = null;
			closestUnitSqrDistance = float.MaxValue;
		}

		if (detectItems)
		{
			closestItem = null;
			closestItemSqrDistance = float.MaxValue;
		}

		if (detectDestructibles)
		{
			closestDestructible = null;
			closestDestructibleSqrDistance = float.MaxValue;
		}

		if (detectMissiles)
		{
			closestMissile = null;
			closestMissileSqrDistance = float.MaxValue;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		float sqrDistance = (other.transform.position - transform.position).sqrMagnitude;

		if (sqrDistance < closestObjectSqrDistance)
		{
			closestObject = other.gameObject;
			closestObjectSqrDistance = sqrDistance;
		}

		switch (other.gameObject.layer)
		{
			case GameLayer.Units:
			{
				if (!detectUnits) { break; }
				if (other.gameObject == owner.gameObject) { break; }
				if (sqrDistance >= closestUnitSqrDistance) { break; }

				closestUnit = other.gameObject;
				closestUnitSqrDistance = sqrDistance;

				break;
			}

			case GameLayer.Items:
			{
				if (!detectItems) { break; }
				if (sqrDistance >= closestItemSqrDistance) { break; }

				closestItem = other.gameObject;
				closestItemSqrDistance = sqrDistance;

				break;
			}

			case GameLayer.Destructibles:
			{
				if (!detectDestructibles) { break; }
				if (sqrDistance >= closestDestructibleSqrDistance) { break; }

				closestDestructible = other.gameObject;
				closestDestructibleSqrDistance = sqrDistance;

				break;
			}

			case GameLayer.Missiles:
			{
				if (!detectMissiles) { break; }
				if (sqrDistance >= closestMissileSqrDistance) { break; }

				closestMissile = other.gameObject;
				closestMissileSqrDistance = sqrDistance;

				break;
			}
		}
	}
}
