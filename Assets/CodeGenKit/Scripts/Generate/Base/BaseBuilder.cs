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
	public class BaseBuilder
	{
		protected Template template = null;

		private BaseBuilder()
		{

		}

		public BaseBuilder(Template template)
		{
			this.template = template;
		}

		public virtual void GenerateCode(BaseBind baseBind)
		{
			StringBuilder sb = new StringBuilder(baseBind.name);
			sb[0] = Char.ToUpper(sb[0]);
			string className = sb.ToString();

			//写入.cs文件
			WriteCS(className, baseBind);

			//写入.Designer.cs文件
			WriteDesigner(className, baseBind);

			//写入ControllerEnum.cs文件
			WriteEnum(className);

			Debug.Log("创建脚本 " + Application.dataPath + "/Scripts/" + baseBind.name + ".cs 成功!");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		protected virtual void WriteCS(string className, BaseBind baseBind)
		{
			string classPath = Application.dataPath + "/" + template.CodePath + "/" + className + CodeSuffix.classSuffix;

			if (!File.Exists(classPath))
			{
				string classStr = template.ClassTemplate;
				classStr = classStr.Replace(CodeTemplateMarker.NAMESPACE, template.NameSpace);
				classStr = classStr.Replace(CodeTemplateMarker.CLASSNAME, className);
				Write(classPath, classStr);
			}
		}

		protected virtual void WriteDesigner(string className, BaseBind baseBind)
		{
			string designerPath = Application.dataPath + "/" + template.CodePath + "/" + className + CodeSuffix.designerSuffix;
			if (File.Exists(designerPath))
				File.Delete(designerPath);

			//成员变量字符串
			StringBuilder sbMembers = new StringBuilder();
			foreach (var otherBind in baseBind.infos)
			{
				string fieldLine = $"\tpublic {otherBind.Object.GetType().FullName} {otherBind.MemberName} = null;\r\n\t";
				sbMembers.Append(fieldLine);
			}

			string designerStr = template.DesignerTemplate;
			designerStr = designerStr.Replace(CodeTemplateMarker.NAMESPACE, template.NameSpace);
			designerStr = designerStr.Replace(CodeTemplateMarker.CLASSNAME, className);
			designerStr = designerStr.Replace(CodeTemplateMarker.MEMBER, sbMembers.ToString());
			Write(designerPath, designerStr);
		}

		protected virtual void WriteEnum(string className)
		{
			string json = File.ReadAllText(CodeGenSetting.ControllerTypeJsonPath);
			List<string> list = JsonConvert.DeserializeObject<List<string>>(json);
			if (list == null)
				list = new List<string>();
			string newPart = $"\t{className}\n";
			list.Add(newPart);
			StringBuilder sb = new StringBuilder();
			foreach (string item in list)
			{
				sb.Append(item);
			}
			string content = CodeGenSetting.ControllerEnum.Replace(CodeTemplateMarker.ENUM, sb.ToString());
			Write(CodeGenSetting.ControllerTypeJsonPath, JsonConvert.SerializeObject(list));
		}

		protected void Write(string filePath, string content)
		{
			if (File.Exists(filePath))
				File.Delete(filePath);

			FileStream file = new FileStream(filePath, FileMode.CreateNew);
			StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
			fileW.Write(content);
			fileW.Flush();
			fileW.Close();
			file.Close();
		}

		public virtual void FindReference(BaseBind baseBind)
		{
			GameObject gameObject = baseBind.gameObject;

			var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly =>
				!assembly.FullName.StartsWith("Unity"));

			var typeName = template.NameSpace + "." + baseBind.name;

			var type = assemblies.Where(a => a.GetType(typeName) != null)
				.Select(a => a.GetType(typeName)).FirstOrDefault();

			if (type == null)
				return;

			var scriptComponent = gameObject.GetComponent<BaseController>();

			if (!scriptComponent)
			{
				scriptComponent = gameObject.AddComponent(type) as BaseController;
			}

			//填充OtherBinds的Member
			var serializedObject = new SerializedObject(scriptComponent);
			foreach (var item in baseBind.infos)
			{
				var componentName = item.MemberName;
				var serializedProperty = serializedObject.FindProperty(item.MemberName);

				serializedProperty.objectReferenceValue = item.Object;
			}

			serializedObject.ApplyModifiedProperties();
			serializedObject.UpdateIfRequiredOrScript();

			EditorUtility.SetDirty(baseBind.gameObject);

			Debug.Log($"{gameObject.name}的{typeName}组件查找引用成功！");
		}

		public virtual void SaveAsPreab(BaseBind baseBind)
		{
			GameObject inspectorObj = baseBind.gameObject;

			string prefabPath = $"Assets/Resources/{template.PrefabPath}/{inspectorObj.name}{CodeSuffix.prefabSuffix}";
			//保存预制体
			GameObject oldPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
			if (oldPrefab != null)
			{
				AssetDatabase.DeleteAsset(prefabPath);
				Debug.Log("Deleted Existing Prefab: " + prefabPath);
			}
			GameObject newPrefab = PrefabUtility.SaveAsPrefabAsset(inspectorObj, prefabPath);
			if (newPrefab != null)
				Debug.Log("Prefab Saved As: " + prefabPath);
			//读取Json
			string json = Resources.Load<TextAsset>(CodeGenSetting.PrefabJsonName).text;
			Dictionary<string, string> prefabDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			if (prefabDic == null)
				prefabDic = new Dictionary<string, string>();

			string key = inspectorObj.GetComponent<BaseController>().GetType().Name;
			if (prefabDic.ContainsKey(key))
			{
				prefabDic[key] = $"{template.PrefabPath}/{inspectorObj.name}";
			}
			else
				prefabDic.Add(key, $"{template.PrefabPath}/{inspectorObj.name}");
			//保存预制体路径至Json
			json = JsonConvert.SerializeObject(prefabDic);
			Write(CodeGenSetting.PrefabJsonPath, json);

			EditorUtility.FocusProjectWindow();
		}
	}
}
#endif
