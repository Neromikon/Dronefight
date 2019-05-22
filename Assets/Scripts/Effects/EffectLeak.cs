using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLeak : Effect
{
	public float leak = 0.5f; //units per second
	public ResourceContainer resource;

	public float puddlePeriod = 1.5f; //seconds
	public GameObject puddle;
	public Liquid liquid;
	public float puddleAmount = 0.2f;

	private float puddleSpawnTimer = 0;

	public override void Update()
	{
		base.Update();

		resource.Spend(leak * Time.deltaTime);

		puddleSpawnTimer -= Time.deltaTime;

		while(puddleSpawnTimer <= 0)
		{
			Puddle.Create(puddle, target.transform.position, puddleAmount);
			Liquid.Create(liquid.gameObject, target.transform.position, puddleAmount);
			puddleSpawnTimer += puddlePeriod;
		}
	}
}
