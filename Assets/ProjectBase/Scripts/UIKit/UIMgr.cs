using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using ProjectBase.CodeGenKit;
using System.Linq;
using DG.Tweening;
using ProjectBase.Res;

namespace ProjectBase.UI
{
	public class UIMgr : Singleton<UIMgr>
	{
		public const float animTime = 0.5f;

		Dictionary<Type, UIController> instanceDic = new Dictionary<Type, UIController>();

		Stack<UIController> uiStack = new Stack<UIController>();

		//UIController Pop(Type type)
		//{
		//	instanceDic
		//}

		public ManagerState loadState { get; private set; }

		private UIMgr() { }

		#region 创建
		/// <summary>
		/// 加载预制体
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T Load<T>() where T : UIController
		{
			Type type = typeof(T);
			//存在则销毁
			if (instanceDic.ContainsKey(type))
			{
				UIController uiController = instanceDic[type];
				instanceDic.Remove(type);
				GameObject.Destroy(uiController.gameObject);
			}
			//将上一个面板的交互关闭
			if (uiStack.Count > 0)
				uiStack.Peek().group.interactable = false;
			//设置新加载的面板
			T t = ResMgr.Instance.Load<T>(type, UIRoot.Instance.CommomLayer);
			t.canvas.overrideSorting = true;
			t.canvas.sortingOrder = instanceDic.Count;
			t.name = type.Name;
			instanceDic[type] = t;
			return t;
		}

		public T Open<T>() where T : UIController
		{
			T t = Load<T>();
			uiStack.Push(t);
			t.group.alpha = 1;
			t.group.interactable = true;

			return t;
		}

		public async UniTask<T> OpenAwait<T>() where T : UIController
		{
			T t = Load<T>();
			uiStack.Push(t);
			await t.group.DOFade(1, animTime).AsyncWaitForCompletion();
			t.group.interactable = true;
			return t;
		}

		public async UniTaskVoid OpenAsync<T>() where T : UIController
		{
			T t = Load<T>();
			uiStack.Push(t);
			await t.group.DOFade(1, animTime).AsyncWaitForCompletion();
			t.group.interactable = true;
		}

		public T UnRecordOpen<T>(int sortingOrder) where T : UIController
		{
			T t = Load<T>();
			t.group.alpha = 1;
			t.group.interactable = true;

			t.canvas.sortingOrder = sortingOrder;

			return t;
		}
		#endregion

		#region 关闭

		public void CloseLast()
		{
			if (uiStack.Count < 1)
				return;
			UIController uiController = uiStack.Pop();
			Type type = uiController.GetType();
			instanceDic.Remove(type);
			uiController.group.interactable = false;

			GameObject.Destroy(uiController.gameObject);
		}

		public async UniTaskVoid CloseLastAsync()
		{
			if (uiStack.Count < 1)
				return;

			UIController controller = uiStack.Pop();
			controller.group.interactable = false;
			//动画
			await controller.group.DOFade(0, animTime).AsyncWaitForCompletion();
			//销毁
			instanceDic.Remove(controller.GetType());
			GameObject.Destroy(controller.gameObject);
		}

		public async UniTask CloseLastAwait()
		{
			if (uiStack.Count < 1)
				return;

			UIController controller = uiStack.Pop();
			controller.group.interactable = false;
			//动画
			await controller.group.DOFade(0, animTime).AsyncWaitForCompletion();
			//销毁
			instanceDic.Remove(controller.GetType());
			GameObject.Destroy(controller.gameObject);
		}

		public void CloseAll()
		{
			while (uiStack.Count > 0)
			{
				CloseLast();
			}
			uiStack.Clear();
		}

		public async UniTask CloseAllAwait()
		{
			foreach (var item in uiStack)
				item.group.interactable = false;

			while (uiStack.Count > 0)
				await CloseLastAwait();
		}

		public async UniTaskVoid CloseAllAsync()
		{
			foreach(var item in uiStack)
				item.group.interactable = false;

			while (uiStack.Count > 0)
				await CloseLastAwait();
		}
		#endregion

		public T Get<T>() where T : UIController
		{
			Type type = typeof(T);
			if (instanceDic.ContainsKey(type))
			{
				return instanceDic[type] as T;
			}
			else
			{
				return null;
			}
		}

		#region 显示
		public void Show<T>() where T : UIController
		{
			T t = Get<T>();
			if (t != null)
			{
				t.gameObject.SetActive(true);
				t.group.interactable = true;
				t.group.alpha = 1;
			}
		}

		public async void ShowFade<T>() where T : UIController
		{
			T t = Get<T>();
			if (t != null)
			{
				t.gameObject.SetActive(true);
				await t.group.DOFade(1, animTime).AsyncWaitForCompletion();
				t.group.interactable = true;
			}
		}

		#endregion

		#region 隐藏
		public void Hide<T>() where T : UIController
		{
			T t = Get<T>();
			if (t != null)
			{
				t.gameObject.SetActive(false);
				t.group.interactable = false;
				t.group.alpha = 0;
			}
		}

		public async void HideFade<T>() where T : UIController
		{
			T t = Get<T>();
			if (t != null)
			{
				t.group.interactable = false;
				await t.group.DOFade(0, animTime).AsyncWaitForCompletion();
				t.gameObject.SetActive(false);
			}
		}
		#endregion
	}
}

