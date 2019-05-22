using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileClaw : Missile
{
	public enum ClawState { READY, SHOOT, RETURN, CLING }
	public ClawState state { get; private set; }

	public Transform missileOrigin;

	public float maximumRange = 5.0f;
	public float returnRange = 1.0f;

	public float returnSpeed;
	public GameObject rope;

	public ResourceContainer ownerEnergyResource;
	public float energySteal; //units per second

	private Vector3 distance;
	private float distanceLength;

	public GameObject targetObject { get; private set; }
	public Unit targetUnit { get; private set; }

	protected override void Start()
	{
		Fix();
	}

	private void ResizeRope()
	{
		Vector3 unitPosition = owner.transform.position + new Vector3(0, 0.5f, 0);

		Vector3 direction = transform.position - unitPosition;
		float distance = direction.magnitude;

		rope.transform.position = (transform.position + unitPosition) * 0.5f;
		rope.transform.rotation = Quaternion.LookRotation(direction);
		rope.transform.localScale = new Vector3(1, 1, distance - 0.1f);
	}

	public void Shoot()
	{
		state = ClawState.SHOOT;

		body.constraints = RigidbodyConstraints.None;
		body.detectCollisions = true;
		body.velocity = transform.forward * speed;

		//rope.SetActive(true);
	}

	private void Fix()
	{
		state = ClawState.READY;

		transform.SetParent(missileOrigin);
		transform.position = missileOrigin.position;
		transform.rotation = missileOrigin.rotation;

		body.constraints = RigidbodyConstraints.FreezeAll;
		body.detectCollisions = false;
		body.velocity = Vector3.zero;

		//rope.SetActive(false);
	}

	private void Cling(Transform target)
	{
		state = ClawState.CLING;

		transform.SetParent(target);

		body.constraints = RigidbodyConstraints.FreezeAll;
		body.detectCollisions = false;
		body.velocity = Vector3.zero;

		targetObject = target.gameObject;
	}

	public void Return()
	{
		switch (state)
		{
			case ClawState.READY:
			case ClawState.RETURN:
				return;

			case ClawState.SHOOT:
			case ClawState.CLING:
			{
				state = ClawState.RETURN;

				if (transform.parent)
				{
					transform.SetParent(null);
				}

				body.constraints = RigidbodyConstraints.None;
				body.detectCollisions = false;
				body.velocity = -distance.normalized * returnSpeed;

				targetObject = null;
				targetUnit = null;

				break;
			}
		}
	}

	protected override void Update()
	{		
		switch (state)
		{
			case ClawState.READY: ReadyUpdate(); break;
			case ClawState.SHOOT: ShootUpdate(); break;
			case ClawState.CLING: ClingUpdate(); break;
			case ClawState.RETURN: ReturnUpdate(); break;
		}
	}

	private void ReadyUpdate()
	{
	
	}

	private void ShootUpdate()
	{
		distance = transform.position - missileOrigin.position;
		distanceLength = distance.magnitude;

		RopeUpdate();
	}

	private void ClingUpdate()
	{
		distance = transform.position - missileOrigin.position;
		distanceLength = distance.magnitude;

		RopeUpdate();

		if (targetObject == null)
		{
			Return();
			return;
		}

		//transform.position = target.transform.position + clingPoint;
		//transform.rotation = Quaternion.LookRotation(distance);

		if (targetUnit)
		{
			ResourceContainer targetEnergyResource = targetUnit.GetResource(ownerEnergyResource.resource);

			if (targetEnergyResource)
			{
				float energy = energySteal * Time.deltaTime;

				if (targetEnergyResource.Have(energy))
				{
					targetEnergyResource.Spend(energy);
					ownerEnergyResource.Spend(-energy);
				}
			}
		}
	}

	private void ReturnUpdate()
	{
		distance = transform.position - missileOrigin.position;
		distanceLength = distance.magnitude;

		if (distanceLength > returnRange)
		{
			body.velocity = -distance.normalized * returnSpeed;
		}
		else
		{
			Fix();
		}
	}

	private void RopeUpdate()
	{
		ResizeRope();

		if (distanceLength > maximumRange)
		{
			Return();
		}
		else
		{
			//RaycastHit raycast;
			//int layerMask = GameLayer.DefaultMask | GameLayer.UnitsMask;
			//if (Physics.Raycast(new Ray(transform.position, -distance), out raycast, distanceLength, layerMask))
			//{
			//	state = ClawState.RETURN;
			//	owner.tool1.Shoot(raycast.point, Vector3.zero); //electric explosion at contact point
			//}
		}
	}

	protected override void OnTriggerEnter(Collider other)
	{
		switch (state)
		{
			case ClawState.READY:
			case ClawState.RETURN:
			case ClawState.CLING:
				return;

			case ClawState.SHOOT:
			{
				break;
			}
		}

		if (other.gameObject == owner.gameObject) { return; }

		Cling(other.transform);

		switch (other.gameObject.layer)
		{
			case GameLayer.Units:
			{
				targetUnit = other.GetComponent<Unit>();
				targetUnit.ReceiveDamage(damage, damageType);
				break;
			}

			case GameLayer.Items:
			{
				break;
			}

			case GameLayer.Missiles:
			{
				break;
			}

			default: return;
		}
	}
}
