using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBase
{
	public class MeshHelper : Singleton<MeshHelper>
	{
		public const string MainTex = "_MainTex";
		public const string MainColor = "_MainColor";

		private MaterialPropertyBlock mpb;

		private MeshHelper()
		{
			mpb = new MaterialPropertyBlock();
		}

		public void SetColor(MeshRenderer renderer, Color color, string name = MainColor)
		{
			mpb.Clear();
			mpb.SetColor(name, color);
			renderer.SetPropertyBlock(mpb);
		}

		public void SetTexture(MeshRenderer renderer, Texture texture, string name = MainTex)
		{
			mpb.Clear();
			mpb.SetTexture(name, texture);
			renderer.SetPropertyBlock(mpb);
		}
	}

}

