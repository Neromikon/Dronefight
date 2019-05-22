using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMagnet : Missile
{
	public enum MagnetState {SHOOT, RETURN}

	public float returnSpeed = 25;
	public float catchDistance = 0.5f;

	public float repellDistance = 2.5f;
	public float repellPower = 5f;
	public float attractPower = 4f;

	private MagnetState state;
	private Item carryItem = null;

	private new void Start ()
	{
		base.Start();

		state = MagnetState.SHOOT;

		if(speed == 0){speed = 10.0f;}
		if(returnSpeed == 0){returnSpeed = speed * 2.0f;}

		body = GetComponent<Rigidbody>();
		if(body != null)
		{
			body.velocity = transform.forward * speed;
		}
		else
		{
			Debug.Log("Body for magnet missile not created");
		}
	}

	private new void Update()
	{
		if(state == MagnetState.SHOOT)
		{
			base.Update();
		}

		else if(state == MagnetState.RETURN)
		{
			//<+> here better would be to use tool.missile1StartPoint
			Vector3 direction = (transform.position - owner.transform.position);
			if(direction.magnitude <= catchDistance)
			{
				owner.Pick(carryItem);
				GameObject.Destroy(this.gameObject);
				return;
			}
			body.velocity = -direction.normalized * returnSpeed;
		}
	}

	protected override void OnTriggerEnter(Collider other)
	{
		if(state == MagnetState.RETURN){return;}

		switch(other.gameObject.layer)
		{
			case GameLayer.Units:
			{
				Unit targetUnit = other.GetComponent<Unit>();
				if(targetUnit != null && owner != targetUnit)
				{
					float distance = (owner.transform.position - targetUnit.transform.position).magnitude;
					if(distance <= repellDistance)
					{
						targetUnit.body.velocity += owner.view_direction * repellPower;
					}
					else
					{
						targetUnit.body.velocity += -owner.view_direction * attractPower;
					}

					GameObject.Destroy(this.gameObject);
					Debug.Log("Missile deleted by collision");
					return;
				}
				break;
			}

			case GameLayer.Items:
			{
				Item targetItem = other.GetComponent<Item>();
				if(targetItem != null && targetItem.magnetic)
				{
					state = MagnetState.RETURN;
					carryItem = targetItem;
					carryItem.transform.SetParent(transform);
					carryItem.transform.localPosition = Vector3.zero;
					if(carryItem.body)
					{
						carryItem.body.detectCollisions = false;
						carryItem.body.useGravity = false;
						carryItem.body.constraints = RigidbodyConstraints.FreezeAll;
					}
					return;
				}
				break;
			}
		}
		
	}
}
