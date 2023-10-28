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

			//д��.cs�ļ�
			WriteCS(className, baseBind);

			//д��.Designer.cs�ļ�
			WriteDesigner(className, baseBind);

			//д��ControllerEnum.cs�ļ�
			WriteEnum(className);

			Debug.Log("�����ű� " + Application.dataPath + "/Scripts/" + baseBind.name + ".cs �ɹ�!");
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

			//��Ա�����ַ���
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

			//���OtherBinds��Member
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

			Debug.Log($"{gameObject.name}��{typeName}����������óɹ���");
		}

		public virtual void SaveAsPreab(BaseBind baseBind)
		{
			GameObject inspectorObj = baseBind.gameObject;

			string prefabPath = $"Assets/Resources/{template.PrefabPath}/{inspectorObj.name}{CodeSuffix.prefabSuffix}";
			//����Ԥ����
			GameObject oldPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
			if (oldPrefab != null)
			{
				AssetDatabase.DeleteAsset(prefabPath);
				Debug.Log("Deleted Existing Prefab: " + prefabPath);
			}
			GameObject newPrefab = PrefabUtility.SaveAsPrefabAsset(inspectorObj, prefabPath);
			if (newPrefab != null)
				Debug.Log("Prefab Saved As: " + prefabPath);
			//��ȡJson
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
			//����Ԥ����·����Json
			json = JsonConvert.SerializeObject(prefabDic);
			Write(CodeGenSetting.PrefabJsonPath, json);

			EditorUtility.FocusProjectWindow();
		}
	}
}
#endif
