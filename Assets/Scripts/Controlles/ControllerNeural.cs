using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerNeural : Controller
{
	public enum InputNeurons : int
	{
		OWN_RESOURCE1,
		OWN_RESOURCE2,
		OWN_RESOURCE3,
		OWN_RESOURCE4,
		OWN_SPEED,

		OWN_TOOL1_RELOAD,
		OWN_TOOL1_IS_ACTIVE,

		OWN_TOOL2_RELOAD,
		OWN_TOOL2_IS_ACTIVE,

		OWN_TOOL3_RELOAD,
		OWN_TOOL3_IS_ACTIVE,

		OWN_BOTTOM_SENSOR,
		OWN_TOP_SENSOR,
		OWN_LEFT_SENSOR,
		OWN_RIGHT_SENSOR,
		OWN_FRONT_SENSOR,
		OWN_BACK_SENSOR,

		DISTANCE_TO_TARGET,
		ANGLE_TO_TARGET, //angle between own view direction and distance vector
		ANGLE_FROM_TARGET, //angle between target view direction and distance vector
		COLLISION_CHANCE, //prediction of collision with target
		DAMAGE_RANGE, //some missiles have explosions
		OWN_CORE_DAMAGE,
		OWN_BODY_DAMAGE,
		OWN_HEAD_DAMAGE,
		OWN_LEGS_DAMAGE,
		OWN_TOOL1_DAMAGE,
		OWN_TOOL2_DAMAGE,
		OWN_TOOL3_DAMAGE,
		TARGET_CORE_DAMAGE,
		TARGET_BODY_DAMAGE,
		TARGET_LEGS_DAMAGE,
		TARGET_TOOL1_DAMAGE,
		TARGET_TOOL2_DAMAGE,
		TARGET_TOOL3_DAMAGE,

		TARGET_RESOURCE1,
		TARGET_RESOURCE2,
		TARGET_RESOURCE3,
		TARGET_RESOURCE4,
		TARGET_SPEED,
		TARGET_RADIUS, //needed to estimate possibility of avoiding big or small missile

		TARGET_TOOL1_RELOAD,
		TARGET_TOOL1_IS_ACTIVE,

		TARGET_TOOL2_RELOAD,
		TARGET_TOOL2_IS_ACTIVE,

		TARGET_TOOL3_RELOAD,
		TARGET_TOOL3_IS_ACTIVE,

		TARGET_BOTTOM_SENSOR,
		TARGET_TOP_SENSOR,
		TARGET_LEFT_SENSOR,
		TARGET_RIGHT_SENSOR,
		TARGET_FRONT_SENSOR,
		TARGET_BACK_SENSOR,

		TARGET_IS_ENEMY,
		TARGET_IS_FRIEND,
		TARGET_IS_NEUTRAL,
		TARGET_IS_OWN_RESOURCE1_ITEM,
		TARGET_IS_OWN_RESOURCE2_ITEM,
		TARGET_IS_OWN_RESOURCE3_ITEM,
		TARGET_IS_OWN_RESOURCE4_ITEM,
		//TARGET_IS_ENEMY_RESOURCE1_ITEM, //how to determine which enemy?
		//TARGET_IS_ENEMY_RESOURCE2_ITEM, //how to determine which enemy?
		//TARGET_IS_ENEMY_RESOURCE3_ITEM, //how to determine which enemy?
		//TARGET_IS_ENEMY_RESOURCE4_ITEM, //how to determine which enemy?
		TARGET_IS_EXPLOSIVE_ITEM,
		TARGET_IS_THROWABLE_ITEM, //value may vary from 0.0 to 1.0 for different items (i.e. partially good for throwing)

		CURRENT_TARGET_DURATION, //how long the same target was chosen; may force to switch targets through time

		Count
	}

	public enum OutputNeurons : int
	{
		TOOL1_HOLD,
		TOOL1_PRESS,
		TOOL1_RELEASE,
		TOOL1_SINGLE_CLICK,
		TOOL1_DOUBLE_CLICK,

		TOOL2_HOLD,
		TOOL2_PRESS,
		TOOL2_RELEASE,
		TOOL2_SINGLE_CLICK,
		TOOL2_DOUBLE_CLICK,

		TOOL3_HOLD,
		TOOL3_PRESS,
		TOOL3_RELEASE,
		TOOL3_SINGLE_CLICK,
		TOOL3_DOUBLE_CLICK,

		JUMP_HOLD,
		JUMP_SINGLE_CLICK,
		JUMP_DOUBLE_CLICK,

		MOVE_TO_TARGET,
		MOVE_FROM_TARGET,
		TURN_FRONT_TO_TARGET,
		TURN_BACK_TO_TARGET,
		TURN_LEFT_SIDE_TO_TARGET,
		TURN_RIGHT_SIDE_TO_TARGET,

		Count
	}

	//not a single neural network but a set of 20 networks for each robot
	private NeuralNetwork targetNetwork; //used in constant update, no need to override some inputs
	private NeuralNetwork signalNetwork; //usen in reaction to target

	private void Start()
	{
		if (true == false) //WIP
		{
			int inputLayer = (int)InputNeurons.Count;
			int hiddenLayer1 = inputLayer + (inputLayer / 2) + (inputLayer / 3);
			int hiddenLayer2 = hiddenLayer1;
			int outputLayer = (int)OutputNeurons.Count;

			int[] topology = { inputLayer, hiddenLayer1, hiddenLayer2, outputLayer };

			targetNetwork = new NeuralNetwork(topology);
			signalNetwork = new NeuralNetwork(topology);
		}
	}

	void ProcessResult(List<int> results)
	{
		int best = 0;
		for (int i = 1; i < results.Count; i++)
		{
			if (results[i] > results[best]) { best = i; }
		}

		switch ((OutputNeurons)best)
		{
			case OutputNeurons.TOOL1_HOLD:
			case OutputNeurons.TOOL1_PRESS:
			case OutputNeurons.TOOL1_RELEASE:
			case OutputNeurons.TOOL1_SINGLE_CLICK:
			case OutputNeurons.TOOL1_DOUBLE_CLICK:

			case OutputNeurons.TOOL2_HOLD:
			case OutputNeurons.TOOL2_PRESS:
			case OutputNeurons.TOOL2_RELEASE:
			case OutputNeurons.TOOL2_SINGLE_CLICK:
			case OutputNeurons.TOOL2_DOUBLE_CLICK:

			case OutputNeurons.TOOL3_HOLD:
			case OutputNeurons.TOOL3_PRESS:
			case OutputNeurons.TOOL3_RELEASE:
			case OutputNeurons.TOOL3_SINGLE_CLICK:
			case OutputNeurons.TOOL3_DOUBLE_CLICK:

			case OutputNeurons.JUMP_HOLD:
			case OutputNeurons.JUMP_SINGLE_CLICK:
			case OutputNeurons.JUMP_DOUBLE_CLICK:

			case OutputNeurons.MOVE_TO_TARGET:
			case OutputNeurons.MOVE_FROM_TARGET:
			case OutputNeurons.TURN_FRONT_TO_TARGET:
			case OutputNeurons.TURN_BACK_TO_TARGET:
			case OutputNeurons.TURN_LEFT_SIDE_TO_TARGET:
			case OutputNeurons.TURN_RIGHT_SIDE_TO_TARGET:
				break;
		}
	}

	void ToolAction()
	{
		//if tool uses missiles - track until missile reached target or dissappears
		//track all damage missile delivers to all targets (including its owner if explosion missile also spawned)

		//if tool just manages state or resources than it is a long-term strategy estimation
	}

	void JumpAction()
	{
		//if cannot jump - wrong action

		//if jumped - track until reached ground again
		//jumped status means all 6 sensors says no contact
	}

	void PickTargetItemAction()
	{
		//if item is unusable - wrong action
	}
}
