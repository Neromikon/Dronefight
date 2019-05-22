using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Destructible : MonoBehaviour
{
	public GameObject explosion;
	public GameObject puddle;
	public List<GameObject> drop;
	public float state;

	public Defence defence;

	void Start ()
	{
		state = 100.0f;

		SetupRandomDrop();
	}
	
	void Update ()
	{
		
	}

	public void Destroy()
	{
		if(explosion) { Instantiate(explosion, transform.position, Quaternion.identity); }
		if(puddle) { Instantiate(explosion, transform.position, Quaternion.identity); }

		foreach(Transform child in transform)
		{
			Item containingItem = child.GetComponent<Item>();
			if(!containingItem) { continue; }

			containingItem.transform.SetParent(transform.parent);
			containingItem.transform.position = transform.position;
			containingItem.gameObject.SetActive(true);
		}

		Destroy(gameObject);
	}

	public void ReceiveDamage(float damage, DamageType damageType)
	{
		state -= defence.ReduceDamage(damage, damageType);

		if(state <= 0)
		{
			Destroy();
		}
	}

	public void SetupRandomDrop(int count = 1)
	{
		if(drop.Count <= 0) { return; }

		System.Random random = new System.Random();
		for(int i = 0; i < count; i++)
		{
			int r = random.Next(0, drop.Count-1);
			GameObject result = Instantiate(drop[r], transform);
			result.SetActive(false);
		}
	}
}
