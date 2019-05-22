using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileJet : Missile
{
	public float splashPeriod = 0.2f;
	public float distanceDamageReduction = 0;
	public float range;

	private float splashReload;

	protected override void Start()
	{
		Debug.Assert(owner != null, "Jet missile must have owner");

		body = GetComponent<Rigidbody>();
		body.constraints = RigidbodyConstraints.FreezeAll;

		particles = GetComponentInChildren<ParticleSystem>();

		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		splashReload = 0;
	}

	private new void Update()
	{
		if(splashReload > 0)
		{
			splashReload -= Time.deltaTime;
			if(splashReload < 0) { splashReload = 0; }
		}

		Vector3 endPoint;

		if(owner.target && owner.targetDistance < range)
		{
			endPoint = owner.viewPoint;

			if(explosion && splashReload == 0)
			{
				Instantiate(explosion, owner.viewPoint - transform.forward * 0.05f, Quaternion.identity);
				splashReload = splashPeriod;
			}

			float dealingDamage = (damage - distanceDamageReduction * (owner.targetDistance / range)) * Time.deltaTime;

			switch(owner.target.layer)
			{
				case GameLayer.Units:
				{
					if(owner.targetUnit)
					{
						owner.targetUnit.ReceiveDamage(dealingDamage, damageType);
					}
					break;
				}

				case GameLayer.Destructibles:
				{
					if(owner.targetDestructible)
					{
						owner.targetDestructible.ReceiveDamage(dealingDamage, damageType);
					}
					break;
				}

				case GameLayer.Items:
				{
					if(owner.targetItem)
					{
						owner.targetItem.ReceiveDamage(dealingDamage, damageType);
					}
					break;
				}
			}
		}
		else
		{
			endPoint = owner.transform.forward * range;

			//if(splashReload == 0 && puddle && puddleAmount > 0)
			//{
			//	Puddle.Create(puddle, Vector3.Scale(endPoint, new Vector3(1, 0, 1)), puddleAmount);
			//	splashReload = splashPeriod;
			//}

			if (liquid && maxLiquidVolume > 0)
			{
				float volume = Random.Range(minLiquidVolume, maxLiquidVolume);
				Liquid.Create(liquid.gameObject, owner.viewPoint - transform.forward * 0.05f, volume);
			}
		}

		Vector3 direction = endPoint - transform.position;

		transform.rotation = Quaternion.LookRotation(direction);
		transform.localScale = new Vector3(1.0f, 1.0f, direction.magnitude);

		if(particles)
		{
			ParticleSystem.ShapeModule shape = particles.shape;
			shape.scale = new Vector3(shape.scale.x, shape.scale.y, transform.localScale.z);
		}
	}

	protected override void OnTriggerEnter(Collider other)
	{
	}
}
