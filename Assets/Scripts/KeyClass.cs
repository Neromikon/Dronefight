using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct Key_class
{
	public enum Action { NONE, PRESS, RELEASE, HOLD, OMIT }

	public const float CLICK_TIME = 0.125f; //125 ms

	public bool state { get; private set; }
	public float hold_time { get; private set; }
	public float release_time { get; private set; }
	public int clicks { get; private set; }
	public Action action { get; private set; }

	public static bool operator ==(Key_class key, bool value) { return key.state == value; }
	public static bool operator !=(Key_class key, bool value) { return key.state != value; }

	public override bool Equals(object obj) { return base.Equals(obj); }
	public override int GetHashCode() { return base.GetHashCode(); }

	public void Setup()
	{
		state = false;
		hold_time = 0.0f;
		release_time = 0.0f;
		clicks = 0;
		action = Action.NONE;
	}

	public void Update(bool newStatus)
	{
		bool old_status = state;
		float delta_time = Time.deltaTime;

		state = newStatus;

		if (state == true)
		{
			if (hold_time < 10000.0f) { hold_time += delta_time; }
		}
		else
		{
			if (release_time < 10000.0f) { release_time += delta_time; }
		}

		if (newStatus != old_status)
		{
			if (newStatus == true)
			{
				if (release_time > CLICK_TIME)
				{
					clicks = 0;
				}
				hold_time = 0;
				action = Action.PRESS;
				return;
			}
			else
			{
				if (hold_time <= CLICK_TIME)
				{
					clicks += 1;
				}
				else
				{
					clicks = 0;
				}
				release_time = 0;
				action = Action.RELEASE;
				return;
			}
		}
		else
		{
			if (old_status == false)
			{
				if (release_time >= CLICK_TIME)
					if (release_time - delta_time < CLICK_TIME)
					{
						action = Action.OMIT;
						return;
					}
			}
			else
			{
				if (hold_time >= CLICK_TIME)
					if (hold_time - delta_time < CLICK_TIME)
					{
						action = Action.HOLD;
						return;
					}
			}
		}

		action = 0;
	}
}