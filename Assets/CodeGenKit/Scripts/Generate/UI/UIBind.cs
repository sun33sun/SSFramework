using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBase.CodeGenKit
{
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasGroup))]
	[RequireComponent(typeof(GraphicRaycaster))]
	public class UIBind : GameBind
	{
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(UIBind))]
	public class UIBindEditor : BaseBindEditor
	{
	}
#endif
}