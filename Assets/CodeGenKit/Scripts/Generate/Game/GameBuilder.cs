using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
namespace ProjectBase.CodeGenKit
{
	public class GameBuilder : BaseBuilder
	{
		public GameBuilder(GameTemplate template):base(template)
		{

		}
	}
}
#endif