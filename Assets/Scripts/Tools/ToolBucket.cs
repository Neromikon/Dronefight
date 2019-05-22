using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBucket : Tool
{
	//active is when bucket is in front, passive when it's up

	public enum State { PASSIVE, ACTIVE, TRANSITION_TO_ACTIVE, TRANSITION_TO_PASSIVE }
	
	public MissileMelee meleeMissile;

	public int bucketAnimationLayer;
	public string moveDownAnimation;
	public string moveUpAnimation;

	public float moveUpDuration = 0.200f; //seconds
	public float moveDownDuration = 0.100f; //seconds
	//public float interruptReloadDuration = 0.150f; //seconds

	private State currentState = State.PASSIVE;
	
	public void Update()
	{
		switch (currentState)
		{
			case State.TRANSITION_TO_PASSIVE:
			{
				if (owner.IsPlayingTransition(bucketAnimationLayer))
				{
					currentState = State.PASSIVE;
				}
				break;
			}

			case State.TRANSITION_TO_ACTIVE:
			{
				if (!meleeMissile.isActiveAndEnabled)
				{
					MoveUp();
				}
				else if (owner.IsPlayingTransition(bucketAnimationLayer))
				{
					currentState = State.ACTIVE;
					//meleeMissile.gameObject.SetActive(false);
				}

				break;
			}

			default: break;
		}
	}

	public override void Singleclick()
	{
		switch (currentState)
		{
			case State.TRANSITION_TO_PASSIVE:
			case State.PASSIVE:
			{
				MoveDown();
				break;
			}

			case State.TRANSITION_TO_ACTIVE:
			case State.ACTIVE:
			{
				MoveUp();
				break;
			}
		}
	}

	private void MoveUp()
	{
		currentState = State.TRANSITION_TO_PASSIVE;
		owner.PlayTransition(moveUpAnimation, moveUpDuration);
	}

	private void MoveDown()
	{
		currentState = State.TRANSITION_TO_ACTIVE;
		owner.PlayTransition(moveDownAnimation, moveDownDuration);

		meleeMissile.gameObject.SetActive(true);
	}
}
