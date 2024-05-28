#if UNITY_EDITOR
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;
using UnityEngine;
using UnityEditor;
namespace ProjectBase.CodeGenKit
{
	public class BaseBuilder
	{
		protected ITemplate template = null;

		private BaseBuilder()
		{

		}

		public BaseBuilder(ITemplate template)
		{
			this.template = template;
		}

		/// <summary>
		/// ���������CS�ļ�������CS�ļ����������CS�ļ������������׶�
		/// </summary>
		/// <param name="className"></param>
		/// <param name="baseBind"></param>
		protected virtual void WriteCS(string className, BaseBind baseBind)
		{
			string classPath = $"{Application.dataPath}/{template.CodePath}/{className}{FileSuffix.classSuffix}";

			if (!File.Exists(classPath))
			{
				string classStr = template.ClassTemplate;
				classStr = classStr.Replace(CodeTemplateMarker.NAMESPACE, template.NameSpace);
				classStr = classStr.Replace(CodeTemplateMarker.CLASSNAME, className);
				CodeGenHelper.Write(classPath, classStr);
			}
		}

		/// <summary>
		/// ɾ������������Designer�ļ�
		/// </summary>
		/// <param name="className"></param>
		/// <param name="baseBind"></param>
		protected virtual void WriteDesigner(string className, BaseBind baseBind)
		{
			string designerPath = $"{Application.dataPath}/{template.CodePath}/{className}{FileSuffix.designerSuffix}";
			if (File.Exists(designerPath))
				File.Delete(designerPath);

			//��Ա�����ַ���
			StringBuilder sbMembers = new StringBuilder();
			foreach (var otherBind in baseBind.infos)
			{
				string fieldLine = $"\t[HideInInspector, SerializeField] public {otherBind.Object.GetType().FullName} {otherBind.MemberName} = null;\r\n\t";
				sbMembers.Append(fieldLine);
			}

			string designerStr = template.DesignerTemplate;
			designerStr = designerStr.Replace(CodeTemplateMarker.NAMESPACE, template.NameSpace);
			designerStr = designerStr.Replace(CodeTemplateMarker.CLASSNAME, className);
			designerStr = designerStr.Replace(CodeTemplateMarker.MEMBER, sbMembers.ToString());
			CodeGenHelper.Write(designerPath, designerStr);
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

			Debug.Log("�����ű� " + Application.dataPath + "/Scripts/" + baseBind.name + ".cs �ɹ�!");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
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

		/// <summary>
		/// ����Ԥ����
		/// </summary>
		/// <param name="baseBind"></param>
		/// <returns></returns>
		public virtual void SaveAsPreab(BaseBind baseBind)
		{
			GameObject inspectorObj = baseBind.gameObject;

			//Ŀ¼�������򴴽� 
			string dirPath = $"Assets/Resources/{template.PrefabPath}";
			if (!Directory.Exists(dirPath))
				Directory.CreateDirectory(dirPath);

			//����Ԥ����
			string prefabPath = $"{dirPath}/{inspectorObj.name}{FileSuffix.prefabSuffix}";
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
			Dictionary<string, string> prefabDic = CodeGenHelper.PrefabDic;

			Type type = inspectorObj.GetComponent<BaseController>().GetType();
			string key = type.Name;
			if (prefabDic.ContainsKey(key))
			{
				prefabDic[key] = $"{template.PrefabPath}/{inspectorObj.name}";
			}
			else
				prefabDic.Add(key, $"{template.PrefabPath}/{inspectorObj.name}");
			//����Ԥ����·����Json
			string json = JsonConvert.SerializeObject(prefabDic);
			CodeGenHelper.Write(CodeGenGlobalSetting.PrefabJsonPath, json);
			EditorUtility.FocusProjectWindow();
		}
	}
}
#endif
