using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Vector2 delta { get; private set; }
	public Vector2 axis { get; private set; }

	private Vector2 pointerStartPosition;
	private Vector2 currentPointerPosition;
	private Vector2 lastPointerPosition;

	void Update()
	{
		delta = currentPointerPosition - lastPointerPosition;
		lastPointerPosition = currentPointerPosition;
	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		pointerStartPosition = eventData.position;
		currentPointerPosition = eventData.position;
		lastPointerPosition = eventData.position;
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		axis = eventData.position - pointerStartPosition;
		currentPointerPosition = eventData.position;
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		axis = Vector2.zero;
		delta = Vector2.zero;
	}
}
