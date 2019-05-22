using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingTool : Tool
{
	public enum State { READY, DELAY, CANCEL, REPEAT, RELOAD }

	public float initialDelay = 0.25f; //seconds
	public float repeatDelay = 0.25f; //seconds
	public float reloadDuration = 0.5f; //seconds

	private State currentState;
	private Support.Timer stateTimer;

	private void Start()
	{
		Debug.Assert(repeatDelay > 0, "Repeat delay for tool " + name +
		" must be more than zero seconds, tool cannot repeat action infinite number of times at moment");
	}

	void Update()
    {
		ConstantUpdate();

		switch (currentState)
		{
			case State.READY:
			{
				PassiveUpdate();
				ReadyUpdate();
				break;
			}

			case State.DELAY:
			{
				ActiveUpdate();
				DelayUpdate();

				stateTimer.Update();

				if (stateTimer.Expired)
				{
					Action();
					
					stateTimer.Start(repeatDelay);
					currentState = State.REPEAT;
				}

				break;
			}

			case State.CANCEL:
			{
				ActiveUpdate();
				DelayUpdate();

				stateTimer.Update();

				if (stateTimer.Expired)
				{
					Action();
					SwitchToReload();
				}

				break;
			}

			case State.REPEAT:
			{
				ActiveUpdate();
				RepeatUpdate();

				stateTimer.Update();

				if (stateTimer.Expired)
				{
					Action();
					stateTimer.Start(repeatDelay);
				}

				if (!Condition()) { SwitchToReload(); }

				break;
			}

			case State.RELOAD:
			{
				PassiveUpdate();
				ReloadUpdate();

				stateTimer.Update();

				if (stateTimer.Expired) { currentState = State.READY; }

				break;
			}
		}
    }

	protected virtual void ConstantUpdate() { }
	protected virtual void ReadyUpdate() { }
	protected virtual void ActiveUpdate() { } //both at delay and repeat states
	protected virtual void PassiveUpdate() { } //both at ready and reload states
	protected virtual void DelayUpdate() { }
	protected virtual void RepeatUpdate() { }
	protected virtual void ReloadUpdate() { }
	protected virtual void Action() { }
	protected virtual void OnPrepare() { }
	protected virtual void OnReload() { }
	protected virtual bool Condition() { return true; }

	public override void Push()
	{
		if (!Condition()) { return; }

		if (currentState == State.READY)
		{
			OnPrepare();

			if (initialDelay > 0)
			{
				currentState = State.DELAY;
				stateTimer.Start(initialDelay);
			}
			else
			{
				Action();

				currentState = State.REPEAT;
				stateTimer.Start(repeatDelay);
			}
		}
	}

	public override void Release()
	{
		switch (currentState)
		{
			case State.DELAY:
			{
				currentState = State.CANCEL;
				break;
			}

			case State.REPEAT:
			{
				SwitchToReload();
				break;
			}
		}
	}

	private void SwitchToReload()
	{
		OnReload();

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
