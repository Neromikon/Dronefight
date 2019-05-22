using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMagnet: Tool
{
	//ALL TODO / REDO



	//public ObjectAffector affector;
	//public ResourceContainer energyResource;

	//public Item holdingItem = null;

	//public int maxTargets = 5;

	//public float minEnergyCost = 0.25f;
	//public float effectiveEnergyCost = 0.5f; //if at least one object touched
	
	//public float impulsePower = 14.0f;

	////public float repellDistance = 2.5f;

	//private float[] distances;

	//public override void Start()
	//{
	//	if (maxTargets > ObjectAffector.MAX_TARGETS)
	//	{
	//		Debug.LogError(name + " has too high max targets parameter, " + ObjectAffector.MAX_TARGETS + " is a maximum");
	//		maxTargets = ObjectAffector.MAX_TARGETS;
	//	}

	//	distances = new float[maxTargets];

	//	affector.affectFunction = Affect;
	//}

	//public override void Singleclick()
	//{
	//	if (!energyResource.Have(minEnergyCost + effectiveEnergyCost)) { return; } //TEMP

	//	Use();
	//}

	//protected override void Apply()
	//{
	//	base.Apply();

	//	if(holdingItem == null)
	//	{
	//		energyResource.Spend(minEnergyCost + effectiveEnergyCost); //TEMP
	//		affector.gameObject.SetActive(true);
	//	}
	//	else
	//	{
	//		//holdingItem.Throw();
	//		holdingItem = null;
	//	}
	//}

	//private void Affect(List<GameObject> targets)
	//{
	//	float totalDistance = 0.0f;

	//	int affectedTargets = Mathf.Min(maxTargets, targets.Count);
		
	//	for (int i = 0; i < affectedTargets; i++)
	//	{
	//		distances[i] = (affector.transform.position - targets[i].transform.position).sqrMagnitude;
	//		totalDistance += distances[i];
	//	}
		
	//	for (int i = 0; i < affectedTargets; i++)
	//	{
	//		Rigidbody rigidbody = targets[i].GetComponent<Rigidbody>();
			
	//		if (!rigidbody) { continue; }

	//		Vector3 direction = (affector.transform.position - rigidbody.transform.position).normalized;

	//		rigidbody.AddForce(direction * impulsePower * (distances[i] / totalDistance));
	//	}
	//}
}