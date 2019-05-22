using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTank: Unit
{
	ToolMinigun minigun;
	ToolFlaregun flaregun;
	ToolFlamer flamer;

	public override void Start()
	{
		base.Start();

		minigun = Support.GetComponentRecursive<ToolMinigun>(transform, "Minigun");
		flaregun = Support.GetComponentRecursive<ToolFlaregun>(transform, "Flaregun");
		flamer = Support.GetComponentRecursive<ToolFlamer>(transform, "Flamer");

		tool1 = minigun;
		tool2 = flaregun;
		tool3 = flamer;

		Debug.Assert(minigun, name + " does not have minigun tool");
		Debug.Assert(flaregun, name + " does not have flaregun tool");
		Debug.Assert(flamer, name + " does not have flamer tool");
	}

	//public override void DamageBack(float damage, DamageType damageType)
	//{
	//	if(storageState > 0){DamageStorage(damage,damageType);}
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
	//	if(tool3 != null && tool3.hp > 0){tool3.ReceiveDamage(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageRight(float damage, DamageType damageType)
	//{
	//	if(tool1 != null && tool1.hp > 0){tool1.ReceiveDamage(damage, damageType);} else
	//	if(tool3 != null && tool3.hp > 0){tool3.ReceiveDamage(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}
}