using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSticker : MonoBehaviour
{
	private Rigidbody rigidbody = null;
	public float viscidity = 0.5f;

	void Start ()
	{
		rigidbody = GetComponentInParent<Rigidbody>();
	}
	
	//void Update ()
	//{
	//}

	private void OnTriggerStay(Collider other)
	{
		if (viscidity > 0)
		{
			rigidbody.AddForce(viscidity * (other.transform.position - transform.position));
		}
	}
}
