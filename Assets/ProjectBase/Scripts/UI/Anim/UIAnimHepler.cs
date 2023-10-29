using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBase.UI
{
	public enum UIAnimType
	{
		FadeIn, FadeOut
	}
	public class UIAnimHepler
	{
		public async UniTask DoAnim(UIAnimType animType, UIController controller)
		{
			switch (animType)
			{
				case UIAnimType.FadeIn:
					await FadeIn(controller.group);
					break;
				case UIAnimType.FadeOut:
					await FadeOut(controller.group);
					break;
			}
		}

		public async UniTask FadeIn(CanvasGroup group)
		{
			await group.DOFade(1, UIManager.ShowTime).AsyncWaitForCompletion();
		}

		public async UniTask FadeOut(CanvasGroup group)
		{
			await group.DOFade(0, UIManager.HideTime).AsyncWaitForCompletion();
		}
	}
}