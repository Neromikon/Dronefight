using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Vector2 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		startingPosition = transform.position;
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		transform.position = startingPosition;
	}
}
