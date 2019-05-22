using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHybrid : Missile
{
	protected override void Start()
	{
		//setup components
		body = GetComponent<Rigidbody>();

		particles = GetComponentInChildren<ParticleSystem>();
		if(particles == null)
		{
			Debug.Log("Missile " + name + " don't have particle system");
		}

		//check if missile is hybridic
		if(GetComponent<Item>())
		{
			Debug.Log("Missile " + name + " disabled as an item-hybrid");
			this.enabled = false;
			return;
		}

		//launch missile
		Launch();
	}

	protected override void Update()
	{
		if(gameObject.layer != GameLayer.Missiles) { return; }

		base.Update();
	}

	protected override void OnTriggerEnter(Collider other)
	{
		if(gameObject.layer != GameLayer.Missiles) { return; }

		base.OnTriggerEnter(other);
	}
}
