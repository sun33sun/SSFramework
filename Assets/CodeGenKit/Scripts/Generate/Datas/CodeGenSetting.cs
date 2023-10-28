using UnityEngine;

namespace ProjectBase.CodeGenKit
{
	/// <summary>
	/// 文件后缀
	/// </summary>
	public static class CodeSuffix
	{
		public static string classSuffix = ".cs";
		public static string designerSuffix = ".Designer.cs";
		public static string prefabSuffix = ".prefab";
	}


	/// <summary>
	/// 代码模板中的标记
	/// </summary>
	public static class CodeTemplateMarker
	{
		public static string CLASSNAME = "#CLASSNAME#";
		public static string NAMESPACE = "#NAMESPACE#";
		public static string MEMBER = "#MEMBER#";

		public static string ENUM = "#ENUM#";
	}


	/// <summary>
	/// 代码模板
	/// </summary>
	public class CodeGenSetting
	{
		public static string ControllerEnum =>
@"namespace #命名空间#
{
	public enum ControllerType
	{
	#ENUM#
	}
}
";

		public static string ControllerTypeJsonPath => $"{Application.dataPath}/CodeGenKit/Resources/{ControllerTypeJson}.json";

		public static string ControllerTypeJson => "ControllerTypeJson";

		public static string PrefabJsonPath => $"{Application.dataPath}/CodeGenKit/Resources/{PrefabJsonName}.json";

		public static string PrefabJsonName => "PrefabPathJson";
	}
}
