using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingTool : Tool
{
	public enum State { READY, PREPARE, CANCEL, APPLY, RELOAD }

	public bool holdOnPrepare = true;
	public bool holdOnApply = false;

	public float prepareDuration = 0.25f; //seconds
	public float applyDuration = 0.25f; //seconds
	public float reloadDuration = 0.5f; //seconds

	private State currentState;
	private Support.Timer stateTimer;
	
	void Update()
    {
		switch (currentState)
		{
			case State.READY: ReadyUpdate(); break;

			case State.PREPARE:
			{
				PrepareUpdate();

				stateTimer.Update();

				if (!holdOnPrepare)
				{
					if (stateTimer.Expired) { SwitchToApply(); }
				}

				break;
			}

			case State.CANCEL:
			{
				PrepareUpdate();

				stateTimer.Update();

				if (stateTimer.Expired) { SwitchToApply(); }

				break;
			}

			case State.APPLY:
			{
				ApplyUpdate();

				if (!holdOnApply)
				{
					stateTimer.Update();
					if (stateTimer.Expired) { SwitchToReload(); }
				}

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
	protected virtual void PrepareUpdate() { }
	protected virtual void ApplyUpdate() { }
	protected virtual void ReloadUpdate() { }
	protected virtual void Prepare() { }
	protected virtual void Action() { }
	protected virtual void OnReload() { }
	protected virtual bool Condition() { return true; }

	public override void Push()
	{
		if (currentState != State.READY) { return; }
		if (!Condition()) { return; }

		Prepare();

		if (prepareDuration > 0)
		{
			currentState = State.PREPARE;
			stateTimer.Start(prepareDuration);
		}
		else
		{
			SwitchToApply();
		}
		
	}

	public override void Release()
	{
		switch (currentState)
		{
			case State.PREPARE:
			{
				if (holdOnPrepare)
				{
					if (stateTimer.Expired)
					{
						SwitchToApply();
					}
					else
					{
						currentState = State.CANCEL;
					}
				}
				break;
			}

			case State.APPLY:
			{
				if (holdOnApply)
				{
					SwitchToReload();
				}
				break;
			}
		}
	}

	private void SwitchToApply()
	{
		Action();

		if (applyDuration > 0)
		{
			currentState = State.APPLY;
			stateTimer.Start(applyDuration);
		}
		else
		{
			SwitchToReload();
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
