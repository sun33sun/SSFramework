namespace ProjectBase.CodeGenKit
{
	public class BuilderManager
	{
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

		BaseBuilder builder = null;

		void PatternMatch(BaseBind baseBind) 
		{
			if (baseBind is UIBind uiBind)
				builder = new UIBuilder(new UITemplate());
			else if (baseBind is GameBind gameBind)
				builder = new GameBuilder(new GameTemplate());
		}


		public void GenerateCode(BaseBind baseBind)
		{
			PatternMatch(baseBind);
		
			builder.GenerateCode(baseBind);
		}

		public void FindReference(BaseBind baseBind)
		{
			PatternMatch(baseBind);

			builder.FindReference(baseBind);
		}

		public void SaveAsPrefab(BaseBind baseBind)
		{
			PatternMatch(baseBind);

			builder.SaveAsPreab(baseBind);
		}
	}
}