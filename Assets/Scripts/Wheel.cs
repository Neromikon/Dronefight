using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
	public enum WheelType
	{
		CENTER,
		FRONT_LEFT,
		FRONT_RIGHT,
		BACK_LEFT,
		BACK_RIGHT,
		FRONT_MIDDLE
	}

	public WheelType type;
	public int collisions = 0;

	private Unit unit;
	//private int collisions = 0;
	private new WheelCollider collider;

	// Use this for initialization
	void Start ()
	{
		for(Transform find = transform.parent;  find != null; find = find.parent)
		{
			unit = find.GetComponent<Unit>();
			if(unit != null) { break; }
		}

		collider = GetComponent<WheelCollider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//switch(type)
		//{
		//	case WheelType.CENTER: unit.centerWheel = (collisions > 0); break;
		//	case WheelType.FRONT_LEFT: unit.frontLeftWheel = (collisions > 0); break;
		//	case WheelType.FRONT_RIGHT: unit.frontRightWheel = (collisions > 0); break;
		//	case WheelType.BACK_LEFT: unit.backLeftWheel = (collisions > 0); break;
		//	case WheelType.BACK_RIGHT: unit.backRightWheel = (collisions > 0); break;
		//	case WheelType.FRONT_MIDDLE: unit.frontMiddleWheel = (collisions > 0); break;
		//}

		if(true == false &&
			unit != null && collider != null)
		{
			if(unit.move_direction.y != 0)
			{
				//Debug.Log("GO!");
				collider.motorTorque = unit.move_direction.y * 100.0f;
				collider.brakeTorque = 0.0f;
			}
			else
			{
				//Debug.Log("STOP!");
				collider.motorTorque = 0.0f;
				collider.brakeTorque = 10.0f;
			}

			if(type == WheelType.FRONT_LEFT || type == WheelType.FRONT_RIGHT)
			{
				if(unit.move_direction.x > 0)
				{
					//Debug.Log("TURN RIGHT!");
					collider.steerAngle = 60.0f;
				}
				else if(unit.move_direction.x < 0)
				{
					//Debug.Log("TURN LEFT!");
					collider.steerAngle = -60.0f;
				}
				else
				{
					//Debug.Log("TURN FORWARD!");
					collider.steerAngle = 0.0f;
				}
			}
		}
	}

	//private void OnTriggerEnter(Collider other)
	//{
	//	if(other.gameObject == unit.gameObject) { return; }
	//	if(type == WheelType.FRONT_MIDDLE && other.gameObject.layer == GameLayer.unit) { return; }
	//	collisions++;
	//}

	//private void OnTriggerExit(Collider other)
	//{
	//	if(other.gameObject == unit.gameObject) { return; }
	//	if(type == WheelType.FRONT_MIDDLE && other.gameObject.layer == GameLayer.unit) { return; }
	//	collisions--;
	//}
}
