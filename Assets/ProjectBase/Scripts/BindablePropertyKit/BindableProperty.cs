

using System;

namespace ProjectBase
{
	public interface IBindableProperty<T> : IReadonlyBindableProperty<T>
	{
		new T Value { get; set; }
		void SetValueWithoutEvent(T newValue);
	}

	public interface IReadonlyBindableProperty<T> : IEventInfo
	{
		T Value { get; }

		IUnRegister RegisterWithInitValue(Action<T> action);
		void UnRegister(Action<T> onValueChanged);
		IUnRegister Register(Action<T> onValueChanged);
	}

	public class BindableProperty<T> : IBindableProperty<T>
	{
		public BindableProperty(T defaultValue = default) => mValue = defaultValue;

		protected T mValue;

		public static Func<T, T, bool> Comparer { get; set; } = (a, b) => a.Equals(b);

		public BindableProperty<T> WithComparer(Func<T, T, bool> comparer)
		{
			Comparer = comparer;
			return this;
		}

		public T Value
		{
			get => GetValue();
			set
			{
				if (value == null && mValue == null) return;
				if (value != null && Comparer(value, mValue)) return;

				SetValue(value);
				mOnValueChanged.Trigger(value);
			}
		}

		protected virtual void SetValue(T newValue) => mValue = newValue;

		protected virtual T GetValue() => mValue;

		public void SetValueWithoutEvent(T newValue) => mValue = newValue;

		private EventInfo<T> mOnValueChanged = new EventInfo<T>();

		public IUnRegister Register(Action<T> onValueChanged)
		{
			return mOnValueChanged.Register(onValueChanged);
		}

		public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
		{
			onValueChanged(mValue);
			return Register(onValueChanged);
		}

		public void UnRegister(Action<T> onValueChanged) => mOnValueChanged.UnRegister(onValueChanged);

		IUnRegister Register(Action onEvent)
		{
			return Register(Action);
			void Action(T _) => onEvent();
		}

		public override string ToString() => Value.ToString();
	}

	internal class ComparerAutoRegister
	{
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void AutoRegister()
		{
			BindableProperty<int>.Comparer = (a, b) => a == b;
			BindableProperty<float>.Comparer = (a, b) => a == b;
			BindableProperty<double>.Comparer = (a, b) => a == b;
			BindableProperty<string>.Comparer = (a, b) => a == b;
			BindableProperty<long>.Comparer = (a, b) => a == b;
			BindableProperty<UnityEngine.Vector2>.Comparer = (a, b) => a == b;
			BindableProperty<UnityEngine.Vector3>.Comparer = (a, b) => a == b;
			BindableProperty<UnityEngine.Vector4>.Comparer = (a, b) => a == b;
			BindableProperty<UnityEngine.Color>.Comparer = (a, b) => a == b;
			BindableProperty<UnityEngine.Color32>.Comparer =
				(a, b) => a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
			BindableProperty<UnityEngine.Bounds>.Comparer = (a, b) => a == b;
			BindableProperty<UnityEngine.Rect>.Comparer = (a, b) => a == b;
			BindableProperty<UnityEngine.Quaternion>.Comparer = (a, b) => a == b;
			BindableProperty<UnityEngine.Vector2Int>.Comparer = (a, b) => a == b;
			BindableProperty<UnityEngine.Vector3Int>.Comparer = (a, b) => a == b;
			BindableProperty<UnityEngine.BoundsInt>.Comparer = (a, b) => a == b;
			BindableProperty<UnityEngine.RangeInt>.Comparer = (a, b) => a.start == b.start && a.length == b.length;
			BindableProperty<UnityEngine.RectInt>.Comparer = (a, b) => a.Equals(b);
		}
	}
}

