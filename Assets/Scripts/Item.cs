using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public enum ItemType {}


public class Item : MonoBehaviour
{
	public bool pickable = true;
	public bool magnetic = false;
	public bool fragile = false; //can be destroyed if pushed by ground unit
	public bool liquid = false; //bottles are not considering as liquid, only for puddles
	public bool disposable = false; //have to be destroyed after usage
	public bool pinnable = false; //can be pinned with drill, arrow, etc

	public Sprite icon;

	public float state = 100.0f;
	public Defence defence;

	public GameObject puddle = null;
	public float puddleAmount = 0.5f;

	public GameObject explosion = null;

	public Rigidbody body;

	public GameObject halo;

	public Missile convertionToMissile;

	public bool explosionActivated { get; private set; }
	public float explosionTimeLeft { get; private set; }

	protected virtual void Start()
	{
		if (halo)
		{
			Instantiate(halo, transform.GetChild(0));
		}
	}
	
	protected virtual void Update()
	{
		if (explosionActivated)
		{
			explosionTimeLeft -= Time.deltaTime;
			if (explosionTimeLeft <= 0) { Destroy(); }
		}
	}

	public void Destroy()
	{
		Debug.Log("Item " + name + " was destroyed");

		if(puddle && puddleAmount > 0)
		{
			Puddle.Create(puddle, transform.position, puddleAmount);
		}

		if(explosion)
		{
			GameObject createdExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
		}

		Destroy(gameObject);
	}

	public void Consume()
	{
		Debug.Log("Item " + name + " was consumed (silently destroyed)");

		Destroy(gameObject);
	}

	public virtual void Pick(Unit unit)
	{
		transform.localPosition = Vector3.zero;

		if(body)
		{
			body.detectCollisions = false;
			body.useGravity = false;
			body.constraints = RigidbodyConstraints.FreezeAll;
		}
	}

	public virtual void Drop()
	{
		transform.SetParent(null);

		if(body)
		{
			body.detectCollisions = true;
			body.useGravity = true;
			body.constraints = RigidbodyConstraints.None;
		}
	}

	public virtual bool Use(Unit unit){ return false; }

	public virtual void Throw()
	{
		if (convertionToMissile)
		{
			this.enabled = false;
			convertionToMissile.enabled = true;
		}
	}

	public virtual void ReceiveDamage(float damage, DamageType damageType)
	{
		state -= defence.ReduceDamage(damage, damageType);

		if(state <= 0)
		{
			Destroy();
		}
	}

	public void Explode(float seconds)
	{
		if (seconds > 0)
		{
			explosionActivated = true;
			explosionTimeLeft = seconds;
		}
		else
		{
			Destroy();
		}
	}
}
