using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : MonoBehaviour
{
	protected AIState substate;

	public virtual AIState Update()
	{
		//if(substate != null) { substate.Update(); }
		if(transform.childCount > 0) { return this; }
		return this;
	}
}