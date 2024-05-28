using Cysharp.Threading.Tasks;

namespace ProjectBase.UI
{
	public static class UIKit
	{
		#region 创建
		public static T Open<T>() where T : UIController
		{
			return UIMgr.Instance.Open<T>();
		}

		public static async UniTask<T> OpenAwait<T>() where T : UIController
		{
			T t = await UIMgr.Instance.OpenAwait<T>();
			return t;
		}

		public static void OpenAsync<T>()where T : UIController
		{
			UniTask.Void(UIMgr.Instance.OpenAsync<T>);
		}

		public static void UnRecordOpen<T>(int sortingOrder) where T : UIController 
		{
			UIMgr.Instance.UnRecordOpen<T>(sortingOrder);
		}
		#endregion

		#region 关闭
		public static void CloseLast()
		{
			UIMgr.Instance.CloseLast();
		}

		public static void CloseLastAsync()
		{
			UniTask.Void(UIMgr.Instance.CloseAllAsync);
		}

		public static async UniTask CloseLastAwait()
		{
			await UIMgr.Instance.CloseLastAwait();
		}

		public static void CloseAll()
		{
			UIMgr.Instance.CloseAll();
		}

		public static void CloseAllAsync()
		{
			UniTask.Void(UIMgr.Instance.CloseAllAsync);
		}

		public static async UniTask CloseAllAwait()
		{
			await UIMgr.Instance.CloseAllAwait();
		}


		#endregion

		/// <summary>
		/// 获取
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Get<T>() where T : UIController
		{
			return UIMgr.Instance.Get<T>();
		}

		#region 显示
		public static void Show<T>() where T : UIController
		{
			UIMgr.Instance.Show<T>();
		}

		public static void ShowFade<T>() where T : UIController
		{
			UIMgr.Instance.ShowFade<T>();
		}

		#endregion

		#region 隐藏
		public static void Hide<T>() where T : UIController
		{
			UIMgr.Instance.Hide<T>();
		}

		public static void HideFade<T>() where T : UIController
		{
			UIMgr.Instance.HideFade<T>();
		}
		#endregion
	}
}

