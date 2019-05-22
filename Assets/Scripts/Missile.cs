using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Missile : MonoBehaviour
{
	public enum ScalingType { NO_DYNAMIC_SCALING, SCALE_WITH_TIME, SCALE_WITH_DISTANCE }

	public float damage;
	public DamageType damageType;
	public Rigidbody body;
	public float speed;
	public float verticalSpeed;
	public Unit owner;
	public float timeToLive = 0;
	public float distanceToTravel = 0;
	public Liquid liquid = null;
	public float minLiquidVolume;
	public float maxLiquidVolume;
	public GameObject effect = null;
	public GameObject explosion = null;

	public ScalingType scalingType = ScalingType.NO_DYNAMIC_SCALING;
	public float initialScale = 1.0f;
	public float finalScale = 1.0f;

	protected Support.Timer lifeTimer;
	protected Vector3 startingPoint;
	protected float traveledDistance;
	public ParticleSystem particles = null;

	public UnityEvent onHit;
	
	protected virtual void Start()
	{
		body = GetComponent<Rigidbody>();

		particles = GetComponentInChildren<ParticleSystem>();
		if (particles == null)
		{
			//Debug.Log("Missile " + name + " don't have particle system");
		}

		Launch();
	}

	public void Launch()
	{
		if(body != null)
		{
			//body.velocity = transform.forward * speed - Physics.gravity.normalized * verticalSpeed;
			body.velocity = transform.forward * speed;
		}

		if (timeToLive > 0) { lifeTimer.Start(timeToLive); }

		startingPoint = transform.position;

		switch (scalingType)
		{
			case ScalingType.SCALE_WITH_TIME:
			case ScalingType.SCALE_WITH_DISTANCE:
			{
				transform.localScale = Vector3.one * initialScale;
				break;
			}
		}
	}
	
	// Update is called once per frame
	protected virtual void Update()
	{
		if(transform.position.magnitude > 100.0f)
		{
			Destroy();
			Debug.Log("Missile " + name + " deleted by total distance");
			return;
		}

		if (timeToLive > 0)
		{
			lifeTimer.Update();

			if (lifeTimer.Expired)
			{
				if (liquid != null && maxLiquidVolume > 0)
				{
					float volume = Random.Range(minLiquidVolume, maxLiquidVolume);
					Liquid.Create(liquid.gameObject, transform.position, volume);
				}

				Destroy();
				Debug.Log("Missile " + name + " deleted by time to live");

				return;
			}
		}

		if (distanceToTravel > 0)
		{
			traveledDistance = (startingPoint - transform.position).magnitude;

			if(traveledDistance >= distanceToTravel)
			{
				if (liquid != null && maxLiquidVolume > 0)
				{
					float volume = Random.Range(minLiquidVolume, maxLiquidVolume);
					Liquid.Create(liquid.gameObject, transform.position, volume);
				}

				Destroy();
				Debug.Log("Missile " + name + " deleted by distance to travel (starting poist is " + startingPoint +
					", now at " + transform.position + ", distance to travel is " + distanceToTravel + ")");
				return;
			}
		}

		switch (scalingType)
		{
			case ScalingType.SCALE_WITH_TIME:
			{
				float percentage = lifeTimer.timeLeft / timeToLive;
				transform.localScale = Vector3.one * (initialScale + (finalScale - initialScale) * percentage);
				break;
			}

			case ScalingType.SCALE_WITH_DISTANCE:
			{
				float percentage = traveledDistance / distanceToTravel;
				transform.localScale = Vector3.one * (initialScale + (finalScale - initialScale) * percentage);
				break;
			}
		}
	}

	protected void Destroy()
	{
		Support.RemoveParticles(transform);

		if(explosion != null)
		{
			GameObject createdExplosion = Instantiate(explosion);
			createdExplosion.transform.position = transform.position;
		}

		GameObject.Destroy(this.gameObject);
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		const float cos45 = 0.70710678118f; //sqrt(2)/2 = 0.70710678118

		switch(other.gameObject.layer)
		{
			case GameLayer.Units:
			{
				Unit targetUnit = other.GetComponent<Unit>();
				if(targetUnit != null && targetUnit != owner)
				{
					targetUnit.DamageSide(transform.position, damage, damageType);
					
					//Vector3 direction = other.transform.position - (transform.position - new Vector3(0, transform.position.y, 0));

					//float COS = Vector3.Dot(direction.normalized, targetUnit.view_direction.normalized);

					//if(COS > cos45)
					//{
					//	targetUnit.DamageBack(damage, damageType);
					//	targetUnit.AddEffect(effect);
					//}
					//else if(COS < -cos45)
					//{
					//	targetUnit.DamageFront(damage, damageType);
					//	targetUnit.AddEffect(effect);
					//}
					//else
					//{
					//	Vector3 right_direction = new Vector3(targetUnit.view_direction.z, targetUnit.view_direction.y, -targetUnit.view_direction.x);
					//	COS = Vector3.Dot(direction.normalized, right_direction.normalized);

					//	if(COS > cos45)
					//	{
					//		targetUnit.DamageLeft(damage, damageType);
					//		targetUnit.AddEffect(effect);
					//	}
					//	else if(COS < -cos45)
					//	{
					//		targetUnit.DamageRight(damage, damageType);
					//		targetUnit.AddEffect(effect);
					//	}
					//}

					Destroy();
				}
				return;
			}


			case GameLayer.Missiles:
			{
				Missile missile2 = other.GetComponent<Missile>();
				if(missile2 != null && missile2.owner != owner)
				{
					Destroy();
					Debug.Log("Missile " + name + " deleted by collision with another missile");
				}
				return;
			}

			case GameLayer.Default:
			{
				//if(enabled == false) { Debug.Log("Disabled missile " + name + " going to be destroyed"); }
				Destroy();
				Debug.Log("Missile " + name + " deleted by collision with " + other.name + " in default layer");
				return;
			}

			case GameLayer.Destructibles:
			{
				Destructible targetDestructible = other.GetComponent<Destructible>();
				if(!targetDestructible) { return; }
				targetDestructible.ReceiveDamage(damage, damageType);
				Destroy();
				return;
			}

			case GameLayer.DamageReceivers:
			{
				DamageReceiver receiver = other.GetComponent<DamageReceiver>();

				if (receiver.owner != owner)
				{
					receiver.ReceiveDamage(damage, damageType);
					Destroy();
				}

				return;
			}
		}

		onHit.Invoke();
	}

	public void SetOwner(Unit newOwner)
	{
		owner = newOwner;
	}

	public void ReceiveDamage(float receivedDamage, DamageType receivedDamageType)
	{
	
	}
}
