using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using ProjectBase.CodeGenKit;
using System.Linq;
using DG.Tweening;

namespace ProjectBase.Res
{
	public enum ManagerState
	{
		None, Loading, Loaded
	}
	public class ResMgr : Singleton<ResMgr>
	{

		Dictionary<Type, BaseController> prefabDic = new Dictionary<Type, BaseController>();

		public ManagerState loadState { get; private set; }

		private ResMgr() { }

		public override void OnSingletonInit()
		{
			LoadPrefabSync();
		}

		void LoadPrefabSync()
		{
			if (loadState != ManagerState.None)
				return;

			loadState = ManagerState.Loading;

			TextAsset textAsset = Resources.Load<TextAsset>(CodeGenGlobalSetting.PrefabJsonName);
			string json = textAsset.text;
			Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			if (dic == null)
				return;
			foreach (var item in dic)
			{
				BaseController controller = Resources.Load<BaseController>(item.Value);
				prefabDic.Add(controller.GetType(), controller);
			}

			loadState = ManagerState.Loaded;
		}

		public T Load<T>(Transform parent) where T : BaseController
		{
			Type type = typeof(T);

			return Load<T>(type, parent);
		}

		public T Load<T>(Type type, Transform parent) where T : BaseController
		{
			BaseController prefab;
			if (prefabDic.TryGetValue(type, out prefab))
			{
				return GameObject.Instantiate<T>(prefab as T, parent);
			}

			return null;
		}

		/// <summary>
		/// 需要反复生成时，使用该方法
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetPrefab<T>() where T : BaseController
		{
			Type type = typeof(T);
			BaseController prefab;
			if (prefabDic.TryGetValue(type, out prefab))
			{
				return prefab as T;
			}

			return null;
		}
	}
}

