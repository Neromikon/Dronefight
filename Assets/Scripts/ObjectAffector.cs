using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAffector : MonoBehaviour
{
	public const int MAX_TARGETS = 20;

	public Unit owner;

	public bool affectUnits = true;
	public bool affectItems = true;
	public bool affectDestructibles = true;
	public bool affectMissiles = true;
	
	public delegate void AffectFunction(List<GameObject> gameObject);
	public AffectFunction affectFunction;

	private List<GameObject> targets = new List<GameObject>(MAX_TARGETS);

	void Update()
    {
		if (affectFunction != null)
		{
			targets.Sort(delegate (GameObject a, GameObject b)
			{
				float distance1 = (transform.position - a.transform.position).sqrMagnitude;
				float distance2 = (transform.position - b.transform.position).sqrMagnitude;
				return distance1.CompareTo(distance2);
			});

			affectFunction(targets);
		}

		gameObject.SetActive(false);
    }

	public void OnEnable()
	{
		targets.Clear();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (targets.Count >= targets.Capacity) { return; }

		switch (other.gameObject.layer)
		{
			case GameLayer.Units:
			{
				if (!affectUnits) { break; }
				if (other.gameObject == owner.gameObject) { break; }

				targets.Add(other.gameObject);

				break;
			}

			case GameLayer.Items:
			{
				if (!affectItems) { break; }

				targets.Add(other.gameObject);

				break;
			}

			case GameLayer.Destructibles:
			{
				if (!affectDestructibles) { break; }

				targets.Add(other.gameObject);

				break;
			}

			case GameLayer.Missiles:
			{
				if (!affectMissiles) { break; }

				targets.Add(other.gameObject);

				break;
			}
		}
	}
}
