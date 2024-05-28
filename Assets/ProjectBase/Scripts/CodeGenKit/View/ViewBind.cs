#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectBase.CodeGenKit
{
	public class ViewBind : BaseBind
	{
		
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(ViewBind))]
	public class ViewBindEditor : BaseBindEditor
	{
		
	}
#endif
}