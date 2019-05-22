using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingTool : Tool
{
	public enum State { READY, DELAY, CHARGE, RELOAD }

	public bool clickToActivate = false;
	public float initialDelay = 0.0f; //seconds
	public float reloadDuration = 1.0f; //seconds

	private State currentState = State.READY;
	private Support.Timer stateTimer;
	
    void Update()
    {
		switch (currentState)
		{
			case State.READY: ReadyUpdate(); break;

			case State.DELAY:
			{
				DelayUpdate();

				stateTimer.Update();

				if (stateTimer.Expired) { currentState = State.CHARGE; }

				break;
			}

			case State.CHARGE: ChargeUpdate(); break;

			case State.RELOAD:
			{
				ReloadUpdate();

				stateTimer.Update();

				if (stateTimer.Expired) { currentState = State.READY; }

				break;
			}
		}
    }

	protected virtual void ReadyUpdate() { }
	protected virtual void DelayUpdate() { }
	protected virtual void ChargeUpdate() { }
	protected virtual void ReloadUpdate() { }
	protected virtual void Action() { }
	protected virtual bool ChargeCondition() { return true; }
	protected virtual bool ActionCondition() { return true; }

	public override void Hold()
	{
		if (currentState != State.READY) { return; }
		if (!ChargeCondition()) { return; }
		
		if (initialDelay > 0)
		{
			currentState = State.DELAY;
			stateTimer.Start(initialDelay);
		}
		else
		{
			currentState = State.CHARGE;
		}
	}

	public override void Release()
	{
		switch (currentState)
		{
			case State.DELAY:
			{
				currentState = State.READY;
				break;
			}

			case State.CHARGE:
			{
				if (clickToActivate)
				{
					currentState = State.READY;
					break;
				}
				else
				{
					Action();

					if (reloadDuration > 0)
					{
						currentState = State.RELOAD;
						stateTimer.Start(reloadDuration);
					}
					else
					{
						currentState = State.READY;
					}
				}

				break;
			}
		}
	}

	public override void Singleclick()
	{
		if (!clickToActivate) { return; }

		if (!ActionCondition()) { return; }

		switch (currentState)
		{
			case State.READY:
			case State.CHARGE:
			{
				Action();

				if (reloadDuration > 0)
				{
					currentState = State.RELOAD;
					stateTimer.Start(reloadDuration);
				}
				else
				{
					currentState = State.READY;
				}

				break;
			}
		}
		
		if (currentState == State.CHARGE && clickToActivate)
		{
			
		}
	}
}
