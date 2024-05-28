#if UNITY_EDITOR
using ProjectBase.CodeGenKit;
using System.IO;
using System.Text;
using UnityEngine;

namespace ProjectBase
{
	public class UIElementBuilder : BaseBuilder
	{
		public UIElementBuilder(ITemplate template) : base(template)
		{
		}

		protected override void WriteCS(string className, BaseBind baseBind)
		{
			BaseController parent = FindParent(baseBind);

			string classPath = $"{Application.dataPath}/{template.CodePath}/{parent.name}/{className}{FileSuffix.classSuffix}";

			if (!File.Exists(classPath))
			{
				string classStr = template.ClassTemplate;
				classStr = classStr.Replace(CodeTemplateMarker.NAMESPACE, template.NameSpace);
				classStr = classStr.Replace(CodeTemplateMarker.CLASSNAME, className);
				CodeGenHelper.Write(classPath, classStr);
			}
		}

		protected override void WriteDesigner(string className, BaseBind baseBind)
		{
			BaseController parent = FindParent(baseBind);

			string designerPath = $"{Application.dataPath}/{template.CodePath}/{parent.name}/{className}{FileSuffix.designerSuffix}";

			if (File.Exists(designerPath))
				File.Delete(designerPath);

			//成员变量字符串
			StringBuilder sbMembers = new StringBuilder();
			foreach (var otherBind in baseBind.infos)
			{
				string fieldLine = $"\t[HideInInspector, SerializeField] public {otherBind.Object.GetType().FullName} {otherBind.MemberName} = null;\r\n\t";
				sbMembers.Append(fieldLine);
			}

			string designerStr = template.DesignerTemplate;
			designerStr = designerStr.Replace(CodeTemplateMarker.NAMESPACE, template.NameSpace);
			designerStr = designerStr.Replace(CodeTemplateMarker.CLASSNAME, className);
			designerStr = designerStr.Replace(CodeTemplateMarker.MEMBER, sbMembers.ToString());
			CodeGenHelper.Write(designerPath, designerStr);
		}

		public override void FindReference(BaseBind baseBind)
		{
			BaseController parent = FindParent(baseBind);
			parent.GetComponent<BaseBind>();

			base.FindReference(baseBind);
		}

		public BaseController FindParent(BaseBind baseBind)
		{
			BaseController parent = null;

			BaseController[] baseControllers = baseBind.GetComponentsInParent<BaseController>(true);


			if (baseControllers != null)
			{
				if(baseBind.GetComponent<BaseController>() != null && baseControllers.Length >= 2)
					parent = baseControllers[1];
				else if(baseControllers.Length == 1)
					parent = baseControllers[0];
			}

			return parent;
		}
	}
}

#endif