using ProjectBase.Res;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectBase.UI
{
	public class UIRoot : PersistentMonoSingleton<UIRoot>
	{
		public EventSystem eventSystem;
		public Camera uiCamera;
		public Canvas canvas;
		public CanvasScaler canvasScaler;

		/// <summary>
		/// 面板都放到这里
		/// </summary>
		public RectTransform CommomLayer;
		/// <summary>
		/// 该层提供框架的通用功能
		/// </summary>
		public RectTransform FrameworkLayer;
		/// <summary>
		/// 遮蔽所有交互
		/// </summary>
		[PropertySpace(SpaceAfter = 10)] public GameObject imgMask;
		public static bool interactable
		{
			get => !Instance.imgMask.activeInHierarchy;
			set => Instance.imgMask.SetActive(!value);
		}

		protected override void Awake()
		{
			ResMgr resMgr = ResMgr.Instance;
			UIMgr uiMgr = UIMgr.Instance;

			base.Awake();
		}

		[Button("TODO:检查预制体是否存在一些问题")]
		public void BuildCheck()
		{
			Debug.Log("TODO:检查预制体是否存在一些问题");
		}
	}
}

