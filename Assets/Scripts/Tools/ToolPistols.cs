using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPistols: Tool
{
	//ALL TODO, COMPLEX CASE

	public enum State { READY, PREPARE, REPEAT, FINISH, RELAX, RELOAD }

	public float prepareDuration;
	public float reloadDuration;

	public Missile bulletMissilePrefab;
	public Transform rightMissileSpawner, leftMissileSpawner;
	public ResourceContainer bulletsResource;
	public ResourceContainer energyResource;

	public int bulletsPerClip = 30;

	public string leftAimAnimation;
	public string rightAimAnimation;

	private State currentState;
	private Support.Timer stateTimer;
	private bool rightTurn = true;
	private int wastedBullets;

	private void Update()
	{
		switch (currentState)
		{
			case State.READY: break;
			case State.PREPARE: break;
			case State.REPEAT: break;
			case State.FINISH: break;
			case State.RELAX: break;
			case State.RELOAD:
			{
				stateTimer.Update();

				if (stateTimer.Expired)
				{
					bulletsResource.Spend(wastedBullets);
					currentState = State.READY;
				}

				break;
			}
		}
	}

	public override void Singleclick()
	{
		switch (currentState)
		{
			case State.READY:
			{
				currentState = State.PREPARE;
				stateTimer.Start(prepareDuration);
				//owner.Play();

				break;
			}
		}
	}

	public override void Doubleclick()
	{
		if (currentState != State.READY) { return; }

		wastedBullets = (int)bulletsResource.amount % bulletsPerClip;

		if (wastedBullets == 0) { return; }

		SwitchToReload();
	}

	public override void Hold()
	{
		switch (currentState)
		{ 
			
		}
	}

	private void Shoot()
	{
		Missile newMissile;

		if (rightTurn)
		{
			newMissile = Instantiate(bulletMissilePrefab, rightMissileSpawner.position, rightMissileSpawner.rotation);
		}
		else
		{
			newMissile = Instantiate(bulletMissilePrefab, leftMissileSpawner.position, leftMissileSpawner.rotation);
		}

		newMissile.SetOwner(owner);

		rightTurn = !rightTurn;
	}

	private void SwitchToReload()
	{
		if (reloadDuration > 0)
		{
			currentState = State.RELOAD;
			stateTimer.Start(reloadDuration);
		}
		else
		{
			bulletsResource.Spend(wastedBullets);
		}
	}
}