using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;


namespace ProjectBase.Exam
{
	public class QuestionParent : MonoBehaviour
	{
		[Header("选项变化的颜色")][SerializeField] private Color _colorNormal = Color.white;
		[SerializeField] private Color _colorSelected = Color.yellow;
		[SerializeField] private Color _colorError = Color.red;
		[SerializeField] private Color _colorCorrect = Color.green;
		[SerializeField] private Color _colorAyalysisError = Color.red;
		[SerializeField] private Color _colorAyalysisCorrect = Color.green;

		[Header("考题生成位置")] public Transform _questionParent;
		public Scrollbar scrollbar;
		[Header("_得分")] public List<float> _scoreList = new List<float>();

		[Header("生成考题后需要放在最后的物体")] public List<GameObject> _lastGos;

		public float GetedTotalScore
		{
			get
			{
				float getedTotalScore = 0f;
				foreach (var VARIABLE in _scoreList)
					getedTotalScore += VARIABLE;
				return getedTotalScore;
			}
		}

		public float TotalScore
		{
			get
			{
				float getedTotalScore = 0f;
				foreach (var VARIABLE in _questionDatas)
					getedTotalScore += VARIABLE.score;
				return getedTotalScore;
			}
		}


		[Header("——角标格式:[类型|尺寸|颜色=内容]——")]
		[Header("*类型:1 上标、2 下标、3 上标带中括号、4 下标带中括号*")]
		[Header("*尺寸:默认为文本尺寸的1/2*")]
		[Header("*颜色:格式为颜色码；如FF0000FF*")]
		[Header("*内容:正常输入字符、如角标内容含有[]，则使用3 4类型*")]
		[Space(10)]
		public List<QuestionData> _questionDatas = new List<QuestionData>();

		private GameObject questionPrefab; //没有把这个鬼东西改为自动查找预制体，目的是为了如果有多种款式的界面 可以自己选择  结果这个想法也没等到扩展
		private GameObject optionPrefab;

		[HideInInspector] public List<QuestionItem> _items = new List<QuestionItem>();

		public void LoadQuestion(string jsonPath, string questionPrefabPath, string optionPrefabPath)
		{
			//删除旧题
			for (int i = _items.Count - 1; i >= 0; i--)
				Destroy(_items[i].gameObject);
			_items.Clear();
			_scoreList.Clear();

			//加载题目
			string json = Resources.Load<TextAsset>(jsonPath).text;
			_questionDatas = JsonConvert.DeserializeObject<List<QuestionData>>(json);
			//加载预制体
			questionPrefab = Resources.Load<GameObject>(questionPrefabPath);
			optionPrefab = Resources.Load<GameObject>(optionPrefabPath);

			GameObject go;
			for (int i = 0; i < _questionDatas.Count; i++)
			{
				go = Instantiate(questionPrefab, _questionParent);
				_items.Add(go.GetComponent<QuestionItem>());
				List<string> AllChoiceContent = new List<string>();
				foreach (var ChoiceContent in _questionDatas[i].optionDic)
					AllChoiceContent.Add(ChoiceContent.Key + ":" + ChoiceContent.Value + "E");
				//设置Item数据
				_items[i].SetColor(_colorNormal, _colorSelected, _colorError, _colorCorrect, _colorAyalysisError,
					_colorAyalysisCorrect);
				_items[i].SetQuestion(_questionDatas[i].strHead, AllChoiceContent, _questionDatas[i].choiceType,
					optionPrefab);
			}

			if (_lastGos != null)
			{
				foreach (GameObject lastGo in _lastGos)
					lastGo.transform.SetAsLastSibling();
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(_questionParent.transform as RectTransform);

			if (scrollbar != null)
				scrollbar.value = 1;
		}

		public void Submit()
		{
			for (int i = 0; i < _items.Count; i++)
			{
				if (_questionDatas[i].strAnalysis == null)
				{
					_questionDatas[i].strAnalysis = "";
				}

				if (_questionDatas[i].choiceType == QuestionData.ChoiceType.Single)
				{
					_scoreList.Add(_items[i].SingleJudge(_questionDatas[i].singleRight, _questionDatas[i].strAnalysis,
						_questionDatas[i].score));
				}
				else
				{
					_scoreList.Add(_items[i].MultipleJudge(_questionDatas[i].multipleRights, _questionDatas[i].strAnalysis,
						_questionDatas[i].score));
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(_questionParent.transform as RectTransform);
		}

		private void OnEnable()
		{
			foreach (var tool in _items)
			{
				tool.Reset();
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(_questionParent.transform as RectTransform);
		}
	}
}