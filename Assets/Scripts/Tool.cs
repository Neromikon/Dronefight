using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tool: MonoBehaviour
{
	//public enum State
	//{
	//	READY, PREPARE, APPLY, RELOAD, STUN
	//}

	public bool active;
	public Unit owner;
	
	public Sprite sprite;
	public float range = 0;

	//public float prepareDuration = 0.5f; //time for prepare state
	//public float applyDuration = 0.5f; //time for apply state
	//public float reloadDuration = 0.5f; //time for reload state

	//protected bool lockState = false;
	//protected bool repeatApply = false;
	protected Defence defence;
	//protected State currentState;

	//private Support.Timer stateTimer;
	
	public void Stun(float seconds)
	{
		//if (currentState == State.STUN)
		//{
		//	stateTimer.Add(seconds);
		//}
		//else
		//{
		//	stateTimer.Start(seconds);
		//	currentState = State.STUN;
		//	OnStun();
		//}
	}

	public virtual void ReceiveDamage(float damage, DamageType damageType)
	{
		//hp -= defence.ReduceDamage(damage, damageType);
		//if(hp < 0) { hp = 0; }
	}

	//protected virtual void ReadyUpdate() { }
	//protected virtual void PrepareUpdate() { }
	//protected virtual void ApplyUpdate() { }
	//protected virtual void ReloadUpdate() { }
	//protected virtual void StunUpdate() { }

	//protected virtual void OnPrepare() { }
	//protected virtual void OnApply() { }
	//protected virtual void OnReload() { }
	//protected virtual void OnReady() { }
	//protected virtual void OnStun() { }

	public virtual void Singleclick(){}
	public virtual void Doubleclick(){}
	public virtual void Push(){}
	public virtual void Hold(){}
	public virtual void Release(){}
	public virtual void Omit(){}

	public void React(Key_class key)
	{
		switch (key.action)
		{
			case Key_class.Action.PRESS: Push(); break;
			case Key_class.Action.RELEASE: Release(); break;
			case Key_class.Action.OMIT:
			{
				if (key.clicks == 1)
				{
					Singleclick();
				}
				else if (key.clicks == 2)
				{
					Doubleclick();
				}

				break;
			}
		}

		if (key.state)
		{
			if (key.hold_time > Key_class.CLICK_TIME)
			{
				Hold();
			}
		}
		else
		{
			if (key.release_time > Key_class.CLICK_TIME)
			{
				Omit();
			}
		}
	}
}