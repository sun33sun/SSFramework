using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScrollView : ScrollRect, IPointerDownHandler, IPointerUpHandler
{
	public enum E_Player
	{
		None = 0,
		First = 1 << 1,
		Second = 1 << 2,
		Third = 1 << 3,
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		print(1 << 1);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
	}
}
