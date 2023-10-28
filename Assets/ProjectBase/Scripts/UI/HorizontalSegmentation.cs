using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBase
{
	/// <summary>
	/// 点击btnLeft，水平方向向左移动，看到右侧孩子
	/// 点击btnPre，水平方向向右移动，看到左侧孩子
	/// </summary>
	public class HorizontalSegmentation : MonoBehaviour
	{
		[Header("需要的UI组件")]
		public RectTransform Viewport;
		public RectTransform Content;
		[Header("左移变小，看到右孩子")]
		public Button btnLeft;
		[Header("右移变大，看到左孩子")]
		public Button btnRight;

		[Header("需要外部设置的参数")]
		public int showItemCount = 5;
		public float duration = 0.1f;

		float maxX;
		float minX;
		float moveSegment;
		Vector2 startPos;

		private void Awake()
		{
			Caculate();
		}

		public void Caculate()
		{
			btnRight.onClick.RemoveListener(RightMove);
			btnLeft.onClick.RemoveListener(LeftMove);

			moveSegment = (Content.GetChild(0).transform as RectTransform).sizeDelta.x +
						  Content.GetComponent<HorizontalLayoutGroup>().spacing;

			startPos = Content.anchoredPosition;
			float distance = (Content.childCount - showItemCount) * moveSegment / 2;
			//偶
			if ((Content.childCount - showItemCount) % 2 == 0)
			{
				//设置起始位置
				startPos.x = 0;
				Content.anchoredPosition = startPos;

				maxX = startPos.x + distance;
				minX = startPos.x - distance;
			}
			else //奇
			{
				//奇数情况下，起始位置要向右移动半个moveSegment，使（奇数除2向下取整的）中间居中显示
				startPos.x = moveSegment / 2;
				Content.anchoredPosition = startPos;

				maxX = startPos.x + distance - moveSegment;
				minX = startPos.x - distance;
			}

			//右移变大，看到左孩子
			btnRight.onClick.AddListener(RightMove);
			//左移变小，看到右孩子
			btnLeft.onClick.AddListener(LeftMove);
		}

		void RightMove()
		{
			float nowX = Content.anchoredPosition.x;
			if (nowX >= minX)
			{
				if (!btnLeft.gameObject.activeInHierarchy)
					btnLeft.gameObject.SetActive(true);

				float newX = nowX - moveSegment;
				if (newX <= minX)
					btnRight.gameObject.SetActive(false);

				Content.DOAnchorPosX(newX, duration);
			}
		}

		void LeftMove()
		{
			float nowX = Content.anchoredPosition.x;
			if (nowX <= maxX)
			{
				if (!btnRight.gameObject.activeInHierarchy)
					btnRight.gameObject.SetActive(true);

				float newX = nowX + moveSegment;
				if (newX >= maxX)
					btnLeft.gameObject.SetActive(false);

				Content.DOAnchorPosX(nowX + moveSegment, duration);
			}
		}

		public void ResetState()
		{
			Content.anchoredPosition = startPos;
			btnLeft.gameObject.SetActive(true);
			btnRight.gameObject.SetActive(true);
		}
	}
}