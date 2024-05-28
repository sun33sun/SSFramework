using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBase
{
	public abstract class UIController : BaseController
	{
		[HideInInspector] public Canvas canvas = null;
		[HideInInspector] public GraphicRaycaster raycaster = null;
		[HideInInspector] public CanvasGroup group = null;
	}
}