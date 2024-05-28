#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace ProjectBase.CodeGenKit
{
	public class PanelBuilder : BaseBuilder
	{
		public PanelBuilder(ITemplate template) : base(template)
		{
		}

		public override void FindReference(BaseBind baseBind)
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

			//为UI固定的属性赋值
			serializedObject.FindProperty("canvas").objectReferenceValue = gameObject.GetComponent<Canvas>();
			serializedObject.FindProperty("raycaster").objectReferenceValue = gameObject.GetComponent<GraphicRaycaster>();
			serializedObject.FindProperty("group").objectReferenceValue = gameObject.GetComponent<CanvasGroup>();


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

		public override void SaveAsPreab(BaseBind baseBind)
		{
			Canvas canvas = baseBind.GetComponent<Canvas>();
			canvas.overrideSorting = true;
			canvas.sortingLayerID = 0;
			canvas.additionalShaderChannels = 0;

			base.SaveAsPreab(baseBind);

			//读取Json
			Dictionary<string, string> prefabDic = CodeGenHelper.PrefabDic;
			//写入DataMgr
			WriteDataMgrDesigner(prefabDic.Keys.Where(key=>key.Contains("Panel")).ToList());
		}

		public void WriteDataMgrDesigner(List<string> nameList)
		{
			StringBuilder sb = new StringBuilder();

			foreach (var name in nameList)
			{
				sb.Append($"\tdataDic.Add(typeof({name}Data), new {name}Data());\r\n\t");
			}
			string json = CodeGenGlobalSetting.DataMgrTemplate;
			json = json.Replace(CodeTemplateMarker.DATA, sb.ToString());
			CodeGenHelper.Write(CodeGenGlobalSetting.DataMgrPath, json);
		}
	}
}
#endif