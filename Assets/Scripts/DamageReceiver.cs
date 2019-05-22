using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver : MonoBehaviour
{
	public Unit owner;

	public ResourceContainer hitpoints;

	public float explosionDefence;
	public float cutDefence;
	public float pierceDefence;
	public float bluntDefence;
	public float bulletDefence;
	public float fireDefence;
	public float coldDefence;
	public float acidDefence;
	public float lightDefence;
	public float electroDefence;
	public float radiationDefence;
	public float plasmaDefence;

	public UnityEvent onDeath;

	public void ReceiveDamage(float damage, DamageType type)
	{
		if (hitpoints.IsEmpty()) { return; }

		switch (type)
		{
			//default: damage *= 1.0f - DEFAULT; break;
			case DamageType.EXPLODE: damage *= 1.0f - explosionDefence; break;
			case DamageType.CUT: damage *= 1.0f - cutDefence; break;
			case DamageType.SHARP: damage *= 1.0f - pierceDefence; break;
			case DamageType.BLUNT: damage *= 1.0f - bluntDefence; break;
			case DamageType.BULLET: damage *= 1.0f - bulletDefence; break;
			case DamageType.FIRE: damage *= 1.0f - fireDefence; break;
			case DamageType.COLD: damage *= 1.0f - coldDefence; break;
			case DamageType.ACID: damage *= 1.0f - acidDefence; break;
			case DamageType.LIGHT: damage *= 1.0f - lightDefence; break;
			case DamageType.ELECTRO: damage *= 1.0f - electroDefence; break;
			case DamageType.RADIATION: damage *= 1.0f - radiationDefence; break;
			case DamageType.PLASMA: damage *= 1.0f - plasmaDefence; break;
		}

		if (damage <= 0) { return; }

		hitpoints.Spend(damage);

		if (hitpoints.IsEmpty())
		{
			Debug.Log(name + " destroyed");
			onDeath.Invoke();
			gameObject.SetActive(false);
		}
	}

	public bool IsDead() { return !hitpoints.IsEmpty(); }
}
