using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickingTool : Tool
{
	public enum State { READY, RELOAD }

	public float reloadDuration = 1.0f; //seconds

	private State currentState;
	private Support.Timer stateTimer;

    void Update()
    {
		switch (currentState)
		{
			case State.READY: ReadyUpdate(); break;

			case State.RELOAD:
			{
				ReloadUpdate();

				stateTimer.Update();

				if (stateTimer.Expired)
				{
					currentState = State.READY;
				}

				break;
			}
		}
    }

	protected virtual void ReadyUpdate() { }
	protected virtual void ReloadUpdate() { }
	protected virtual void Action() { }
	protected virtual bool Condition() { return true; }

	public override void Push()
	{
		if (!Condition()) { return; }

		if (currentState == State.READY)
		{
			Action();

			currentState = State.RELOAD;
			stateTimer.Start(reloadDuration);
		}
	}
}
