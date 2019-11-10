using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	Vector3 dragOffset = Vector3.zero;

	public void OnEndDrag (PointerEventData eventData)
	{
		dragOffset = Vector3.zero;
	}

	public void OnDrag (PointerEventData eventData)
	{
		//transform.position = Globals.instance.ScreenToWorld(Input.mousePosition + dragOffset);
		transform.position = Input.mousePosition + dragOffset;
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		//dragOffset = Globals.instance.WorldToScreen(transform.position) - Input.mousePosition;
		dragOffset = transform.position - Input.mousePosition;
	}
}
