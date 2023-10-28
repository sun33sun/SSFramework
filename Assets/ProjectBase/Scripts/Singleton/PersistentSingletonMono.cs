using UnityEngine;

namespace ProjectBase
{
	public class PersistentSingletonMono<T> : MonoBehaviour where T : PersistentSingletonMono<T>
	{
		#region µ¥Àý
		private static T instance;
		public static T Instance { get { return instance; } }
		#endregion

		protected virtual void Awake()
		{
			if (instance == null)
			{
				instance = (T)this;
				
				DontDestroyOnLoad(instance);
			}
			else
			{
				Destroy(this);
			}
		}

		protected virtual void OnDestory()
		{
			if (instance == this)
				instance = null;
		}
	}
}
