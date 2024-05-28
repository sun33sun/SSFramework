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
		/// ��嶼�ŵ�����
		/// </summary>
		public RectTransform CommomLayer;
		/// <summary>
		/// �ò��ṩ��ܵ�ͨ�ù���
		/// </summary>
		public RectTransform FrameworkLayer;
		/// <summary>
		/// �ڱ����н���
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

		[Button("TODO:���Ԥ�����Ƿ����һЩ����")]
		public void BuildCheck()
		{
			Debug.Log("TODO:���Ԥ�����Ƿ����һЩ����");
		}
	}
}

