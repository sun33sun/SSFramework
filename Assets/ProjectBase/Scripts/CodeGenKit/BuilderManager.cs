#if UNITY_EDITOR
namespace ProjectBase.CodeGenKit
{
	public class BuilderManager
	{
		BaseBuilder builder = null;

		//����
		static BuilderManager instance = null;
		public static BuilderManager Instance
		{
			get
			{
				if (instance == null)
					instance = new BuilderManager();
				return instance;
			}
		}
		private BuilderManager()
		{

		}

		void PatternMatch(BaseBind baseBind)
		{
			if (baseBind is PanelBind uiBind)
				builder = new PanelBuilder(new PanelTemplate());
			else if (baseBind is ViewBind gameBind)
				builder = new ViewBuilder(new ViewTemplate());
			else if (baseBind is UIElementBind uiElementBind)
				builder = new UIElementBuilder(new UIElementTemplate());
		}


		#region ��һ�׶Σ����ɴ���

		/// <summary>
		/// �׶�һ�����ɴ���
		/// </summary>
		/// <param name="baseBind"></param>
		public void GenerateCode(BaseBind baseBind)
		{
			PatternMatch(baseBind);

			builder.GenerateCode(baseBind);
		}
		#endregion

		/// <summary>
		/// �׶ζ�����������
		/// </summary>
		/// <param name="baseBind"></param>
		public void FindReference(BaseBind baseBind)
		{
			PatternMatch(baseBind);

			builder.FindReference(baseBind);
		}

		/// <summary>
		/// �׶���������Ԥ����
		/// </summary>
		/// <param name="baseBind"></param>
		public void SaveAsPrefab(BaseBind baseBind)
		{
			PatternMatch(baseBind);

			builder.SaveAsPreab(baseBind);
		}

		/// <summary>
		/// TODO:�׶��ģ�������ɵġ������Ԥ���塱�Ƿ�ɾ����
		/// �����ɾ����Ҫ�޸�ö��ControllerName�����ݹ�����DataMgr.Designer
		/// </summary>
		public void CheckError()
		{

		}
	}
}
#endif