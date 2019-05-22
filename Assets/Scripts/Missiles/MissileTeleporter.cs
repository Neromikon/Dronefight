using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTeleporter : Missile
{
	public float maxHeight = 4.0f;
	public GameObject chargeEffect;
	public float distanceFromContactPoint = 1.2f;

	public new void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == 0)
		{
			Destroy();
		}
	}

	private void OnDestroy()
	{
		GameObject charge1 = Instantiate(chargeEffect);
		charge1.transform.position = owner.transform.position;

		RaycastHit raycast;
		if(Physics.Raycast(new Ray(transform.position, Vector3.down), out raycast, maxHeight, 1))
		{
			owner.transform.position = raycast.point - charge1.transform.forward * distanceFromContactPoint;
		}
		else
		{
			owner.transform.position = transform.position - charge1.transform.forward * distanceFromContactPoint;
		}

		owner.transform.localScale.Scale(new Vector3(-1, 1, 1));

		GameObject charge2 = Instantiate(chargeEffect);
		charge2.transform.position = owner.transform.position;
	}
}
