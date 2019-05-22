using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMiner: Unit
{
	ToolDrill drill;
	ToolReactor reactor;

	public override void Start()
	{
		base.Start();
	}

	//public override void DamageBack(float damage, DamageType damageType)
	//{
	//	if(tool3 != null && tool3.hp > 0){tool3.ReceiveDamage(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageFront(float damage, DamageType damageType)
	//{
	//	if(tool3 != null && tool3.hp > 0){tool3.ReceiveDamage(damage,damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageLeft(float damage, DamageType damageType)
	//{
	//	if(storageState > 0){DamageStorage(damage,damageType);} else
	//	if(tool2 != null && tool2.hp > 0){tool2.ReceiveDamage(damage, damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}

	//public override void DamageRight(float damage, DamageType damageType)
	//{
	//	if(tool1 != null && tool1.hp > 0){tool1.ReceiveDamage(damage, damageType);} else
	//	if(bodyState > 0){DamageBody(damage,damageType);}
	//}
}