using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Weather
{
	Calm, Storm, ThunderStorm,
	WaterRain, AcidRain, MeteorRain, IceRain,
	Snow
}


public static class Environment
{
	public static float irradiation;
	public static float temperature;
	public static float gravity;
	public static float density;
	public static float pressure;
	public static float humidity;
	public static Vector3 wind;
}
