using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBlades: Tool
{
	//ALL TODO

	public enum State { READY, PREPARE_LEFT, PREPARE_RIGHT, PREPARE_DOUBLE, PREPARE_WHIRL, APPLY, REPEAT, RELOAD }

	public MissileMelee blade1Missile;
	public MissileMelee blade2Missile;
	public ResourceContainer energyResource;
	
	public float energyCost = 0.75f;
	public string leftAttackAnimation;
	public string rightAttackAnimation;
	public string doubleAttackAnimation;
	public string whirlAttackAnimation;

	private State currentState;
	private Support.Timer stateTimer;

	private bool isLeftHandTurn = true;

	public override void Hold()
	{
		if (currentState != State.READY) { return; }

		//currentState = 
	}

	public override void Singleclick()
	{
		if (currentState != State.READY) { return; }
	}

	public override void Doubleclick()
	{
		if (currentState != State.READY) { return; }
	}
}