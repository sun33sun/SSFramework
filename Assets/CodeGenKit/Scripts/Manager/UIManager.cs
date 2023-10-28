using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using ProjectBase.CodeGenKit;
using System.Linq;
using DG.Tweening;

namespace ProjectBase.UI
{
	public enum ManagerState
	{
		None, Loading, Loaded
	}
	public static class UIManager
	{
		public const float ShowTime = 0.5f;
		public const float HideTime = 0.5f;

		static Dictionary<string, UIController> prefabDic = new Dictionary<string, UIController>();
		static Dictionary<string, UIController> instanceDic = new Dictionary<string, UIController>();

		public static ManagerState loadState { get; private set; }

		public static void LoadPrefabAsync()
		{
			if (loadState != ManagerState.None)
				return;

			loadState = ManagerState.Loading;

			TextAsset textAsset = Resources.Load<TextAsset>(CodeGenSetting.PrefabJsonName);
			string json = textAsset.text;
			Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			foreach (var item in dic)
			{
				UIController controller = Resources.Load<UIController>(item.Value);
				prefabDic.Add(item.Key, controller);
			}

			loadState = ManagerState.Loaded;
		}

		#region 创建
		/// <summary>
		/// 加载预制体
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		static T Load<T>() where T : UIController
		{
			Type type = typeof(T);
			if (instanceDic.ContainsKey(type.Name))
				GameObject.Destroy(instanceDic[type.Name]);

			T t = GameObject.Instantiate(prefabDic[type.Name], UIRoot.Instance.panelRoot) as T;
			t.name = prefabDic[type.Name].name;

			instanceDic[type.Name] = t;
			return t;
		}

		public static T Open<T>() where T : UIController
		{
			T t = Load<T>();
			t.group.alpha = 1;
			t.group.interactable = true;

			return t;
		}

		public static async UniTask<T> OpenFade<T>() where T : UIController
		{
			T t = Load<T>();
			await t.group.DOFade(1, ShowTime).AsyncWaitForCompletion();
			t.group.interactable = true;
			return t;
		}
		#endregion

		#region 关闭
		static KeyValuePair<string, UIController> Pop()
		{
			KeyValuePair<string, UIController> pair = instanceDic.Last();
			instanceDic.Remove(pair.Key);
			return pair;
		}

		public static void Close()
		{
			if (instanceDic.Count < 1)
				return;
			GameObject.Destroy(Pop().Value);
		}

		public static async UniTask CloseFade()
		{
			if (instanceDic.Count < 1)
				return;

			UIController controller = Pop().Value;
			controller.group.interactable = false;
			//动画
			await controller.group.DOFade(0, ShowTime).AsyncWaitForCompletion();
			//销毁
			GameObject.Destroy(controller);
		}
		#endregion

		public static T Get<T>() where T : UIController
		{
			Type type = typeof(T);
			if (instanceDic.ContainsKey(type.Name))
			{
				return instanceDic[type.Name] as T;
			}
			else
			{
				return null;
			}
		}

		#region 显示
		public static void Show<T>() where T : UIController
		{
			T t = Get<T>();
			if (t != null)
			{
				t.gameObject.SetActive(true);
				t.group.interactable = true;
				t.group.alpha = 1;
			}
		}

		public static async void ShowFade<T>() where T : UIController
		{
			T t = Get<T>();
			if (t != null)
			{
				t.gameObject.SetActive(true);
				await t.group.DOFade(1, ShowTime).AsyncWaitForCompletion();
				t.group.interactable = true;
			}
		}

		#endregion

		#region 隐藏
		public static void Hide<T>() where T : UIController
		{
			T t = Get<T>();
			if (t != null)
			{
				t.gameObject.SetActive(false);
				t.group.interactable = false;
				t.group.alpha = 0;
			}
		}

		public static async void HideFade<T>() where T : UIController
		{
			T t = Get<T>();
			if (t != null)
			{
				t.group.interactable = false;
				await t.group.DOFade(0, HideTime).AsyncWaitForCompletion();
				t.gameObject.SetActive(false);
			}
		}
		#endregion
	}
}

