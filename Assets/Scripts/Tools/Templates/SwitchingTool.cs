using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingTool : Tool
{
	public enum State
	{
		PASSIVE, ACTIVE,
		TRANSITION_TO_ACTIVE, TRANSITION_TO_PASSIVE,
		RETURN_TO_ACTIVE, RETURN_TO_PASSIVE
	}

	public bool holdingModeAvailable = false;

	public float transitionToActiveDuration;
	public float transitionToPassiveDuration;

	private State currentState = State.PASSIVE;
	private Support.Timer stateTimer;
	private bool isHolding = false;

	void Update()
    {
		switch (currentState)
		{
			case State.ACTIVE:
			{
				ActiveUpdate();

				if (!KeepActiveCondition()) { MakeTransitionToPassive(); }

				break;
			}

			case State.PASSIVE:
			{
				PassiveUpdate();
				break;
			}

			case State.RETURN_TO_ACTIVE:
			case State.TRANSITION_TO_ACTIVE:
			{
				TransitionToActiveUpdate();

				stateTimer.Update();

				if (stateTimer.Expired) { SetActive(); }

				break;
			}

			case State.RETURN_TO_PASSIVE:
			case State.TRANSITION_TO_PASSIVE:
			{
				TransitionToPassiveUpdate();

				stateTimer.Update();

				if (stateTimer.Expired) { SetPassive(); }

				break;
			}
		}
    }

	protected virtual void ActiveUpdate() { }
	protected virtual void PassiveUpdate() { }
	protected virtual void TransitionToActiveUpdate() { }
	protected virtual void TransitionToPassiveUpdate() { }
	protected virtual void OnActivation() { }
	protected virtual void OnPassivation() { }
	protected virtual bool ActivationCondition() { return true; }
	protected virtual bool PassivationCondition() { return true; }
	protected virtual bool KeepActiveCondition() { return true; }

	public override void Singleclick()
	{
		switch (currentState)
		{
			case State.ACTIVE:
			{
				MakeTransitionToPassive();
				break;
			}

			case State.PASSIVE:
			{
				MakeTransitionToActive();
				break;
			}
		}
	}

	public override void Hold()
	{
		if (!holdingModeAvailable) { return; }
		if (isHolding) { return; }

		isHolding = true;

		switch (currentState)
		{
			case State.ACTIVE:
			{
				MakeTransitionToPassive();
				break;
			}

			case State.PASSIVE:
			{
				MakeTransitionToActive();
				break;
			}
		}
	}

	public override void Release()
	{
		if (!holdingModeAvailable) { return; }
		if (!isHolding) { return; }

		isHolding = false;

		switch (currentState)
		{
			case State.ACTIVE:
			case State.TRANSITION_TO_ACTIVE:
			{
				ReturnToPassive();
				break;
			}

			case State.PASSIVE:
			case State.TRANSITION_TO_PASSIVE:
			{
				ReturnToActive();
				break;
			}
		}
	}
	
	private void MakeTransitionToActive()
	{
		if (!ActivationCondition()) { return; }

		if (transitionToActiveDuration > 0)
		{
			currentState = State.TRANSITION_TO_ACTIVE;
			stateTimer.Start(transitionToActiveDuration);
		}
		else
		{
			SetActive();
		}
	}

	private void MakeTransitionToPassive()
	{
		if (!PassivationCondition()) { return; }

		if (transitionToPassiveDuration > 0)
		{
			currentState = State.TRANSITION_TO_PASSIVE;
			stateTimer.Start(transitionToPassiveDuration);
		}
		else
		{
			SetPassive();
		}
	}

	private void ReturnToActive()
	{
		if (!ActivationCondition()) { return; }

		if (transitionToActiveDuration > 0)
		{
			currentState = State.RETURN_TO_ACTIVE;
			stateTimer.Start(transitionToActiveDuration - stateTimer.timeLeft);
		}
		else
		{
			SetActive();
		}
	}

	private void ReturnToPassive()
	{
		if (!PassivationCondition()) { return; }

		if (transitionToPassiveDuration > 0)
		{
			currentState = State.RETURN_TO_PASSIVE;
			stateTimer.Start(transitionToPassiveDuration - stateTimer.timeLeft);
		}
		else
		{
			SetPassive();
		}
	}

	private void SetActive()
	{
		currentState = State.ACTIVE;

		OnActivation();
	}

	private void SetPassive()
	{
		currentState = State.PASSIVE;

		OnPassivation();
	}
}
