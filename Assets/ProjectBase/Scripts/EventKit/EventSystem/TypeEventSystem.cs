using System.Collections.Generic;
using System;

namespace ProjectBase
{
	public class TypeEventSystem
	{
		private Dictionary<Type, IEventInfo> mEventDic = new Dictionary<Type, IEventInfo>();

		public void Send<T>() where T : new()
		{
			var e = new T();
			Send<T>(e);
		}

		public void Send<T>(T e)
		{
			var type = typeof(T);
			IEventInfo registrations;

			if (mEventDic.TryGetValue(type, out registrations))
			{
				(registrations as EventInfo<T>).Trigger(e);
			}
		}

		public IUnRegister Register<T>(Action<T> onEvent)
		{
			var type = typeof(T);
			IEventInfo registrations;

			if (mEventDic.TryGetValue(type, out registrations))
			{

			}
			else
			{
				registrations = new EventInfo<T>();
				mEventDic.Add(type, registrations);
			}

			(registrations as EventInfo<T>).Register(onEvent);

			return new TypeEventSystemUnRegister<T>()
			{
				OnEvent = onEvent,
				TypeEventSystem = this
			};
		}

		public void UnRegister<T>(Action<T> onEvent)
		{
			var type = typeof(T);
			IEventInfo registrations;

			if (mEventDic.TryGetValue(type, out registrations))
			{
				(registrations as EventInfo<T>).UnRegister(onEvent);
			}
		}
	}

	public struct TypeEventSystemUnRegister<T> : IUnRegister
	{
		public TypeEventSystem TypeEventSystem;
		public Action<T> OnEvent;

		public void UnRegister()
		{
			TypeEventSystem.UnRegister<T>(OnEvent);

			TypeEventSystem = null;

			OnEvent = null;
		}
	}
}
