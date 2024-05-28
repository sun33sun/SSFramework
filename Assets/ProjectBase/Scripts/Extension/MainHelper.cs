using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using HighlightPlus;
using UnityEngine;

namespace ProjectBase
{
    public static class MainHelper
    {
        public static float AnimTime = 0.5f;

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

        #region Component扩展
        public static T GetOrAddComponent<T>(this Component self) where T : Component
        {
            var comp = self.gameObject.GetComponent<T>();
            return comp ? comp : self.gameObject.AddComponent<T>();
        }

		public static T GetOrAddComponent<T>(this GameObject self) where T : Component
		{
			var comp = self.GetComponent<T>();
			return comp ? comp : self.AddComponent<T>();
		}
		#endregion
	}
}