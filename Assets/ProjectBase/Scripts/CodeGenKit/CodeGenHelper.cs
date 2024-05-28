using Newtonsoft.Json;
using ProjectBase.CodeGenKit;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ProjectBase
{
	public static class CodeGenHelper
	{
		static Dictionary<string, string> prefabDic = null;
		public static Dictionary<string, string> PrefabDic
		{
			get
			{
				if (prefabDic == null)
				{
					string json = Resources.Load<TextAsset>(CodeGenGlobalSetting.PrefabJsonName).text;
					prefabDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
					if (prefabDic == null )
					{
						prefabDic = new Dictionary<string, string>();
					}
				}

				return prefabDic;
			}
		}

		public static void Write(string filePath, string content)
		{
			if (File.Exists(filePath))
				File.Delete(filePath);
			string path = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(path))
			{
				DirectoryInfo directoryInfo = Directory.CreateDirectory(path);
			}
			FileStream file = new FileStream(filePath, FileMode.CreateNew);
			StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
			fileW.Write(content);
			fileW.Flush();
			fileW.Close();
			file.Close();
		}
	}
}

