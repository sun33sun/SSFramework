using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectBase.UI
{
	public class UIRoot : MonoBehaviour
	{
		private static UIRoot instance;
		public static UIRoot Instance=>instance;

		public Canvas canvas;
		public CanvasScaler canvasScaler;
		public Camera uiCamera;
		public RectTransform panelRoot;
		public EventSystem eventSystem;

		private void Awake()
		{
			instance = this;
		}
	}
}

