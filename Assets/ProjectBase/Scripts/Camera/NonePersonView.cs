using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBase
{
	public class NonePersonView : IPersonView
	{
		Transform cam;
		public Transform transform { get => cam; set { cam = value; } }
		PersonViewField pvField;
		public PersonViewField PvField { get => pvField; set { pvField = value; } }

		public void UpdateMovement(Vector2 dir)
		{
			
		}

		public void Reset()
		{
		}

		public void OnMouseSliding(Vector2 slidingValue)
		{
		}

		public void OnEState()
		{
		}

		public void OnQState()
		{
		}

		public void OnMouseScrollWheel(float distance)
		{
			
		}
	}
}
