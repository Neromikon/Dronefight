using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class GameLayer
{
	public const int Default = 0;
	public const int Units = 8;
	public const int Missiles = 9;
	public const int Items = 10;
	public const int Collectors = 11;
	public const int Puddles = 12;
	public const int Destructibles = 13;
	public const int Liquid = 14;
	public const int Effects = 20;
	public const int DamageReceivers = 22;
	public const int ObjectSensors = 23;
	public const int GroundSensors = 24;
	public const int LiquidStick = 25;

	public const int DefaultMask = 1 << Default;
	public const int UnitsMask = 1 << Units;
	public const int MissilesMask = 1 << Missiles;
	public const int ItemsMask = 1 << Items;
	public const int CollectorsMask = 1 << Collectors;
	public const int PuddlesMask = 1 << Puddles;
	public const int DestructiblesMask = 1 << Destructibles;
	public const int LiquidsMask = 1 << Liquid;
	public const int EffectsMask = 1 << Effects;
	public const int DamageReceiversMask = 1 << DamageReceivers;
	public const int ObjectSensorsMask = 1 << ObjectSensors;
	public const int GroundSensorsMask = 1 << GroundSensors;
	public const int LiquidStickMask = 1 << LiquidStick;
}