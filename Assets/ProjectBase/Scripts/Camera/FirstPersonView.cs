using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBase
{
	public class FirstPersonView : IPersonView
	{
		Transform m_transform;
		public Transform transform
		{
			get => m_transform; 
			set
			{
				m_transform = value;
				cam = m_transform.GetComponent<CinemachineVirtualCamera>();
				rig = m_transform.GetComponent<Rigidbody>();
			}
		}
		PersonViewField pvField;
		public PersonViewField PvField { get => pvField; set { pvField = value; } }
		CinemachineVirtualCamera cam;
		Rigidbody rig;

		public FirstPersonView(Transform transform,PersonViewField pvField)
		{
			this.transform = transform;
			this.pvField = pvField;
		}

		public void UpdateMovement(Vector2 dir)
		{
			rig.velocity = (transform.forward * dir.y + transform.right * dir.x) * PvField.moveSpeed;
		}

		public void Reset()
		{
		}

		public void OnMouseSliding(Vector2 slidingValue)
		{
			m_transform.rotation = Quaternion.AngleAxis(-slidingValue.y, m_transform.right) * Quaternion.AngleAxis(slidingValue.x, m_transform.up) * m_transform.rotation;

			Vector3 euler = m_transform.localEulerAngles;
			if (euler.z != 0)
			{
				euler.z = 0;
				m_transform.localEulerAngles = euler;
			}
		}

		public void OnEState()
		{
			Vector3 nowPos = m_transform.transform.localPosition;
			nowPos.y -= pvField.upSpeed * Time.fixedDeltaTime;
			m_transform.transform.localPosition = nowPos;
		}

		public void OnQState()
		{
			Vector3 nowPos = m_transform.transform.localPosition;
			nowPos.y += pvField.upSpeed * Time.fixedDeltaTime;
			m_transform.transform.localPosition = nowPos;
		}

		public void OnMouseScrollWheel(float distance)
		{
			cam.m_Lens.FieldOfView += distance * pvField.viewSpeed;
			if (cam.m_Lens.FieldOfView < 1)
				cam.m_Lens.FieldOfView = 1;
			else if (cam.m_Lens.FieldOfView > 90)
				cam.m_Lens.FieldOfView = 90;
		}
	}
}
