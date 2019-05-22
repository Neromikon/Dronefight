using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBumerang : Missile
{
	public enum State {SHOOT, RETURN, WAIT}
	public State state;
	public float returnSpeed;
	public float minReturnDistance;

	private Unit targetUnit;
	private Vector3 unitPosition;
	private Vector3 bumerangPosition;
	private Vector3 direction;
	private float distance;


	new void Start ()
	{
		if(speed == 0){speed = 10.0f;}
		if(returnSpeed == 0){returnSpeed = speed * 2.0f;}

		body = GetComponent<Rigidbody>();
		if(body != null)
		{
			body.velocity = transform.forward * speed;
		}
		else
		{
			Debug.Log("Body for claw missile not created");
		}

		state = State.SHOOT;
	}
	
	new void Update()
	{
		Vector3 unitPosition = owner.transform.position + new Vector3(0, 0.5f, 0);
		Vector3 bumerangPosition = transform.position;
		Vector3 direction = bumerangPosition - unitPosition;
		float distance = direction.magnitude;

		if(state == State.SHOOT)
		{
		}
		else if(state == State.RETURN)
		{
			if(distance < 1.0f)
			{
				gameObject.SetActive(false);
				return;
			}

			if(body != null)
			{
				body.velocity = -direction.normalized * returnSpeed;
			}
		}
		else if(state == State.WAIT)
		{
			if(body != null)
			{
				//body.useGravity = true;
				body.velocity = Vector3.zero;
			}
		}
	}

	protected override void OnTriggerEnter(Collider other)
	{
		if(state == State.WAIT){return;}

		GameObject target = other.gameObject;

		if(target.layer == GameLayer.Units)
		{
			targetUnit = other.GetComponent<Unit>();
			if(targetUnit != null)
			{
				targetUnit.ReceiveDamage(damage, damageType);
			}
		}

		if(state == State.SHOOT) { state = State.WAIT; }
		if(state == State.RETURN) { state = State.WAIT; }
	}

	public void Return()
	{
		if(distance < minReturnDistance)
		{
			state = State.RETURN;
		}
	}
}
