using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHeater: Unit
{
	ToolSolderer solderer;
	ToolDryer dryer;
	ToolFurnace furnace;

	public override void Start()
	{
		base.Start();

		solderer = Support.GetComponentRecursive<ToolSolderer>(transform, "Solderer");
		dryer = Support.GetComponentRecursive<ToolDryer>(transform, "Dryer");
		furnace = Support.GetComponentRecursive<ToolFurnace>(transform, "Furnace");

		tool1 = solderer;
		tool2 = dryer;
		tool3 = furnace;

		Debug.Assert(solderer, name + " does not have solderer tool");
		Debug.Assert(dryer, name + " does not have dryer tool");
		Debug.Assert(furnace, name + " does not have furnace tool");
	}

	public override void Update()
	{
		base.Update();
	}

	//public override void DamageBack(float damage, DamageType damageType)
	//{
	//	if(tool3 != null && tool3.hp > 0){tool3.ReceiveDamage(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageFront(float damage, DamageType damageType)
	//{
	//	if(headState > 0){DamageHead(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageLeft(float damage, DamageType damageType)
	//{
	//	if(tool2 != null && tool2.hp > 0){tool2.ReceiveDamage(damage, damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageRight(float damage, DamageType damageType)
	//{
	//	if(tool1 != null && tool1.hp > 0){tool1.ReceiveDamage(damage, damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}
}