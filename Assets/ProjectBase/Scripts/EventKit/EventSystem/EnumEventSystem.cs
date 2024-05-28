using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectBase
{
	public class EnumEventSystem : Singleton<EnumEventSystem>
	{
		private Dictionary<int, IEventInfo> eventDic = new Dictionary<int, IEventInfo>();

		private EnumEventSystem() { }

		/// <summary>
		/// 添加事件监听
		/// </summary>
		/// <param name="key">事件的名字</param>
		/// <param name="onEvent">准备用来处理事件 的委托函数</param>
		public IUnRegister Register<K, T>(K key, Action<T> onEvent) where K : IConvertible
		{
			int value = key.ToInt32(null);
			//有没有对应的事件监听
			//有的情况
			if (eventDic.ContainsKey(value))
			{
				return (eventDic[value] as EventInfo<T>).Register(onEvent);
			}
			//没有的情况
			else
			{
				EventInfo<T> eventInfo = new EventInfo<T>();
				eventDic.Add(value, eventInfo);
				return eventInfo.Register(onEvent);
			}
		}

		/// <summary>
		/// 添加事件监听
		/// </summary>
		/// <param name="key">事件的名字</param>
		/// <param name="onEvent">准备用来处理事件 的委托函数</param>
		public IUnRegister Register<K>(K key, Action onEvent) where K : IConvertible
		{
			//有没有对应的事件监听
			int value = key.ToInt32(null);
			//有的情况
			if (eventDic.ContainsKey(value))
			{
				return (eventDic[value] as EventInfo).Register(onEvent);
			}
			//没有的情况
			else
			{
				EventInfo eventInfo = new EventInfo();
				eventDic.Add(value, eventInfo);
				return eventInfo.Register(onEvent);
			}
		}

		/// <summary>
		/// 移除对应的事件监听
		/// </summary>
		/// <param name="key">事件的名字</param>
		/// <param name="onEvent">对应之前添加的委托函数</param>
		public void UnRegister<K, T>(K key, Action<T> onEvent) where K : IConvertible
		{
			int value = key.ToInt32(null);
			if (eventDic.ContainsKey(value))
				(eventDic[value] as EventInfo<T>).Register(onEvent);
		}

		/// <summary>
		/// 移除不需要参数的事件
		/// </summary>
		/// <param name="key"></param>
		/// <param name="onEvent"></param>
		public void UnRegister<K>(K key, Action onEvent) where K : IConvertible
		{
			int value = key.ToInt32(null);
			if (eventDic.ContainsKey(value))
				(eventDic[value] as EventInfo).UnRegister(onEvent);
		}

		/// <summary>
		/// 事件触发
		/// </summary>
		/// <param name="key">哪一个名字的事件触发了</param>
		public void Send<K, T>(K key, T e) where K : IConvertible
		{
			int value = key.ToInt32(null);
			//有没有对应的事件监听
			//有的情况
			if (eventDic.ContainsKey(value))
			{
				(eventDic[value] as EventInfo<T>).Trigger(e);
			}
		}

		/// <summary>
		/// 事件触发（不需要参数的）
		/// </summary>
		/// <param name="key"></param>
		public void Send<K>(K key) where K : IConvertible
		{
			int value = key.ToInt32(null);
			//有没有对应的事件监听
			//有的情况
			if (eventDic.ContainsKey(value))
			{
				(eventDic[value] as EventInfo).Trigger();
			}
		}

		/// <summary>
		/// 清空事件中心
		/// 主要用在 场景切换时
		/// </summary>
		public void Clear()
		{
			eventDic.Clear();
		}

	}

}
