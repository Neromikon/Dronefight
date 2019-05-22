using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCopter: Unit
{
	ToolSprayer sprayer;
	ToolAirgun airgun;
	ToolDrone drone;

	public override void Start()
	{
		base.Start();

		sprayer = Support.GetComponentRecursive<ToolSprayer>(transform, "Sprayer");
		airgun = Support.GetComponentRecursive<ToolAirgun>(transform, "Airgun");
		drone = Support.GetComponentRecursive<ToolDrone>(transform, "Drone");

		tool1 = sprayer;
		tool2 = airgun;
		tool3 = drone;

		Debug.Assert(sprayer, name + " does not have sprayer tool");
		Debug.Assert(airgun, name + " does not have airgun tool");
		Debug.Assert(drone, name + " does not have minidrone tool");
	}


	public override void Update()
	{
		base.Update();
	}

	//public override void DamageBack(float damage,DamageType damageType)
	//{
	//	if(storageState > 0){DamageStorage(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageFront(float damage,DamageType damageType)
	//{
	//	if(storageState > 0){DamageStorage(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageLeft(float damage,DamageType damageType)
	//{
	//	if(tool2 != null && tool2.hp > 0){tool2.ReceiveDamage(damage,damageType);} else
	//	if(storageState > 0){DamageStorage(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageRight(float damage,DamageType damageType)
	//{
	//	if(tool1 != null && tool1.hp > 0){tool1.ReceiveDamage(damage,damageType);} else
	//	if(storageState > 0){DamageStorage(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}
}