using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelector : MonoBehaviour
{
	public PrivateButton target { get; private set; }
	public RectTransform rectTransform;
	public Vector2 indentation;

	private Rect ownRect;
	private List<PrivateButton> targets = new List<PrivateButton>();

	void Start()
	{
		DontDestroyOnLoad(this);

		ownRect = rectTransform.rect;
	}

	public Transform area { get; private set; }

	public void SetArea(Transform newArea)
	{
		area = newArea;
		target = null;

		targets.Clear();
		FindTargetsRecursive(newArea);
	}

	private void FindTargetsRecursive(Transform searchTransform)
	{
		PrivateButton privateButton = searchTransform.GetComponent<PrivateButton>();

		if (privateButton) { targets.Add(privateButton); }

		foreach (Transform child in searchTransform)
		{
			FindTargetsRecursive(child);
		}
	}

	public void Move4D(Vector2 direction)
	{
		Move(direction, 0.38268343236f); //cos(3pi/8)
	}

	public void Move8D(Vector2 direction)
	{
		Move(direction, 0.8314696123f); //cos(3pi/16)
	}

	public void Move(Vector2 direction, float minCosinus)
	{
		if (!area) { return; }
		if (targets.Count == 0) { return; }
		if (!target) { SetAnyTarget(); return; }

		float closestDistance = 0.0f;
		PrivateButton closestTarget = null;

		foreach (PrivateButton potentialTarget in targets)
		{
			if (!potentialTarget.isActiveAndEnabled) { continue; }
			if (potentialTarget == target) { continue; }

			Vector2 distance = potentialTarget.transform.position - target.transform.position;
			float distanceSquare = distance.sqrMagnitude;

			float cosinus = Vector2.Dot(distance, direction) / (distance.magnitude * direction.magnitude);

			if (cosinus < minCosinus) { continue; }

			if (distanceSquare > closestDistance && closestTarget != null) { continue; }

			closestTarget = potentialTarget;
			closestDistance = distanceSquare;
		}

		if (closestTarget)
		{
			SetTarget(closestTarget);
		}
		else
		{
			Debug.Log("Found no target");
		}
	}

	private void SetAnyTarget()
	{
		foreach (PrivateButton possibleTarget in targets)
		{
			if (possibleTarget.isActiveAndEnabled)
			{
				SetTarget(possibleTarget);
				return;
			}
		}

		SetTarget(null);
	}

	public void SetTarget(PrivateButton newTarget)
	{
		if (newTarget && newTarget.transform.IsChildOf(area))
		{
			target = newTarget;

			gameObject.SetActive(true);

			transform.SetParent(newTarget.transform, false);

			RectTransform targetRectTransform = target.GetComponent<RectTransform>();

			if (rectTransform)
			{
				//transform.localPosition = Vector2.zero;
				rectTransform.anchoredPosition = Vector2.zero;
				rectTransform.localScale = Vector2.one;

				//ownRect.size = targetRectTransform.rect.size + indentation;
				rectTransform.sizeDelta = targetRectTransform.sizeDelta + indentation;
			}
			else
			{
				target = null;
				gameObject.SetActive(false);
				transform.SetParent(null);
			}
		}
		else
		{
			target = null;
			gameObject.SetActive(false);
			transform.SetParent(null);
		}
	}
}
