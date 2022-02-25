/*
* Author: Kennan
* Date: Mar.16.2021
* Summary: Makes inventory items draggable within the inventory.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	#region Variables
	public static GameObject itemBeingDragged;
	public static Transform startParent;
	Vector3 startPosition;
	#endregion

	public void OnBeginDrag(PointerEventData eventData)
	{//IBeginDragHandler implementation
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;

		//Hide tooltip within inventory (drag stop is handled in InvSlot)
		InvTooltip.instance.GetComponent<InvTooltip>().StartDrag();
	}//OnBeginDrag(PointerEventData)

	public void OnDrag(PointerEventData eventData)
	{//IDragHandler implementation
		transform.position = eventData.position;
	}//OnDrag(PointerEventData)

	public void OnEndDrag(PointerEventData eventData)
	{//IEndDragHandler implementation
		//print("On End Drag Call");
		itemBeingDragged = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		if(transform.parent == startParent){
			transform.position = startPosition;
		}
	}//OnEndDrag(PointerEventData)

}//DragHandler
