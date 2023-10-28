using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBase
{
	public struct PersonViewField
	{
		public float moveSpeed;
		public float upSpeed;
		public float rotateSpeed;
		public float viewSpeed;
	}
	public enum PersonViewType
	{
		None, FirstPerson, ThirdPerson
	}
	interface IPersonView
	{
		Transform transform { get; set; }
		PersonViewField PvField { get; set; }
		void UpdateMovement(Vector2 dir);
		void OnMouseSliding(Vector2 slidingValue);
		void OnQState();
		void OnEState();
		void OnMouseScrollWheel(float distance);
		void Reset();
	}
}
