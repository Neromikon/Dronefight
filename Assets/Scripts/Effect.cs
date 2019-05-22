using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
	public float duration = 1;
	public float power = 1;
	public Unit target;

	private float lifetime;


	public virtual void Start ()
	{
		lifetime = duration;
	}
	

	public virtual void Update ()
	{
		if(duration <= 0){return;}

		lifetime -= Time.deltaTime;
		if(lifetime <= 0)
		{
			Support.RemoveParticles(transform);
			Cancel();
			Destroy(gameObject);
		}
	}

	public virtual void Apply(Unit target)
	{
		this.target = target;
	}

	//public virtual void Cancel(Unit target)
	public void Cancel()
	{
		//Debug.Log("Effect " + name + " cancelled from unit " + target.name);
		transform.SetParent(null);
		if(target)
		{
			target.ResetParameters();
			target.ApplyEffects();
			this.target = null;
		}
		Support.RemoveParticles(transform);
		Destroy(gameObject);
	}
}
