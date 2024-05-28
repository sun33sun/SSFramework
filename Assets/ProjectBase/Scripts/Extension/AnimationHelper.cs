using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ProjectBase.Extension.AnimExtension
{
	public static class AnimationHelper
	{
		public static async UniTask ShowAwait(this Component component)
		{
			component.gameObject.SetActive(true);
			await (component.transform as RectTransform).DOScale(Vector2.one, MainHelper.AnimTime)
				.AsyncWaitForCompletion();
		}

		public static async UniTask HideAwait(this Component component)
		{
			await component.transform.DOScale(Vector2.zero, MainHelper.AnimTime).AsyncWaitForCompletion();
			component.gameObject.SetActive(false);
		}

		public static void ShowSync(this Component component)
		{
			component.gameObject.SetActive(true);
			(component.transform as RectTransform).localScale = Vector2.one;
		}

		public static void HideSync(this Component component)
		{
			(component.transform as RectTransform).localScale = Vector2.zero;
			component.gameObject.SetActive(false);
		}

		public static void ShowAsync(this Component component)
		{
			component.gameObject.SetActive(true);
			(component.transform as RectTransform).DOScale(Vector2.one, MainHelper.AnimTime);
		}

		public static void HideAsync(this Component component)
		{
			(component.transform as RectTransform).DOScale(Vector2.zero, MainHelper.AnimTime)
				.OnComplete(() => component.gameObject.SetActive(false));
		}
	}
}