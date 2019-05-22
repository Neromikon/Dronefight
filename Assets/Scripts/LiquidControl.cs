using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidControl : MonoBehaviour
{
	void Start ()
	{
		
	}
	
	void Update ()
	{
		Liquid[] liquids = FindObjectsOfType<Liquid>();

		for(uint i = 0; i < liquids.Length; i++)
		{
			for (uint j = i + 1; j < liquids.Length; j++)
			{
				Vector3 direction = liquids[j].transform.position - liquids[i].transform.position;

				float d = direction.sqrMagnitude;

				Rigidbody rigibody1 = liquids[i].GetComponent<Rigidbody>();
				Rigidbody rigibody2 = liquids[j].GetComponent<Rigidbody>();

				if (d < (0.9f * 0.9f))
				{
					if (d > (0.4f * 0.4f))
					{
						rigibody1.AddForce(direction.normalized * 0.5f);
						rigibody2.AddForce(-direction.normalized * 0.5f);
						//rigibody1.velocity += -direction.normalized * 0.5f;
						//rigibody2.velocity += direction.normalized * 0.5f;
					}
					else if (d < (0.05f * 0.05f))
					{
						rigibody1.AddForce(-direction.normalized * 0.5f);
						rigibody2.AddForce(direction.normalized * 0.5f);
						//rigibody1.velocity += direction.normalized * 0.5f;
						//rigibody2.velocity += -direction.normalized * 0.5f;
					}
				}
			}
		}
	}
}
