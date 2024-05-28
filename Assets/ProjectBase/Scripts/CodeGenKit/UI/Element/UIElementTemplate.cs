namespace ProjectBase.CodeGenKit
{
	public class UIElementTemplate : ITemplate
	{
		public string ClassTemplate =>
@"using UnityEngine;
using System;
namespace #NAMESPACE#
{
	public partial class #CLASSNAME# : BaseController
	{
		
	}
}
";

		public string DesignerTemplate =>
@"namespace #NAMESPACE#
{
	using UnityEngine;

	public partial class #CLASSNAME#
	{
	#MEMBER#
	}
}
";

		public string NameSpace => "ProjectBase.UI";

		public string PrefabPath => "UIPanelPrefab";

		public string CodePath => "Scripts/UIPanel";
	}
}