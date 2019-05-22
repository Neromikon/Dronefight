using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBeam : Missile
{
	public LineRenderer lineRenderer;

	public float continuousDamage;
	public DamageType continuousDamageType;

	public GameObject targetObject { get; private set; }
	public Unit targetUnit { get; private set; }
	public Missile targetMissile { get; private set; }
	public Item targetItem { get; private set; }

	protected override void Start()
	{
		base.Start();

		//Debug.Assert(owner != null, "Beam missile must have owner");
		Debug.Assert(lineRenderer != null, "Beam missile must have line renderer assigned");
	}

	protected override void Update()
	{
		lifeTimer.Update();

		if (lifeTimer.Expired) { gameObject.SetActive(false); }

		//if (targetObject)
		//{
		//	lineRenderer.SetPosition(1, targetObject.transform.position);

		//	switch (targetObject.layer)
		//	{
		//		case GameLayer.Units:
		//		{
		//			targetUnit.ReceiveDamage(continuousDamage * Time.deltaTime, continuousDamageType);
		//			break;
		//		}

		//		case GameLayer.Missiles:
		//		{
		//			targetMissile.ReceiveDamage(continuousDamage * Time.deltaTime, continuousDamageType);
		//			break;
		//		}

		//		case GameLayer.Items:
		//		{
		//			targetItem.ReceiveDamage(continuousDamage * Time.deltaTime, continuousDamageType);
		//			break;
		//		}
		//	}
		//}
	}

	public void SetTarget(GameObject target)
	{
		//targetObject = target;

		//if (target != null)
		//{
		//	lineRenderer.enabled = true;

		//	switch (target.layer)
		//	{
		//		case GameLayer.Units:
		//		{
		//			targetUnit = target.GetComponent<Unit>();
		//			Debug.Assert(targetUnit, "Target for beam missile " + name + " belongs to units layer but is not a unit");
		//			break;
		//		}

		//		case GameLayer.Missiles:
		//		{
		//			targetMissile = target.GetComponent<Missile>();
		//			Debug.Assert(targetUnit, "Target for beam missile " + name + " belongs to missiles layer but is not a missile");
		//			break;
		//		}

		//		case GameLayer.Items:
		//		{
		//			targetItem = target.GetComponent<Item>();
		//			Debug.Assert(targetUnit, "Target for beam missile " + name + " belongs to items layer but is not an item");
		//			break;
		//		}
		//	}
		//}
		//else
		//{
		//	lineRenderer.enabled = false;
		//}
	}
}
