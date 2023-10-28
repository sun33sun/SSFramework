using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using HighlightPlus;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ProjectBase.Extension
{
    public static class MainHelper
    {
        public static float HideTime = 0.5f;
        public static float ShowTime = 0.5f;

        #region UI扩展
        public static Func<bool> GetAnimatorEndFunc(this Toggle tog)
        {
            Animator animator = tog.animator;
            return () => animator.GetCurrentAnimatorStateInfo(0).IsName("Normal");
        }

        /// <summary>
        /// 执行异步函数过程中会屏蔽所有UI交互
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="invoke"></param>
        public static void AddAwaitAction(this Button btn, Func<UniTask> invoke)
        {
            CancellationToken token = btn.GetCancellationTokenOnDestroy();
            UnityAction asyncAction = async () =>
            {
                if (token.IsCancellationRequested) return;
                await btn.animator.GetAsyncAnimatorMoveTrigger().FirstAsync(btn.GetCancellationTokenOnDestroy());
                await invoke();
            };
            btn.onClick.AddListener(asyncAction);
        }

        /// <summary>
        /// 执行异步函数过程中会屏蔽所有UI交互
        /// </summary>
        /// <param name="tog"></param>
        /// <param name="invoke"></param>
        public static void AddAwaitAction(this Toggle tog, Func<bool, UniTask> invoke)
        {
            Func<bool> animFunc = tog.GetAnimatorEndFunc();
            CancellationToken token = tog.GetCancellationTokenOnDestroy();
            UnityAction<bool> asyncAction = null;
            //有group的情况下，会同时触发两个toggle，因此屏蔽由isOn的Toggle管理。
            if (tog.group != null && !tog.group.allowSwitchOff)
            {
                asyncAction = async isOn =>
                {
                    if (token.IsCancellationRequested) return;
                    await UniTask.WaitUntil(animFunc);
                    await invoke(isOn);
                };
            }
            else
            {
                asyncAction = async isOn =>
                {
                    if (token.IsCancellationRequested) return;
                    await UniTask.WaitUntil(animFunc);
                    await invoke(isOn);
                };
            }

            tog.onValueChanged.AddListener(asyncAction);
        }
        #endregion


        #region 3D物体扩展
        public static async UniTask HightlightClickAsync(this GameObject obj, CancellationToken cancellationToken)
        {
            HighlightEffect highlightEffect = obj.GetComponent<HighlightEffect>();
            if (highlightEffect == null)
                highlightEffect = obj.AddComponent<HighlightEffect>();
            highlightEffect.highlighted = true;
            highlightEffect.outlineColor = Color.red;

            await obj.GetAsyncPointerClickTrigger().FirstOrDefaultAsync(cancellationToken);
            highlightEffect.highlighted = false;
        }

        public static async UniTask HightlightClickAsync(this List<GameObject> objs,
            CancellationToken cancellationToken, Action<HighlightEffect> callBack = null)
        {
            List<HighlightEffect> objsHighlight = new List<HighlightEffect>();
            int count = objs.Count;
            for (int i = 0; i < objs.Count; i++)
            {
                GameObject obj = objs[i];
                HighlightEffect highlightEffect = obj.GetComponent<HighlightEffect>();
                if (highlightEffect == null)
                    highlightEffect = obj.AddComponent<HighlightEffect>();
                highlightEffect.highlighted = true;
                highlightEffect.outlineColor = Color.red;
                objsHighlight.Add(highlightEffect);
                obj.GetAsyncPointerClickTrigger().FirstOrDefaultAsync(d =>
                {
                    highlightEffect.highlighted = false;
                    count--;
                    callBack?.Invoke(highlightEffect);
                    return true;
                }, cancellationToken).Forget();
            }
            await UniTask.WaitUntil(() => count == 0, cancellationToken: cancellationToken);
        }
        #endregion
    }
}