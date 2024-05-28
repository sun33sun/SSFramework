using Newtonsoft.Json;
using ProjectBase.Exam;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace QuestionKit
{
	public class QuestionEditorWindow : OdinEditorWindow
	{
		[MenuItem("Tools/QuestionKit/QuestionEditWindow")]
		private static void OpenWindow()
		{
			var window = GetWindow<QuestionEditorWindow>();

			window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
		}

		[OdinSerialize] private string textAssetName;
		[OdinSerialize] private List<QuestionData> questionDatas;

		private string GetPath(string fileName) => $"{Application.dataPath}/QuestionKit/Resources/{fileName}.json";

		[PropertySpace(20)][Button]
		private void LoadJson(TextAsset newTextAsset)
		{
			textAssetName = newTextAsset.name;
			string path = GetPath(textAssetName);
			string content = File.ReadAllText(path);
			questionDatas = JsonConvert.DeserializeObject<List<QuestionData>>(newTextAsset.text);
		}

		[PropertySpace(20)]
		[Button]
		private void SaveJson()
		{
			if (textAssetName.IsNullOrWhitespace())
				return;

			string content = JsonConvert.SerializeObject(questionDatas);
			File.WriteAllText(GetPath(textAssetName), content);
			AssetDatabase.Refresh();
		}

		[PropertySpace(20)]
		[Button]
		private void CheckQuestionData()
		{

		}
	}
}
