#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectBase.CodeGenKit
{
	public class GameBind : BaseBind
	{
		
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(GameBind))]
	public class GameBindEditor : BaseBindEditor
	{
		
	}
#endif
}