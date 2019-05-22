using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Puddle : MonoBehaviour
{
	public const float areaEvaporationFactor = 0.95f;

	public float radius;
	public Rigidbody body;
	public GameObject effect;
	public float evaporation = 0;

	private ParticleSystem particles = null;
	//private int index;
	public int index;

	//private static List<Puddle> puddles;
	private static Puddle[] puddles = {null, null, null, null, null, null, null, null};
	private static int puddlesCount = 0;
	private static int addSearchStart = 0;
	private static int first = 0;
	private static int last = 0;

	public float Area()
	{
		return radius * radius * (float)Math.PI;
	}

	public static void Create(GameObject prefab, Vector3 position, float amount)
	{
		if(amount <= 0)
		{
			Debug.Log("Failed to create puddle with zero amount");
			return;
		}

		GameObject newPuddle = Instantiate(prefab);
		Puddle puddleData = newPuddle.GetComponent<Puddle>();
		if(puddleData == null)
		{
			Debug.Log("Failed to get puddle script data on puddle creation");
			Destroy(newPuddle);
			return;
		}

		newPuddle.transform.position = position;

		puddleData.radius = (float)Math.Sqrt(amount / (float)Math.PI);

		newPuddle.transform.localScale = new Vector3(puddleData.radius, puddleData.radius, puddleData.radius);
	}

	// Use this for initialization
	void Start ()
	{
		Debug.Log("New puddle radius is " + radius);

		body = GetComponent<Rigidbody>();

		particles = GetComponentInChildren<ParticleSystem>();
		if(particles == null)
		{
			Debug.Log("New puddle has no particle system");
		}

		UpdateSize();

		Add(this);
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
		if(evaporation > 0)
		{
			float area = Area();
			area -= evaporation * (1.0f + area * areaEvaporationFactor) * Time.deltaTime;
			radius = (float)Math.Sqrt(area / (float)Math.PI);

			if(area <= 0)
			{
				Destroy();
				return;
			}
		
			UpdateSize();
		}
	}

	public void UpdateSize()
	{
		transform.localScale = new Vector3(radius, 1.0f, radius);

		if(particles != null)
		{
			ParticleSystem.ShapeModule shape = particles.shape;
			shape.radius = radius;
		}
	}

	public void Destroy()
	{
		Support.RemoveParticles(transform);
		Remove();
		GameObject.Destroy(gameObject);
	}

	public void Add(Puddle puddle)
	{
		{
			Debug.Log("Adding new puddle (current puddle count is " + puddlesCount + ")");

			int i;
			for(i = addSearchStart; i < puddles.Length; i++)
			{
				if(puddles[i] == null)
				{
					puddles[i] = puddle;
					index = i;
					if(i > last){last = i;}
					if(i < first){first = i;}
					addSearchStart = i + 1;
					break;
				}
			}

			if(i >= puddles.Length)
			{
				last = puddles.Length;
				Array.Resize<Puddle>(ref puddles, puddles.Length * 2);
				puddles[last] = puddle;
				index = last;
				addSearchStart = last + 1;

				for(i = last+1; i < puddles.Length; i++)
				{
					puddles[i] = null;
				}
			}

			puddlesCount++;
		}

		Restart:
		Debug.Log(
			"Puddles info:   " +
			"length: " + puddles.Length + ";  " +
			"first: " + first + ";  " +
			"last: " + last + ";  " +
			"add search start: " + addSearchStart);

		for(int i = first; i <= last; i++)
		{
			if(puddles[i] == null){continue;}

			for(int j = i+1; j <= last; j++)
			{
				if(puddles[j] == null){continue;}

				float distance = (puddles[i].transform.position - puddles[j].transform.position).magnitude;
			
				if(distance <= puddles[i].radius)
				{
					puddles[i].Consume(puddles[j]);
					goto Restart;
				}

				if(distance <= puddles[j].radius)
				{
					puddles[j].Consume(puddles[i]);
					goto Restart;
				}
			}
		}
	}

	public void Remove()
	{
		Remove(index);
	}

	private static void Remove(int puddleIndex)
	{
		Debug.Log("Deleting puddle #" + puddleIndex + " (current puddle count is " + puddlesCount + ")");

		puddles[puddleIndex] = null;

		puddlesCount--;

		if(puddleIndex == last)
		while(last >= 0 && puddles[last] == null)
		{
			last--;
		}

		if(last < addSearchStart)
		{
			addSearchStart = last + 1;
		}
	}

	private void Consume(Puddle other)
	{
		float totalArea = Area() + other.Area();

		radius = (float)Math.Sqrt(totalArea / (float)Math.PI);
		UpdateSize();

		other.Destroy();
	}

	private void OnTriggerStay(Collider other)
	{
		//Debug.Log(other.name + " stays in " + name);

		if(other.gameObject.layer != GameLayer.Units) { return; }
		Unit target = other.GetComponent<Unit>();
		if(target == null) { return; }
		if(target.isFlying) { return; }

		target.AddEffect(effect);
	}
}