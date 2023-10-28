using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBase
{
    public class ThirdPersonView : IPersonView
    {
        public Transform transform
        {
            get => cam.transform;
            set
            {
                cam = value.GetComponent<CinemachineVirtualCamera>();
                if (cam.Follow != null)
                {
                    _player = cam.Follow.GetComponent<PlayerController>();
                    _player._Camera = cam.transform;
                }
            }
        }

        public PersonViewField pvField;

        public PersonViewField PvField
        {
            get => pvField;
            set { pvField = value; }
        }

        CinemachineVirtualCamera cam;
        private PlayerController _player = null;

        public PlayerController Player
        {
            set
            {
                _player = value;
                cam.Follow = _player.transform;
                cam.transform.position = cam.Follow.position;
                _player._Camera = cam.transform;
            }
        }

        public ThirdPersonView(CinemachineVirtualCamera cam, PersonViewField pvField)
        {
            this.cam = cam;
            this.pvField = pvField;
        }

        public void UpdateMovement(Vector2 dir)
        {
            if (_player != null)
                _player.UpdateMovement(dir);
        }

        public void Reset()
        {
        }

        public void OnMouseSliding(Vector2 slidingValue)
        {
            cam.transform.rotation = Quaternion.AngleAxis(-slidingValue.y, cam.transform.right) *
                                     Quaternion.AngleAxis(slidingValue.x, cam.transform.up) * cam.transform.rotation;
            Vector3 euler = cam.transform.transform.localEulerAngles;
            if (euler.z != 0)
            {
                euler.z = 0;
                cam.transform.transform.localEulerAngles = euler;
            }
        }

        public void OnEState()
        {
        }

        public void OnQState()
        {
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