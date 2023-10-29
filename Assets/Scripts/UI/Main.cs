using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectBase;
using Cysharp.Threading.Tasks;

namespace ProjectBase.UI
{
	public class Main : PersistentSingletonMono<Main>
	{
		private void Start()
		{
			UIManager.Preload();
		}

		private void Update()
		{
			if(Input.GetKeyUp(KeyCode.Escape))
			{
				UniTask.Void(async () =>
				{
					TestPanel testPanel = await UIManager.OpenAsync<TestPanel>();
				});
			}
		}
	}
}

