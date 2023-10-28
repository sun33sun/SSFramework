namespace ProjectBase.CodeGenKit
{
	public class GameTemplate : Template
	{
		public string ClassTemplate =>
@"
using UnityEngine;
using System;
namespace #NAMESPACE#
{
	public partial class #CLASSNAME# : GameController
	{
		
	}
}
";

		public string DesignerTemplate =>
@"namespace #NAMESPACE#
{
	public partial class #CLASSNAME# : GameController
	{
	#MEMBER#
	}
}
";

		public string NameSpace => "ProjectBase.Game";

		public string PrefabPath => "GamePrefab";

		public string CodePath => "Scripts/Game";
	}
}