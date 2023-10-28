using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ProjectBase.Extension.AnimExtension
{
    public static class AnimationHelper
    {
        public static async UniTask ShowAsync(this Component component)
        {
            component.gameObject.SetActive(true);
            await (component.transform as RectTransform).DOAnchorPosY(0, MainHelper.ShowTime)
                .AsyncWaitForCompletion();
        }

        public static async UniTask HideAsync(this Component component)
        {
            (component.transform as RectTransform).DOAnchorPos(new Vector2(0, Screen.height),
                MainHelper.HideTime);
            await component.transform.DOLocalMoveY(1080, MainHelper.HideTime).AsyncWaitForCompletion();
            component.gameObject.SetActive(false);
        }

        public static void ShowSync(this Component component)
        {
            component.gameObject.SetActive(true);
            (component.transform as RectTransform).anchoredPosition = Vector2.zero;
        }

        public static void HideSync(this Component component)
        {
            (component.transform as RectTransform).anchoredPosition = new Vector2(0, Screen.height);
            component.gameObject.SetActive(false);
        }
    }
}