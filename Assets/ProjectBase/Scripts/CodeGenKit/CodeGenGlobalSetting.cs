using UnityEngine;

namespace ProjectBase.CodeGenKit
{
	/// <summary>
	/// 文件后缀
	/// </summary>
	public static class FileSuffix
	{
		public static string classSuffix = ".cs";
		public static string designerSuffix = ".Designer.cs";
		public static string prefabSuffix = ".prefab";
	}


	/// <summary>
	/// 代码模板中的标记
	/// </summary>
	public class CodeTemplateMarker
	{
		public const string CLASSNAME = "#CLASSNAME#";
		public const string NAMESPACE = "#NAMESPACE#";
		public const string MEMBER = "#MEMBER#";
		public const string ENUM = "#ENUM#";
		public const string DATA = "#DATA#";
		public const string KEY = "#KEY#";
	}


	/// <summary>
	/// 代码模板
	/// </summary>
	public static class CodeGenGlobalSetting
	{
		public static string DataMgrPath = $"{Application.dataPath}/ProjectBase/Scripts/CodeGenKit/Data/DataMgr{FileSuffix.designerSuffix}";
		public static string DataMgrTemplate =
@"using ProjectBase.UI;
using ProjectBase.Game;

namespace ProjectBase
{
	public partial class DataMgr
	{
		void Init()
		{
		#DATA#
		}
	}
}
";
		public static string PrefabJsonName => "PrefabPathJson";

		public static string PrefabJsonPath => $"{Application.dataPath}/ProjectBase/Resources/{PrefabJsonName}.json";
	}
}
