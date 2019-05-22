using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	public Controller player,slayer;

	// Use this for initialization
	void Start (){}
	
	// Update is called once per frame
	void Update()
	{
		if(player == null){return;}
		if(slayer == null){return;}

		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit))
			{
				Unit u = hit.collider.GetComponent<Unit>();
				if(u != null){player.unit = u;}
			}
		}

		if(Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit))
			{
				Unit u = hit.collider.GetComponent<Unit>();
				if(u != null){slayer.unit = u;}
			}
		}
	}
}
