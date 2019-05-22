using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWeb : Missile
{
	Missile []endings;

	protected override void OnTriggerEnter(Collider other)
	{
		//spawn many minimissiles to different directions
		//they should collide only with default layer
		//filter missiles that hit the target too close
		//take 2/3/4/5/6 random successfull missiles and create a web
		//if less than 2 successfull missiles then create web bulb instead

		foreach (Missile missile in endings)
		{
			missile.transform.position = transform.position;
			//missile.transform.rotation = ;
			missile.gameObject.SetActive(true);
		}


		//Unit unit = other.GetComponent<Unit>();
		//if (unit != null)
		//{
		//	unit.ReceiveDamage(damage, damageType);
		//}
		//GameObject.Destroy(this.gameObject);
		//Debug.Log("Missile deleted by collision");
	}
}
