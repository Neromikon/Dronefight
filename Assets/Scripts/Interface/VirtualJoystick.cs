using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public RectTransform areaRectTransform;
	public RectTransform buttonRectTransform;
	public CircleCollider2D circle;
	public float circleScaling = 1.0f;
	public Vector2 axis { get; private set; }

	private Vector2 pointerStartPosition;
	private Vector2 startingPosition;

	private void Update()
	{
		if (areaRectTransform.hasChanged)
		{
			areaRectTransform.hasChanged = false;
			float widthDiff = areaRectTransform.rect.width - buttonRectTransform.rect.width;
			float heightDiff = areaRectTransform.rect.height - buttonRectTransform.rect.height;
			circle.radius = 0.5f * circleScaling * Mathf.Max(widthDiff, heightDiff);
		}
	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		startingPosition = transform.position;
		pointerStartPosition = eventData.position;
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		Vector2 delta = eventData.position - pointerStartPosition;

		transform.position = startingPosition + delta;

		if (!circle.OverlapPoint(transform.position))
		{
			transform.position = circle.ClosestPoint(transform.position);
		}

		axis = delta / circle.radius;
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		transform.position = startingPosition;
		axis = Vector2.zero;
	}
}
