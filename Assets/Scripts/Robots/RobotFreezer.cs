using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFreezer: Unit
{
	public float constantHeating = 0.01f; //units per second

	ToolWatercannon watercannon;
	ToolCooler cooler;
	ToolCompresser compresser;

	public override void Start()
	{
		base.Start();

		watercannon = Support.GetComponentRecursive<ToolWatercannon>(transform, "Watercannon");
		cooler = Support.GetComponentRecursive<ToolCooler>(transform, "Cooler");
		compresser = Support.GetComponentRecursive<ToolCompresser>(transform, "Compresser");

		tool1 = watercannon;
		tool2 = cooler;
		tool3 = compresser;

		Debug.Assert(watercannon, name + " does not have watercannon tool");
		Debug.Assert(cooler, name + " does not have cooler tool");
		Debug.Assert(compresser, name + " does not have compresser tool");
	}

	public override void Update()
	{
		base.Update();

		//TODO
		//Spend(-constantHeating * Time.deltaTime, ResourceType.HEAT);

		//float r = resource3 * 0.01f;
		//float b = 1.0f - resource3 * 0.01f;
		//resource3Color = new Color(r, b, b);
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