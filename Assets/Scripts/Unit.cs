using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//add to each robot transform named "Plug" for the point there
//station connector have to be while robot is charging


public class Unit : MonoBehaviour
{
	public enum Shape { TALL, LONG, WIDE }

	public bool isFlying = false;
	public bool isAbleToMove = true;
	public bool isAbleToJump = true;

	public Shape shape = Shape.TALL;

	public Rigidbody body;
	//protected BoxCollider box;
	public float acceleration, friction, breaking;
	public float speed, angularSpeed;
	public float moveTorque = 50.0f;
	public float brakeTorque = 150.0f;
	public float flightSpeed = 50.0f;

	private float baseAcceleration, baseFriction, baseBreaking;
	private float baseSpeed, baseAngularSpeed;

	public Vector3 view_direction, move_direction;
	//public Vector3 legs_direction;

	public Tool tool1, tool2, tool3;

	//public float resource1, resource2, resource3;
	//public float capacity1 = 100.0f; //maximum amount of resource 1
	//public float capacity2 = 100.0f; //maximum amount of resource 2
	//public float capacity3 = 100.0f; //maximum amount of resource 3
	//public Color resource1Color, resource2Color, resource3Color;
	//public ResourceType resource1Type, resource2Type, resource3Type;

	public ResourceContainer resource1, resource2, resource3, resource4;
	public ResourceContainer motionResource;

	public float passiveConsumption; //units per second, how much motion resource spent constantly
	public float moveConsumption; //units per second, how much motion resource spent when moving

	public Sprite headSprite, bodySprite, legsSprite, storageSprite;
	
	//public float constConsumption = 0.1f; //units per second
	//public float moveConsumption = 0.2f; //units per second
	
	public Animator animator;

	public List<Effect> effects;

	public Item holdingItem;

	public GameObject target;
	public Unit targetUnit;
	public Destructible targetDestructible;
	public Item targetItem;
	public Vector3 viewPoint;
	public float targetDistance;
	
	public WheelCollider[] wheels;

	public Sprite avatar;
	public LocalizedText localizedName;
	public LocalizedText localizedDetails;
	public string localizedBackground;

	public GroundSensor bottomGroundSensor;
	public GroundSensor topGroundSensor;

	public Transform heart; //center of an object, place to apply internal explosions
	
	public bool isInitialized { get; private set; }

	public string motionAnimationParameter;
	
	protected RaycastHit viewRaycast;
	protected const float viewRaycastUpdatePeriod = 0.2f;
	protected float viewRaycastTime = 0;
	protected const float viewRaycastRange = 7.5f;

	protected bool jumpBreaksActive = false;

	private int motionAnimationParameterId;
	

	public virtual void Start()
	{
		Debug.Assert(body, "Rigid body for unit " + name + " is not assigned");
		Debug.Assert(animator, "Animator for unit " + name + " is not assigned");
		Debug.Assert(wheels.Length > 0, "Wheels for unit " + name + " are not assigned");

		motionAnimationParameterId = Animator.StringToHash(motionAnimationParameter);

		transform.position += Vector3.up * 0.02f; //fixes physics, units stuck in ground

		SaveParameters();

		isInitialized = true;
	}
	

	public virtual void Update()
	{
		//if (!isAbleToMove) { move_direction = Vector3.zero; }

		if (motionResource)
		{
			if (move_direction != Vector3.zero)
			{
				motionResource.Activate();
			}
			else
			{
				motionResource.Passivate();
			}
		}

		//const float gap = 0.01f;
		//RaycastHit raycast;
		float currentSpeed = body.velocity.magnitude;

		//failsafe
		if(transform.position.magnitude > 100.0f)
		{
			transform.position = new Vector3(0, 75, 0);
			transform.rotation = Quaternion.identity;
			body.velocity = Vector3.zero;
		}

		Position();

		move_direction.Normalize();

		if (motionResource)
		{
			if (motionResource.amount <= 0) { move_direction = Vector3.zero; } //TEMPORARY, todo better
		}

		if(move_direction != Vector3.zero && !jumpBreaksActive)
		{
			//if((frontLeftWheel && frontRightWheel) ||
			//   (backLeftWheel && backRightWheel) && true == false)
			if(true == false)
			{
				float q = Vector3.Angle(view_direction, move_direction) / 180.0f;
				float f = Vector3.Angle(body.velocity, move_direction) / 180.0f;

				float V =
					body.velocity.magnitude -
					friction * Time.deltaTime * f
					- (q > 90.0f ? breaking : 0);

				if(V < 0) { V = 0; }

				body.velocity =
					body.velocity.normalized * V +
					move_direction * acceleration * (1.0f - q) * Time.deltaTime;

				if(currentSpeed > speed)
				{
					body.velocity = body.velocity.normalized * speed;
				}
			}

			foreach (WheelCollider wheel in wheels)
			{
				wheel.motorTorque = moveTorque;
				wheel.brakeTorque = 0.0f;
			}
			
			animator.SetBool(motionAnimationParameter, true);

			if (isFlying && !bottomGroundSensor.contact)
			{
				body.AddForce(move_direction * flightSpeed);
			}
		}
		else
		{
			if(true == false)
			{
				float V = body.velocity.magnitude - friction * Time.deltaTime;
				if (V < 0) { V = 0; }
				body.velocity = body.velocity.normalized * V;
			}

			foreach (WheelCollider wheel in wheels)
			{
				if (!wheel) { continue; }
				wheel.motorTorque = 0.0f;
				wheel.brakeTorque = brakeTorque;
			}

			animator.SetBool(motionAnimationParameter, false);
		}

		if(//body.useGravity == false && true == false &&
		   move_direction != Vector3.zero && !jumpBreaksActive)
		{
			float angle = Vector3.Angle(move_direction, transform.forward);
			float maxAngle = angularSpeed * Time.deltaTime;
			if(angle > maxAngle) { angle = maxAngle; }
			Debug.DrawRay(transform.position, move_direction, Color.blue);
			Debug.DrawRay(transform.position, transform.forward, Color.white);

			float c = Vector3.Dot(move_direction, transform.right);
			if(c != 0) { c = (c > 0) ? 1 : -1; }

			Quaternion q = Quaternion.AngleAxis(angle * c, transform.up);
			view_direction = q * transform.forward;
			Debug.DrawRay(transform.position, view_direction, Color.green);

			//transform.rotation = Quaternion.LookRotation(view_direction);
			transform.rotation *= q;
		}

		view_direction = transform.forward;
		//transform.rotation = Quaternion.LookRotation(view_direction);

		//view raycast
		ViewRaycast();
	}


	private void ViewRaycast()
	{
		viewRaycastTime -= Time.deltaTime;
		if(viewRaycastTime > 0) { return; }
		viewRaycastTime = viewRaycastUpdatePeriod;

		int mask = GameLayer.DefaultMask | GameLayer.UnitsMask | GameLayer.ItemsMask | GameLayer.DestructiblesMask;

		//	viewRaycastTime = viewRaycastUpdatePeriod;

		//	Vector3 raycastStart = transform.position + new Vector3(0, 0.25f, 0.8f);
		//	Debug.DrawRay(raycastStart, transform.forward * viewRaycastRange, Color.yellow);

		//	if(Physics.Raycast( raycastStart, transform.forward, out viewRaycast, viewRaycastRange, mask))
		//	{
		//		target = viewRaycast.collider.gameObject;

		//		if(target == gameObject)
		//		{
		//			target = null;
		//			return;
		//		}

		//		viewPoint = viewRaycast.point;
		//		targetDistance = Vector3.Distance(raycastStart, viewPoint);

		//		switch(target.layer)
		//		{
		//			case GameLayer.Units: targetUnit = target.GetComponent<Unit>(); break;
		//			case GameLayer.Destructibles: targetDestructible = target.GetComponent<Destructible>(); break;
		//			case GameLayer.Items: targetItem = target.GetComponent<Item>(); break;
		//		}
		//	}
		//	else
		//	{
		//		target = null;
		//	}

		target = null;
		targetDistance = -1;
		//Collider targetCollider = new Collider();

		foreach(RaycastHit cast in Physics.BoxCastAll(
			transform.position + transform.forward * viewRaycastRange * 0.5f + transform.up * 0.51f,
			new Vector3(0.75f, 0.5f, viewRaycastRange * 0.5f),
			transform.forward,
			transform.rotation,
			Mathf.Infinity,
			mask))
		{
			//Debug.DrawLine(transform.position, transform.position + transform.forward * viewRaycastRange * 0.5f + transform.up * 2.01f, Color.gray);
			//Debug.DrawLine(transform.position, cast.point, Color.red);

			if(cast.collider.gameObject == gameObject) { continue; }
			Vector3 closestPoint = cast.collider.ClosestPoint(transform.position + transform.up * 0.5f);

			float distance = Vector3.Distance(transform.position, closestPoint);
			if(distance < targetDistance || targetDistance < 0)
			{
				targetDistance = distance;
				target = cast.collider.gameObject;
				//targetCollider = cast.collider;
				viewPoint = closestPoint;
			}
		}

		if(target)
		{
			Debug.DrawLine(transform.position, viewPoint, Color.yellow);

			switch(target.layer)
			{
				case GameLayer.Units: targetUnit = target.GetComponent<Unit>(); break;
				case GameLayer.Destructibles: targetDestructible = target.GetComponent<Destructible>(); break;
				case GameLayer.Items: targetItem = target.GetComponent<Item>(); break;
			}
		}
	}


	void Position()
	{
		//int wheelSum =
		//	Convert.ToInt32(centerWheel) +
		//	Convert.ToInt32(frontLeftWheel) +
		//	Convert.ToInt32(frontRightWheel) +
		//	Convert.ToInt32(backLeftWheel) +
		//	Convert.ToInt32(backRightWheel);

		//if(wheelSum >= 3)
		//{
		//	body.useGravity = false;
		//	body.freezeRotation = true;
		//}
		//else
		//{
		//	body.useGravity = true;
		//	body.freezeRotation = false;
		//}

		//if(frontMiddleWheel)
		//{
		//	float angle = body.velocity.magnitude * 2.0f;

		//	Vector3 back = transform.position - transform.forward * box.size.z * 0.5f;
		//	Vector3 front = transform.position + transform.forward * box.size.z * 0.5f;
		//	Vector3 direction = front - back;
		//	Quaternion q = Quaternion.AngleAxis(-angle, transform.right);
		//	direction = q * direction;

		//	transform.position = back + direction * 0.5f;
		//	transform.rotation = Quaternion.LookRotation(direction);
		//}
	}


	private void SaveParameters()
	{
		baseSpeed = speed;
		baseAcceleration = acceleration;
		baseFriction = friction;
		baseBreaking = breaking;
		baseAngularSpeed = angularSpeed;
	}


	public void ResetParameters()
	{
		//Debug.Log("Parameters of " + name + " were reset");
		speed = baseSpeed;
		acceleration = baseAcceleration;
		friction = baseFriction;
		breaking = baseBreaking;
		angularSpeed = baseAngularSpeed;
	}


	public void ApplyEffects()
	{
		//Debug.Log("Effects on " + name + " were re-applied");

		foreach(Transform child in transform)
		{
			if(child.gameObject.layer != GameLayer.Effects) { continue; }

			Effect effect = child.GetComponent<Effect>();
			if(effect == null) { continue; }

			effect.Apply(this);
		}
	}


	public Effect AddEffect(GameObject effect)
	{
		if(effect == null) { return null; }

		foreach(Transform child in transform)
		{
			if(child.name == effect.name)
			{
				Effect effectData = child.GetComponent<Effect>();
				if(effect != null) { effectData.Start(); }
				return null;
			}
		}

		Debug.Log("Effect " + effect.name + " added to " + name);

		GameObject newEffect = Instantiate(effect.gameObject);
		newEffect.name = effect.name;
		newEffect.transform.position = transform.position;
		newEffect.transform.SetParent(transform);

		Effect newEffectData = newEffect.GetComponent<Effect>();
		if(newEffectData != null) { newEffectData.Apply(this); }

		return newEffectData;
	}

	public virtual void ReceiveDamage(float damage, DamageType damageType){}

	public virtual void DamageSide(Vector3 source, float damage, DamageType damageType)
	{
		Debug.Log(name + " received " + damageType.ToString() + " " + damage + " damage");

		const float cos45 = 0.70710678118f; //sqrt(2)/2 = 0.70710678118

		Vector3 direction = transform.position - source;

		float cos = Vector3.Dot(transform.forward, direction.normalized);

		if(cos > cos45){DamageBack(damage, damageType);} else
		if(cos < -cos45){DamageFront(damage, damageType);} else
		{
			cos = Vector3.Dot(transform.right, direction.normalized);

			if(cos > cos45){DamageLeft(damage, damageType);} else
			if(cos < -cos45){DamageRight(damage, damageType);}
		}
	}

	public virtual void DamageFront(float damage, DamageType damageType) { }
	public virtual void DamageBack(float damage, DamageType damageType) { }
	public virtual void DamageLeft(float damage, DamageType damageType) { }
	public virtual void DamageRight(float damage, DamageType damageType) { }
	public virtual void DamageInside(float damage, DamageType damageType) { }
	public virtual void DamageAllSides(float damage, DamageType damageType) { }
	public virtual void DamageTop(float damage, DamageType damageType) { }
	public virtual void DamageBottom(float damage, DamageType damageType) { }

	//public void DamageHead(float damage, DamageType damageType)
	//{
	//	headState -= headDefence.ReduceDamage(damage, damageType);
	//	if(headState < 0){headState = 0;}
	//}

	//public void DamageBody(float damage, DamageType damageType)
	//{
	//	bodyState -= bodyDefence.ReduceDamage(damage, damageType);
	//	if(bodyState < 0){bodyState = 0;}
	//}

	//public void DamageLegs(float damage, DamageType damageType)
	//{
	//	legsState -= legsDefence.ReduceDamage(damage, damageType);
	//	if(legsState < 0){legsState = 0;}
	//}

	//public void DamageStorage(float damage, DamageType damageType)
	//{
	//	storageState -= storageDefence.ReduceDamage(damage, damageType);
	//	if(storageState < 0){storageState = 0;}
	//}

	public virtual void HeadDestroyed() { }
	public virtual void BodyDestroyed() { }
	public virtual void LegsDestroyed() { }
	public virtual void StorageDestroyed() { }
	public virtual void Tool1Destroyed() { }
	public virtual void Tool2Destroyed() { }
	public virtual void Tool3Destroyed() { }

	public virtual void Move(Vector2 direction)
	{
		if(!isAbleToMove) { return; }

		//move_direction = new Vector3(direction.x, 0, direction.y);

		//move_direction =
		//	Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z)) *
		//	new Vector3(direction.x, 0, direction.y);

		//float q = 1.0f / new Vector3(transform.forward.x, 0, transform.forward.z).magnitude;
		//move_direction = new Vector3(
		//	direction.x * q,
		//	transform.forward.y,
		//	direction.y * q);

		//direction.Normalize();
		//Vector3 right = Vector3.Cross(transform.up, Vector3.back);
		//Vector3 forward = Vector3.Cross(transform.up, right);
		//right = Vector3.Cross(transform.up, forward);
		//move_direction = right.normalized * direction.x + forward.normalized * direction.y;

		//move_direction = new Vector3(direction.x, direction.y, 0);

		Vector3 destination = transform.position + new Vector3(direction.x, 0, direction.y);
		Vector3 distance = destination - transform.position;
		Vector3 projection = destination - transform.up * Vector3.Dot(distance, transform.up);
		move_direction = (projection - transform.position).normalized;

		Debug.DrawLine(destination, projection, Color.grey);
	}

	public void Move(Vector3 direction)
	{
		
	}

	private void MoveUpdate()
	{
		motionResource.Spend(moveConsumption * Time.deltaTime);
	}


	public void MoveTo(Vector3 point)
	{
		Vector3 distance = point - transform.position;
		Vector3 projection = point - transform.up * Vector3.Dot(distance, transform.up);
		move_direction = (projection - transform.position).normalized;

		Debug.DrawLine(point, projection, Color.grey);
	}


	//public float Have(ResourceType resource)
	//{
	//	if(resource1Type == resource) { return resource1; }
	//	if(resource2Type == resource) { return resource2; }
	//	if(resource3Type == resource) { return resource3; }
	//	return 0;
	//}

	//public bool Have(float value, ResourceType resource)
	//{
	//	if(resource1Type == resource) { return (resource1 >= value); }
	//	if(resource2Type == resource) { return (resource2 >= value); }
	//	if(resource3Type == resource) { return (resource3 >= value); }
	//	return false;
	//}


	//public void Spend(float value, ResourceType resource)
	//{
	//	if(resource == ResourceType.NONE)
	//	{
	//		Debug.Log("Can't spend resource NONE");
	//		return;
	//	}

	//	if(resource == ResourceType.OTHER)
	//	{
	//		Debug.Log("Can't spend resource OTHER");
	//		return;
	//	}

	//	if(resource1Type == resource)
	//	{
	//		resource1 -= value;
	//		if(resource1 < 0) { resource1 = 0; }
	//		if(resource1 > capacity1) { resource1 = capacity1; }
	//		return;
	//	}

	//	if(resource2Type == resource)
	//	{
	//		resource2 -= value;
	//		if(resource2 < 0) { resource2 = 0; }
	//		if(resource2 > capacity2) { resource2 = capacity2; }
	//		return;
	//	}

	//	if(resource3Type == resource)
	//	{
	//		resource3 -= value;
	//		if(resource3 < 0) { resource3 = 0; }
	//		if(resource3 > capacity3) { resource3 = capacity3; }
	//		return;
	//	}
	//}

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Trigger happened");
		if(other.gameObject.layer == GameLayer.Items)
		{
			Item item = other.GetComponent<Item>();
			if(item != null)
			{
				if(!isFlying && item.fragile) { item.Destroy(); }
			}
		}
	}


	//private void OnTriggerExit(Collider other)
	//{
	//	
	//}

	public void Pick()
	{
		//Debug.Log("Pick ordered to " + name);

		//if(holdingItem)
		//{
		//	if(holdingItem.Use(this) && holdingItem.disposable)
		//	{
		//		holdingItem.Consume();
		//		holdingItem = null;
		//	}
		//	//Drop();
		//	return;
		//}

		//if(grab == null) { return; }

		//grab.gameObject.SetActive(true);
	}

	public void Drop()
	{
		if(!holdingItem) { return; }

		holdingItem.Drop();
		holdingItem = null;
	}

	public virtual void Pick(Item item)
	{
		//Debug.Log(name + " picked an item " + item.name);

		//if(hold == null) { return; }

		//item.transform.SetParent(hold);
		//item.Pick(this);

		//if(item.Use(this) && item.disposable)
		//{
		//	item.Consume();
		//}
		//else
		//{
		//	holdingItem = item;
		//}
	}

	public ResourceContainer GetResource(Resource resourceType)
	{
		if (resource1.resource == resourceType) { return resource1; }
		if (resource2.resource == resourceType) { return resource2; }
		if (resource3.resource == resourceType) { return resource3; }
		if (resource4.resource == resourceType) { return resource4; }
		return null;
	}

	public bool HaveResource(Resource resourceType, float amount)
	{
		if (resource1.resource == resourceType) { return resource1.Have(amount); }
		if (resource2.resource == resourceType) { return resource2.Have(amount); }
		if (resource3.resource == resourceType) { return resource3.Have(amount); }
		if (resource4.resource == resourceType) { return resource4.Have(amount); }
		return false;
	}

	public void Play(int animationId) { animator.Play(animationId); }
	public void Play(string animationName, int layer, float normalizedTime) { animator.Play(animationName, layer, normalizedTime); }

	public void PlayTransition(string animationName, float normalizedTransitionDuration) { animator.CrossFade(animationName, normalizedTransitionDuration); }
	public bool IsPlayingTransition(int layer) { return animator.IsInTransition(layer); }

	public void RestartAnimationInLayer(int layerIndex) { animator.Play(animator.GetCurrentAnimatorStateInfo(layerIndex).shortNameHash); }

	public void PlayMoment(int animationId, int layer, float normalizedTime) { animator.Play(animationId, layer, normalizedTime); }

	public bool HaveAnimationParameter(string parameterName)
	{
		foreach (AnimatorControllerParameter parameter in animator.parameters)
		{
			if (parameter.name == parameterName) { return true; }

			//note: animator parameters hash codes does not correspond to parameter names hash codes
			//Debug.Log("animatior string to hash:" + Animator.StringToHash(parameterName));
			//Debug.Log("parameter hash:" + parameter.GetHashCode());
		}

		return false;
	}

	public int GetAnimatorParameterId(string parameterName) { return Animator.StringToHash(parameterName); }
	public void SetAnimationLayerWeight(int layerIndex, float weight) { animator.SetLayerWeight(layerIndex, weight); }
	public float GetAnimationLayerWeight(int layerIndex) { return animator.GetLayerWeight(layerIndex); }

	public void SetAnimatorParameter(int id, bool value) { animator.SetBool(id, value); }
	public void SetAnimatorParameter(int id, int value) { animator.SetInteger(id, value); }
	public void SetAnimatorParameter(int id, float value) { animator.SetFloat(id, value); }

	public virtual void JumpPress() { }
	public virtual void JumpRelease() { }
	public virtual void JumpSingleClick() { }
	public virtual void JumpDoubleClick() { }
	public virtual void JumpHold() { }
	public virtual void JumpOmit() { }

	public virtual void PickPress() { }
	public virtual void PickRelease() { }
	public virtual void PickSingleClick() { }
	public virtual void PickDoubleClick() { }
	public virtual void PickHold() { }
	public virtual void PickOmit() { }

	public void PickReact(Key_class key)
	{
		switch (key.action)
		{
			case Key_class.Action.PRESS: PickPress(); break;
			case Key_class.Action.RELEASE: PickRelease(); break;
			case Key_class.Action.OMIT:
			{
				if (key.clicks == 1)
				{
					PickSingleClick();
				}
				else if (key.clicks == 2)
				{
					PickDoubleClick();
				}

				break;
			}
		}

		if (key.state)
		{
			if (key.hold_time > Key_class.CLICK_TIME)
			{
				PickHold();
			}
		}
		else
		{
			if (key.release_time > Key_class.CLICK_TIME)
			{
				PickOmit();
			}
		}
	}

	public void JumpReact(Key_class key)
	{
		if (!isAbleToJump) { return; }

		switch (key.action)
		{
			case Key_class.Action.PRESS: JumpPress(); break;
			case Key_class.Action.RELEASE: JumpRelease(); break;
			case Key_class.Action.OMIT:
			{
				if (key.clicks == 1)
				{
					JumpSingleClick();
				}
				else if (key.clicks == 2)
				{
					JumpDoubleClick();
				}

				break;
			}
		}

		if (key.state)
		{
			if (key.hold_time > Key_class.CLICK_TIME)
			{
				JumpHold();
			}
		}
		else
		{
			if (key.release_time > Key_class.CLICK_TIME)
			{
				JumpOmit();
			}
		}
	}

	protected void OnLegsDestroyed()
	{
		isAbleToMove = false;
		isAbleToJump = false;
	}

	protected void OnTool1Destroyed()
	{
		tool1 = null;
	}

	protected void OnTool2Destroyed()
	{
		tool2 = null;
	}

	protected void OnTool3Destroyed()
	{
		tool3 = null;
	}
}