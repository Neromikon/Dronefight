using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public static class Support
{
	public enum Direction : int
	{
		NONE =       (0 + 0), //0
		RIGHT =      (0 + 1), //1
		LEFT =       (0 + 2), //2
		UP =         (3 + 0), //3
		UP_RIGHT =   (3 + 1), //4
		UP_LEFT =    (3 + 2), //5
		DOWN =       (6 + 0), //6
		DOWN_RIGHT = (6 + 1), //7
		DOWN_LEFT =  (6 + 2)  //8
	}

	public struct Grid
	{
		public Vector2 position;
		public Vector2 size;
		public Vector2 elementSize;

		private Vector2Int dimension;

		public Grid(Vector2 position, Vector2 size, Vector2 elementSize)
		{
			this.position = position;
			this.size = size;
			this.elementSize = elementSize;

			dimension = new Vector2Int((int)(size.x / elementSize.x), (int)(size.y / elementSize.y));
		}

		public Vector2 At(int index)
		{
			int y = index / dimension.x;
			int x = index % dimension.x;
			return position + new Vector2(x * elementSize.x, y * elementSize.y);
		}
	}

	public struct Timer
	{
		public float timeLeft { get; private set; }

		public void Update()
		{
			if (timeLeft <= 0) { return; }

			timeLeft -= Time.deltaTime;

			if (timeLeft < 0) { timeLeft = 0; }
		}

		public void Start(float time)
		{
			timeLeft = time;
		}

		public void Add(float time)
		{
			timeLeft += time;
		}

		public bool Expired { get { return timeLeft <= 0; } }
	}

	public static Transform FindRecursive(Transform current, string name)
	{
		foreach(Transform child in current)
		{
			if (name == child.name) { return child; }
		}

		foreach (Transform child in current)
		{
			Transform find = FindRecursive(child, name);
			if (find != null) { return find; }
		}

		return null;
	}


	public static T GetComponentRecursive<T>(Transform current, string name)
	{
		Transform found = FindRecursive(current, name);

		if (found)
		{
			return found.GetComponent<T>();
		}

		return default(T);
	}


	public static T GetComponentRecursive<T>(Transform current) where T : UnityEngine.Object
	{
		T result = current.GetComponent<T>();

		if (result != null) { return result; }

		foreach (Transform child in current)
		{
			T found = GetComponentRecursive<T>(child);
			if (found != null) { return found; }
		}

		return default(T);
	}


	public static Unit GetOwner(Transform current)
	{
		for(Transform find = current; find != null; find = find.parent)
		{
			Unit unit = find.GetComponent<Unit>();
			if(unit != null) { return unit; }
		}
		return null;
	}


	public static void RemoveParticles(Transform parent)
	{
		Repeat:
		foreach(Transform child in parent)
		{
			ParticleSystem particles = child.GetComponent<ParticleSystem>();
			if(!particles) { continue; }

			//Debug.Log(child.name + " is now child of null");
			child.SetParent(null);

			particles.Stop();
			ParticleSystem.EmissionModule emissionModule = particles.emission;
			emissionModule.enabled = false;

			GameObject.Destroy(child.gameObject, particles.main.startLifetime.constantMax);

			//Note: can't change transform hierarchy while iterating it, goto seems to be best approach here
			goto Repeat;
		}
	}

	public static float GetJoystickAxis(int joystickIndex, int axisIndex)
	{
		return Input.GetAxis("Joystick" + joystickIndex.ToString() + "Axis" + axisIndex.ToString());
	}

	public static Direction DefineDirectoin(Vector2 v)
	{
		int x = (v.x == 0) ? 0 : ((v.x > 0) ? 1 : 2);
		int y = (v.y == 0) ? 0 : ((v.y > 0) ? 3 : 6);
		return (Direction)(x + y);
	}

	public static Vector2 GetMovementDirection(bool up, bool down, bool left, bool right)
	{
		Vector2 result = Vector2.zero;

		if (left) { result += Vector2.left; }
		if (right) { result += Vector2.right; }
		if (up) { result += Vector2.up; }
		if (down) { result += Vector2.down; }

		return result;
	}

	public static Vector2 CirclePoint(float unitAngle, Vector2 scale)
	{
		float radianAngle = unitAngle * Mathf.PI * 2.0f;
		return Vector2.Scale(new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)), scale * 0.5f);
	}

	public static void ShuffleChildrenOrder(Transform parent)
	{
		int[] siblingIndices = new int[parent.childCount];

		for (int i = 0; i < siblingIndices.Length; i++)
		{
			siblingIndices[i] = i;
		}

		siblingIndices = siblingIndices.OrderBy(x => UnityEngine.Random.Range(0, 1000)).ToArray();

		{
			int i = 0;
			foreach (Transform child in parent)
			{
				child.SetSiblingIndex(siblingIndices[i]);
				i++;
			}
		}
	}
}