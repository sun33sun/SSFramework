namespace ProjectBase.CodeGenKit
{
	public class ViewTemplate : ITemplate
	{
		public string ClassTemplate =>
@"using UnityEngine;
using System;
namespace #NAMESPACE#
{
	public partial class #CLASSNAME# : ViewController
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

		public string NameSpace => "ProjectBase.Game";

		public string PrefabPath => "GameController";

		public string CodePath => "Scripts/GameController";
	}
}