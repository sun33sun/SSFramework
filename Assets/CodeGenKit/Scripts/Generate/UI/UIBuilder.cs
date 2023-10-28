using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Linq;

namespace ProjectBase.CodeGenKit
{
#if UNITY_EDITOR
	public class UIBuilder : BaseBuilder
	{
		public UIBuilder(UITemplate template) : base(template)
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
			CanvasGroup group = baseBind.GetComponent<CanvasGroup>();
			group.alpha = 0;
			group.interactable = false;
			base.SaveAsPreab(baseBind);
		}
	}
#endif

}