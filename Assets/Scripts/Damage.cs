using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum DamageType
{
	NONE, EXPLODE, CUT, SHARP,
	BLUNT, BULLET, FIRE, COLD,
	ACID, LIGHT, ELECTRO, RADIATION,
	PLASMA
}

public struct Defence
{
	float DEFAULT;
	float EXPLODE, CUT, SHARP;
	float BLUNT, BULLET, FIRE, COLD;
	float ACID, LIGHT, ELECTRO, RADIATION;
	float PLASMA;

	public float ReduceDamage(float damage, DamageType damageType)
	{
		switch(damageType)
		{
			default:                     damage *= 1.0f - DEFAULT;     break;
			case DamageType.EXPLODE:     damage *= 1.0f - EXPLODE;     break;
			case DamageType.CUT:         damage *= 1.0f - CUT;         break;
			case DamageType.SHARP:       damage *= 1.0f - SHARP;       break;
			case DamageType.BLUNT:       damage *= 1.0f - BLUNT;       break;
			case DamageType.BULLET:      damage *= 1.0f - BULLET;      break;
			case DamageType.FIRE:        damage *= 1.0f - FIRE;        break;
			case DamageType.COLD:        damage *= 1.0f - COLD;        break;
			case DamageType.ACID:        damage *= 1.0f - ACID;        break;
			case DamageType.LIGHT:       damage *= 1.0f - LIGHT;       break;
			case DamageType.ELECTRO:     damage *= 1.0f - ELECTRO;     break;
			case DamageType.RADIATION:   damage *= 1.0f - RADIATION;   break;
			case DamageType.PLASMA:      damage *= 1.0f - PLASMA;      break;
		}

		if(damage < 0) { damage = 0; }

		return damage;
	}
}