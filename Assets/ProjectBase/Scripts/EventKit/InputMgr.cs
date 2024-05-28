using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectBase
{
    /// <summary>
    /// 1.Input类
    /// 2.事件中心模块
    /// 3.公共Mono模块的使用
    /// </summary>
    public class InputMgr : Singleton<InputMgr>
    {

		private bool isStartUpdate = false;
		private bool isStartFixedUpdate = false;

        /// <summary>
        /// 构造函数中 添加Updata监听
        /// </summary>
        public InputMgr()
        {
			MonoMgr.Instance.AddUpdateListener(MyUpdate);
			MonoMgr.Instance.AddFixedUpdateListener(MyFixedUpdate);
            // MonoMgr.GetInstance.AddLateUpdateListener(MyLateUpdate);
		}

        /// <summary>
        /// 是否开启或关闭 我的输入检测
        /// </summary>
        public void ChangerInput(bool isOpen)
        {
			isStartUpdate = isOpen;
			isStartFixedUpdate = isOpen;
        }

        /// <summary>
        /// 用来检测按键抬起按下 分发事件的
        /// </summary>
        /// <param name="key"></param>
        private void CheckKeyCode(KeyCode key)
        {
            //事件中心模块 分发按下抬起事件
            if (Input.GetKeyDown(key))
                StringEventSystem.Instance.Send(key + "按下");
            if(Input.GetKey(key))
                StringEventSystem.Instance.Send(key + "保持");
            //事件中心模块 分发按下抬起事件
            if (Input.GetKeyUp(key))
                StringEventSystem.Instance.Send(key + "抬起");
        }

		private void MyUpdate()
		{
			//没有开启输入检测 就不去检测 直接return
			if (!isStartUpdate)
				return;

            if (Input.GetMouseButtonDown(0))
			{
                StringEventSystem.Instance.Send("鼠标左键按下");
            }
            if (Input.GetMouseButtonUp(0))
                StringEventSystem.Instance.Send("鼠标左键抬起");
            if (Input.GetMouseButtonDown(1))
                StringEventSystem.Instance.Send("鼠标右键按下");
            if (Input.GetMouseButtonUp(1))
                StringEventSystem.Instance.Send("鼠标右键抬起");

            StringEventSystem.Instance.Send<Vector2>("鼠标滑动", new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));

            StringEventSystem.Instance.Send<float>("鼠标滚轮", Input.GetAxis("Mouse ScrollWheel"));
        }

        private void MyFixedUpdate()
		{
            if (!isStartFixedUpdate)
                return;

            CheckKeyCode(KeyCode.Space);
            CheckKeyCode(KeyCode.LeftControl);
            StringEventSystem.Instance.Send("移动方向", new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        }

        private void MyLateUpdate()
        {
        }
    }
}

