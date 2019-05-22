using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
	public const float EVAPORATION_FACTOR = 0.95f;
	public const float MIN_VOLUME = 0.001f;
	public const float MAX_VOLUME = 0.1f;
	public static readonly float MAX_RADIUS = Mathf.Pow(3.0f * MAX_VOLUME / (Mathf.PI * 4.0f), 1.0f / 3.0f);

	public float radius;
	public float evaporation = 0;

	private float volume;
	private ParticleSystem particles = null;

	void Start ()
	{
		particles = GetComponentInChildren<ParticleSystem>();

		if (radius > MAX_RADIUS)
		{
			radius = MAX_RADIUS;
		}

		volume = (4.0f / 3.0f) * Mathf.PI * radius * radius * radius;
	}
	
	void Update ()
	{
		if (evaporation > 0)
		{
			volume -= evaporation * (1.0f + volume * EVAPORATION_FACTOR) * Time.deltaTime;

			if (volume < MIN_VOLUME)
			{
				Destroy(gameObject);
				return;
			}

			radius = Mathf.Pow(3.0f * volume / (Mathf.PI * 4.0f), 1.0f / 3.0f);

			transform.localScale = new Vector3(radius, radius, radius);

			if (particles)
			{
				ParticleSystem.ShapeModule shape = particles.shape;
				shape.radius = radius;
			}
		}
	}

	private void OnDestroy()
	{
		//ParticleSystem particles = child.GetComponent<ParticleSystem>();
		if (particles)
		{
			particles.transform.SetParent(null);

			particles.Stop();
			ParticleSystem.EmissionModule emissionModule = particles.emission;
			emissionModule.enabled = false;

			Destroy(particles.gameObject, particles.main.startLifetime.constantMax);
		}
	}

	public static void Create(GameObject prefab, Vector3 position, float amount)
	{
		if (amount <= 0)
		{
			Debug.Log("Failed to create puddle with zero amount");
			return;
		}

		GameObject newLiquid = Instantiate(prefab);
		Liquid liquidData = newLiquid.GetComponent<Liquid>();
		if (liquidData == null)
		{
			Debug.LogError("Failed to get puddle script data on puddle creation");
			Destroy(newLiquid);
			return;
		}

		newLiquid.transform.position = position;

		liquidData.volume = amount;
		liquidData.radius = Mathf.Pow(3.0f * amount / (Mathf.PI * 4.0f), 1.0f / 3.0f);

		newLiquid.transform.localScale = new Vector3(liquidData.radius, liquidData.radius, liquidData.radius);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!gameObject.activeSelf || !collision.gameObject.activeSelf) { return; }

		if (collision.gameObject.layer == GameLayer.Liquid)
		{
			float otherRadius = collision.transform.localScale.x;

			if (radius + otherRadius < MAX_RADIUS)
			{
				radius += otherRadius;
				volume = (4.0f / 3.0f) * Mathf.PI * radius * radius * radius;

				transform.localScale = new Vector3(radius, radius, radius);

				transform.position =
					(transform.position * radius + collision.transform.position * otherRadius) /
					(radius + otherRadius);

				collision.gameObject.SetActive(false);
				Destroy(collision.gameObject);
			}
		}
	}
}
