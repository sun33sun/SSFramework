#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectBase.CodeGenKit
{
	[System.Serializable]
	public class BindInfo
	{
		public string MemberName;
		public Object Object;
	}

	public class BindInfoComparer : IComparer<BindInfo>
	{
		public int Compare(BindInfo a, BindInfo b)
		{
			return string.Compare(a.MemberName, b.MemberName, StringComparison.Ordinal);
		}
	}

	public class BaseBind : MonoBehaviour
	{
		public List<BindInfo> infos = new List<BindInfo>();
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(BaseBind))]
	public class BaseBindEditor : Editor
	{
		protected BaseBind mBaseBind;

		protected virtual void OnEnable()
		{
			mBaseBind = (BaseBind)target;
		}

		public override void OnInspectorGUI()
		{
			Undo.RecordObject(mBaseBind, "Changed Settings");
			var dataProperty = serializedObject.FindProperty("infos");
			EditorGUILayout.BeginHorizontal();

			GUILayout.EndHorizontal();
			EditorGUILayout.Space();

			var delList = new List<int>();
			SerializedProperty property;
			for (int i = mBaseBind.infos.Count - 1; i >= 0; i--)
			{
				GUILayout.BeginHorizontal();
				property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("MemberName");
				property.stringValue = EditorGUILayout.TextField(property.stringValue, GUILayout.Width(150));
				property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("Object");
				property.objectReferenceValue =
					EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(Object), true);

				if (property.objectReferenceValue is Component component)
				{
					var objects = new List<Object>();
					objects.AddRange(component.gameObject.GetComponents<Component>());
					objects.Add(component.gameObject);

					var index = objects.FindIndex(c => c.GetType() == property.objectReferenceValue.GetType());
					var newIndex = EditorGUILayout.Popup(index, objects.Select(c => c.GetType().FullName).ToArray());
					if (index != newIndex)
					{
						property.objectReferenceValue = objects[newIndex];
					}
				}
				else if (property.objectReferenceValue is GameObject gameObject)
				{
					var objects = new List<Object>();
					objects.AddRange(gameObject.GetComponents<Component>());
					objects.Add(gameObject);

					var index = objects.FindIndex(c => c.GetType() == property.objectReferenceValue.GetType());
					var newIndex = EditorGUILayout.Popup(index, objects.Select(c => c.GetType().FullName).ToArray());
					if (index != newIndex)
					{
						property.objectReferenceValue = objects[newIndex];
					}
				}

				if (GUILayout.Button("X"))
				{
					//将元素添加进删除list
					delList.Add(i);
				}

				GUILayout.EndHorizontal();
			}

			GUILayout.Label("将其他需要生成变量的 Object 拖拽至此");
			var sfxPathRect = EditorGUILayout.GetControlRect();
			sfxPathRect.height = 50;
			GUI.Box(sfxPathRect, string.Empty);
			EditorGUILayout.LabelField(string.Empty, GUILayout.Height(35));
			if (
				(Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
				&& sfxPathRect.Contains(Event.current.mousePosition)
			)
			{
				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

				if (Event.current.type == EventType.DragPerform)
				{
					DragAndDrop.AcceptDrag();
					foreach (var o in DragAndDrop.objectReferences)
					{
						string key = o.name.Replace(" ", "");
						key = key.Replace("-", "");
						key = key.Replace("@", "");
						AddReference(dataProperty, key, o);
					}
				}

				Event.current.Use();
			}

			if (!Application.isPlaying)
			{
				if (GUILayout.Button("Generate Code", GUILayout.Height(30)))
				{
					BuilderManager.Instance.GenerateCode(mBaseBind);
				}

				if (GUILayout.Button("Find Reference", GUILayout.Height(30)))
				{
					BuilderManager.Instance.FindReference(mBaseBind);
				}

				if (GUILayout.Button("Save As Prefab", GUILayout.Height(30)))
				{
					BuilderManager.Instance.SaveAsPrefab(mBaseBind);
				}
			}


			GUILayout.BeginHorizontal();

			EditorGUILayout.EndHorizontal();

			foreach (var i in delList)
			{
				dataProperty.DeleteArrayElementAtIndex(i);
			}

			serializedObject.ApplyModifiedProperties();
			serializedObject.UpdateIfRequiredOrScript();
		}

		public virtual void AddReference(SerializedProperty dataProperty, string key, Object obj)
		{
			int index = dataProperty.arraySize;
			dataProperty.InsertArrayElementAtIndex(index);
			var element = dataProperty.GetArrayElementAtIndex(index);
			element.FindPropertyRelative("MemberName").stringValue = key;
			element.FindPropertyRelative("Object").objectReferenceValue = obj;
		}
	}
#endif
}