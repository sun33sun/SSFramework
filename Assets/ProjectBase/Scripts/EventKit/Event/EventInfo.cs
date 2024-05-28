using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectBase
{
	/// <summary>
	/// 注销事件
	/// </summary>
	public interface IUnRegister
	{
		void UnRegister();
	}

	public struct SSUnRegister : IUnRegister
	{
		/// <summary>
		/// 委托对象
		/// </summary>
		private Action mOnUnRegsiter { get; set; }

		public SSUnRegister(Action onUnRegsiter)
		{
			mOnUnRegsiter = onUnRegsiter;
		}

		/// <summary>
		/// 资源释放
		/// </summary>
		public void UnRegister()
		{
			mOnUnRegsiter.Invoke();
			mOnUnRegsiter = null;
		}
	}

	/// <summary>
	/// 事件
	/// </summary>
	public interface IEventInfo
	{
	}

	public class EventInfo<T> : IEventInfo
	{
		private Action<T> mOnEvent = (T t) => { };

		public IUnRegister Register(Action<T> onEvent)
		{
			mOnEvent += onEvent;
			return new SSUnRegister(() => UnRegister(onEvent));
		}

		public void UnRegister(Action<T> onEvent)
		{
			mOnEvent -= onEvent;
		}

		public void Trigger(T info)
		{
			mOnEvent?.Invoke(info);
		}
	}

	public class EventInfo : IEventInfo
	{
		private Action mOnEvent = () => { };

		public IUnRegister Register(Action onEvent)
		{
			mOnEvent += onEvent;
			return new SSUnRegister(() => { UnRegister(onEvent); });
		}

		public void UnRegister(Action onEvent)
		{
			mOnEvent -= onEvent;
		}

		public void Trigger()
		{
			mOnEvent?.Invoke();
		}
	}
}