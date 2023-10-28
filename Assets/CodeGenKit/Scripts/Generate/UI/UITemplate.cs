namespace ProjectBase.CodeGenKit
{
	public class UITemplate : Template
	{
		public string ClassTemplate =>
@"
using UnityEngine;
using System;
namespace #NAMESPACE#
{
	public partial class #CLASSNAME# : UIController
	{
		
	}
}
";

		public string DesignerTemplate =>
@"namespace #NAMESPACE#
{
	public partial class #CLASSNAME# : UIController
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