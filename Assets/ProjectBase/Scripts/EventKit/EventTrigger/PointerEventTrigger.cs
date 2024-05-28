using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectBase
{
	public class PointerEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerExitHandler
	{
		public readonly EventInfo<PointerEventData> mOnPointerEnterEvent = new EventInfo<PointerEventData>();

		public readonly EventInfo<PointerEventData> mOnPointerDownEvent = new EventInfo<PointerEventData>();

		public readonly EventInfo<PointerEventData> mOnPointerClickEvent = new EventInfo<PointerEventData>();

		public readonly EventInfo<PointerEventData> mOnPointerUpEvent = new EventInfo<PointerEventData>();

		public readonly EventInfo<PointerEventData> mOnPointerExitEvent = new EventInfo<PointerEventData>();

		/// <summary>
		/// 触发鼠标进入
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerEnter(PointerEventData eventData)
		{
			mOnPointerEnterEvent.Trigger(eventData);
		}

		/// <summary>
		/// 触发鼠标按下
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerDown(PointerEventData eventData)
		{
			mOnPointerDownEvent.Trigger(eventData);
		}

		/// <summary>
		/// 触发鼠标点击
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerClick(PointerEventData eventData)
		{
			mOnPointerClickEvent.Trigger(eventData);
		}

		/// <summary>
		/// 触发鼠标抬起
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerUp(PointerEventData eventData)
		{
			mOnPointerUpEvent.Trigger(eventData);
		}

		/// <summary>
		/// 触发鼠标离开
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerExit(PointerEventData eventData)
		{
			mOnPointerExitEvent.Trigger(eventData);
		}

		public IUnRegister RegisterEnterEvent(Action<PointerEventData> onPointerEvent)
		{
			return mOnPointerClickEvent.Register(onPointerEvent);
		}

		public IUnRegister RegisterDownEvent(Action<PointerEventData> onPointerEvent)
		{
			return mOnPointerDownEvent.Register(onPointerEvent);
		}

		public IUnRegister RegisterClickEvent(Action<PointerEventData> onPointerEvent)
		{
			return mOnPointerClickEvent.Register(onPointerEvent);
		}

		public IUnRegister RegisterUpEvent(Action<PointerEventData> onPointerEvent)
		{
			return mOnPointerUpEvent.Register(onPointerEvent);
		}

		public IUnRegister RegisterExitEvent(Action<PointerEventData> onPointerEvent)
		{
			return mOnPointerExitEvent.Register(onPointerEvent);
		}
	}

	public static class PointerEventTriggerExtension
	{
		public static IUnRegister RegisterPointerEnter<T>(this T self, Action<PointerEventData> onPointerEvent) where T : Component
		{
			return self.GetOrAddComponent<PointerEventTrigger>().RegisterEnterEvent(onPointerEvent);
		}

		public static IUnRegister RegisterPointerDown<T>(this T self, Action<PointerEventData> onPointerEvent) where T : Component
		{
			return self.GetOrAddComponent<PointerEventTrigger>().RegisterDownEvent(onPointerEvent);
		}

		public static IUnRegister RegisterPointerClick<T>(this T self, Action<PointerEventData> onPointerEvent) where T : Component
		{
			return self.GetOrAddComponent<PointerEventTrigger>().RegisterClickEvent(onPointerEvent);
		}

		public static IUnRegister RegisterPointerUp<T>(this T self, Action<PointerEventData> onPointerEvent) where T : Component
		{
			return self.GetOrAddComponent<PointerEventTrigger>().RegisterUpEvent(onPointerEvent);
		}

		public static IUnRegister RegisterPointerExit<T>(this T self, Action<PointerEventData> onPointerEvent) where T : Component
		{
			return self.GetOrAddComponent<PointerEventTrigger>().RegisterExitEvent(onPointerEvent);
		}
	}
}

