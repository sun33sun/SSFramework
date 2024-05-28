using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace ProjectBase
{
	public class StringEventSystem : Singleton<StringEventSystem>
	{
		private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

		/// <summary>
		/// 添加事件监听
		/// </summary>
		/// <param name="key">事件的名字</param>
		/// <param name="onEvent">准备用来处理事件 的委托函数</param>
		public IUnRegister Register<T>(string key, Action<T> onEvent)
		{
			//有没有对应的事件监听
			//有的情况
			if (eventDic.ContainsKey(key))
			{
				return (eventDic[key] as EventInfo<T>).Register(onEvent);
			}
			//没有的情况
			else
			{
				EventInfo<T> eventInfo = new EventInfo<T>();
				eventDic.Add(key, eventInfo);
				return eventInfo.Register(onEvent);
			}
		}

		/// <summary>
		/// 监听不需要参数传递的事件
		/// </summary>
		/// <param name="key"></param>
		/// <param name="onEvent"></param>
		public IUnRegister Register(string key, Action onEvent)
		{
			//有没有对应的事件监听
			//有的情况
			if (eventDic.ContainsKey(key))
			{
				return (eventDic[key] as EventInfo).Register(onEvent);
			}
			//没有的情况
			else
			{
				EventInfo eventInfo = new EventInfo();
				eventDic.Add(key, eventInfo);
				return eventInfo.Register(onEvent);
			}
		}

		/// <summary>
		/// 移除对应的事件监听
		/// </summary>
		/// <param name="key">事件的名字</param>
		/// <param name="onEvent">对应之前添加的委托函数</param>
		public void UnRegister<T>(string key, Action<T> onEvent)
		{
			if (eventDic.ContainsKey(key))
				(eventDic[key] as EventInfo<T>).Register(onEvent);
		}

		/// <summary>
		/// 移除不需要参数的事件
		/// </summary>
		/// <param name="key"></param>
		/// <param name="onEvent"></param>
		public void UnRegister(string key, Action onEvent)
		{
			if (eventDic.ContainsKey(key))
				(eventDic[key] as EventInfo).UnRegister(onEvent);
		}

		/// <summary>
		/// 事件触发
		/// </summary>
		/// <param name="key">哪一个名字的事件触发了</param>
		public void Send<T>(string key, T e)
		{
			//有没有对应的事件监听
			//有的情况
			if (eventDic.ContainsKey(key))
			{
				(eventDic[key] as EventInfo<T>).Trigger(e);
			}
		}

		/// <summary>
		/// 事件触发（不需要参数的）
		/// </summary>
		/// <param name="key"></param>
		public void Send(string key)
		{
			//有没有对应的事件监听
			//有的情况
			if (eventDic.ContainsKey(key))
			{
				(eventDic[key] as EventInfo).Trigger();
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