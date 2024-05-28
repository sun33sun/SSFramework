using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBase
{
	public class UnRegisterTrigger : MonoBehaviour
	{
		public readonly HashSet<IUnRegister> onDisableEvent = new HashSet<IUnRegister>();
		public readonly HashSet<IUnRegister> onDestroyEvent = new HashSet<IUnRegister>();

		private void OnDisable()
		{
			foreach (var unRegister in onDisableEvent)
			{
				unRegister.UnRegister();
			}

			onDisableEvent.Clear();
		}

		private void OnDestroy()
		{
			foreach (var unRegister in onDestroyEvent)
			{
				unRegister.UnRegister();
			}

			onDestroyEvent.Clear();
		}
	}

	public static class UnRegisterExtension
	{
		/// <summary>
		/// 当Destroy时，自动注销
		/// </summary>
		/// <param name="unRegister"></param>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static IUnRegister UnRegisterOnDestroy(this IUnRegister unRegister, GameObject gameObject)
		{
			var trigger = gameObject.GetOrAddComponent<UnRegisterTrigger>();

			trigger.onDestroyEvent.Add(unRegister);

			return unRegister;
		}

		/// <summary>
		/// 当Destroy时，自动注销
		/// </summary>
		/// <param name="unRegister"></param>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static IUnRegister UnRegisterOnDestroy<T>(this IUnRegister unRegister, T component)
			where T : Component
		{
			return unRegister.UnRegisterOnDestroy(component.gameObject);
		}

		/// <summary>
		/// 当Disable时自动注销
		/// </summary>
		/// <param name="unRegister"></param>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static IUnRegister UnRegisterOnDisable(this IUnRegister unRegister, GameObject gameObject)
		{
			var trigger = gameObject.GetOrAddComponent<UnRegisterTrigger>();

			trigger.onDisableEvent.Add(unRegister);

			return unRegister.UnRegisterOnDestroy(gameObject);
		}

		/// <summary>
		/// 当Disable时自动注销
		/// </summary>
		/// <param name="unRegister"></param>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static IUnRegister UnRegisterOnDisable<T>(this IUnRegister unRegister, T component)
			where T : Component
		{
			return unRegister.UnRegisterOnDisable(component.gameObject);
		}
	}
}
