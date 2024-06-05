using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectBase
{
	public static class EnumCodeGenarator
	{
		private static string filePath = $"{Application.dataPath}/ProjectBase/GenData/GenData.cs";

		private static string EnumDefaultTemplate =>
@"using System;

public enum EmTag
{
	//EmTag

	//EmTag
}

public enum EmLayer
{
	//EmLayer

	//EmLayer
}

public enum EmScene
{
	//EmScene

	//EmScene
}
";
#if UNITY_EDITOR
		public static void GenTemplate()
		{
			if (!File.Exists(filePath))
			{
				File.WriteAllText(EnumDefaultTemplate, filePath);
			}
		}


		private static void ReplaceByName(string newPartContent, string name)
		{
			//��֤�ļ�����
			GenTemplate();

			//�ؼ���
			string key = $"//{name}";

			//���ļ���ȡ ���滻��ȫ��������
			string allContent = File.ReadAllText(filePath);
			//������ʽ
			string pattern = $@"{Regex.Escape(key)}.*?{Regex.Escape(key)}";
			Regex regex = new Regex(pattern, RegexOptions.Singleline);
			//ʹ��������ʽ
			var match = regex.Match(allContent);
			if (match.Success)
			{
				//Ҫ�滻��������
				newPartContent = $"{key}\n\r{newPartContent}\n\r\t{key}";
				//�滻
				allContent = regex.Replace(allContent, newPartContent);
				//д���ļ�
				File.WriteAllText(filePath, allContent);
			}
		}

		[MenuItem("Tools/GenEnum/GenTagEnum")]
		public static void GenTagEnum()
		{
			StringBuilder sb = new StringBuilder();
			var tags = InternalEditorUtility.tags;
			for (int i = 0; i < tags.Length; i++)
			{
				var name = tags[i].Replace(" ", string.Empty);
				if (i == 0)
				{
					sb.Append("\t").Append(name);
				}
				else if (i == tags.Length - 1)
				{
					sb.Append(name);
				}
				else
				{
					sb.Append(name).Append(", ");
				}
			}

			ReplaceByName(sb.ToString(), "EmTag");
		}


		[MenuItem("Tools/GenEnum/GenLayerEnum")]
		public static void GenLayerEnum()
		{
			StringBuilder sb = new StringBuilder();
			var layers = InternalEditorUtility.layers;
			for (int i = 0; i < layers.Length; i++)
			{
				var hashCode = LayerMask.NameToLayer(layers[i]);
				var name = layers[i].Replace(" ", string.Empty);
				name = $"{name} = {hashCode}";
				if (i == 0)
				{
					if(i == layers.Length - 1)
						sb.Append("\t").Append(name);
					else
						sb.Append("\t").Append(name).Append(", ");
				}
				else if (i == layers.Length - 1)
				{
					sb.Append(name);
				}
				else
				{
					sb.Append(name).Append(", ");
				}
			}

			ReplaceByName(sb.ToString(), "EmLayer");
		}

		[MenuItem("Tools/GenEnum/GenSceneEnum")]
		public static void GenSceneEnum()
		{
			StringBuilder sb = new StringBuilder();
			int count = SceneManager.sceneCountInBuildSettings;
			for (int i = 0; i < count; i++)
			{
				var name = SceneUtility.GetScenePathByBuildIndex(i).Split('/').Last().Replace(" ", string.Empty)
					.Replace(".unity", string.Empty);
				if (i == 0)
				{
					sb.Append("\t").Append(name);
				}
				else if (i == count - 1)
				{
					sb.Append(name);
				}
				else
				{
					sb.Append(name).Append(", ");
				}
			}

			ReplaceByName(sb.ToString(), "EmScene");
		}
#endif

	}
}