#if UNITY_EDITOR
namespace ProjectBase.CodeGenKit
{
	public class BuilderManager
	{
		BaseBuilder builder = null;

		//单例
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


		#region 第一阶段，生成代码

		/// <summary>
		/// 阶段一：生成代码
		/// </summary>
		/// <param name="baseBind"></param>
		public void GenerateCode(BaseBind baseBind)
		{
			PatternMatch(baseBind);

			builder.GenerateCode(baseBind);
		}
		#endregion

		/// <summary>
		/// 阶段二：查找引用
		/// </summary>
		/// <param name="baseBind"></param>
		public void FindReference(BaseBind baseBind)
		{
			PatternMatch(baseBind);

			builder.FindReference(baseBind);
		}

		/// <summary>
		/// 阶段三：生成预制体
		/// </summary>
		/// <param name="baseBind"></param>
		public void SaveAsPrefab(BaseBind baseBind)
		{
			PatternMatch(baseBind);

			builder.SaveAsPreab(baseBind);
		}

		/// <summary>
		/// TODO:阶段四，检查生成的“代码或预制体”是否被删除。
		/// 如果被删除，要修改枚举ControllerName和数据管理器DataMgr.Designer
		/// </summary>
		public void CheckError()
		{

		}
	}
}
#endif