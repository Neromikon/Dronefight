using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactTool : Tool
{
	public enum State { READY, DELAY, APPLY, RELOAD }

	public float delayDuration = 1.0f; //seconds
	public float reloadDuration = 1.0f; //seconds

	public ConstantObjectSensor sensor;

	private State currentState;
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

				if (stateTimer.Expired)
				{
					OnStart();
					sensor.gameObject.SetActive(true);
					currentState = State.APPLY;
				}

				break;
			}

			case State.APPLY:
			{
				ApplyUpdate();

				if (!Condition()) { SwitchToReload(); }

				break;
			}

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
	protected virtual void ApplyUpdate() { }
	protected virtual void ReloadUpdate() { }
	protected virtual void OnStart() { }
	protected virtual void OnStop() { }
	protected virtual bool Condition() { return true; }

	public override void Push()
	{
		if (!Condition()) { return; }

		if (currentState == State.READY) { SwitchToDelay(); }
	}

	public override void Release()
	{
		switch (currentState)
		{
			case State.DELAY: currentState = State.READY; break;

			case State.APPLY:
			{
				SwitchToReload();
				break;
			}
		}
	}

	private void SwitchToDelay()
	{
		if (delayDuration > 0)
		{
			currentState = State.DELAY;
			stateTimer.Start(delayDuration);
		}
		else
		{
			OnStart();
			sensor.gameObject.SetActive(true);
			currentState = State.APPLY;
		}
	}

	private void SwitchToReload()
	{
		OnStop();
		sensor.gameObject.SetActive(false);

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
}
