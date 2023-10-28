using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;
using ProjectBase;
using UnityEngine.EventSystems;

namespace ProjectBase
{
    public class CameraManager : SingletonMono<CameraManager>
    {
        List<IPersonView> personViews = new List<IPersonView>();
        PersonViewField pvField;
        [SerializeField] bool isEnable = false;
        public PersonViewType pvType = PersonViewType.FirstPerson;


        [SerializeField] Camera mainC;
        [SerializeField] CinemachineVirtualCamera noneC = null;
        [SerializeField] CinemachineVirtualCamera firstC = null;
        [SerializeField] CinemachineVirtualCamera followC = null;
        [SerializeField] CinemachineVirtualCamera thirdC = null;

        //��������ĸ���
        Rigidbody roamRig = null;

        Vector3 originPos;
        Vector3 originAngle;
        float originFieldOfView;
        Vector3 nowPos;

        public bool IsEnable
        {
            get { return isEnable; }
            set
            {
                if (value)
                    roamRig.constraints = RigidbodyConstraints.FreezeRotation;
                else
                    roamRig.constraints = RigidbodyConstraints.FreezeAll;
                isEnable = value;
            }
        }

        bool isRotate = false;

        private void Start()
        {
            //�������
            roamRig = firstC.GetComponent<Rigidbody>();
            //��¼��ʼλ��
            originPos = firstC.transform.position;
            originAngle = firstC.transform.rotation.eulerAngles;
            originFieldOfView = firstC.m_Lens.FieldOfView;

            #region �ƶ�����ת

            //�ƶ�
            InputMgr.GetInstance().ChangerInput(true);
            //�����ƶ�
            EventCenter.GetInstance().AddEventListener(KeyCode.LeftControl + "����", OnEState);
            // EventCenter.GetInstance().AddEventListener<float>("������", OnMouseScrollWheel);
            //��ת
            EventCenter.GetInstance().AddEventListener("����Ҽ�����", OnMouseRightDown);
            EventCenter.GetInstance().AddEventListener("����Ҽ�̧��", OnMouseRightUp);
            EventCenter.GetInstance().AddEventListener<Vector2>("��껬��", OnMouseSliding);
            EventCenter.GetInstance().AddEventListener(KeyCode.Space + "����", OnQState);
            EventCenter.GetInstance().AddEventListener<Vector2>("�ƶ�����", UpdateMovement);

            #endregion

            #region ��ʼ������

            pvField.moveSpeed = 3;
            pvField.upSpeed = 2;
            pvField.rotateSpeed = 3;
            pvField.viewSpeed = 10;
            personViews.Add(new NonePersonView());
            personViews.Add(new FirstPersonView(firstC.transform, pvField));
            personViews.Add(new ThirdPersonView(thirdC, pvField));

            #endregion
        }

        #region �ƶ��������ƶ�����ת��������Ұ

        void UpdateMovement(Vector2 dir)
        {
            if(isEnable)
                personViews[(int)pvType].UpdateMovement(dir);
            
        }

        private void OnEState()
        {
            if (!IsEnable)
                return;
            personViews[(int)pvType].OnEState();
        }

        private void OnQState()
        {
            if (!IsEnable)
                return;
            personViews[(int)pvType].OnQState();
        }

        private void OnMouseRightDown()
        {
            if (!IsEnable)
                return;
            isRotate = true;
        }

        private void OnMouseRightUp()
        {
            if (!IsEnable)
                return;
            isRotate = false;
        }

        private void OnMouseSliding(Vector2 slidingValue)
        {
            if (!isRotate || !IsEnable)
                return;
            
            personViews[(int)pvType].OnMouseSliding(slidingValue);
        }

        private void OnMouseScrollWheel(float distance)
        {
            if (!IsEnable)
                return;
            personViews[(int)pvType].OnMouseScrollWheel(distance);
        }

        #endregion

        #region �����˳�

        public void ThirPersonView(PlayerController playerController)
        {
            pvType = PersonViewType.ThirdPerson;
            thirdC.Priority = 20;
            noneC.Priority = 10;
            firstC.Priority = 10;
            followC.Priority = 10;
            (personViews[(int)pvType] as ThirdPersonView).Player = playerController;
        }

        public void NonePersonView()
        {
            pvType = PersonViewType.None;
            noneC.Priority = 20;
            thirdC.Priority = 10;
            firstC.Priority = 10;
            followC.Priority = 10;
        }

        public void FirstPersonView()
        {
            switch (pvType)
            {
                case PersonViewType.ThirdPerson:
                    firstC.transform.forward = thirdC.Follow.forward;
                    firstC.transform.position = thirdC.Follow.position + Vector3.up * 2;
                    break;
                case PersonViewType.None:
                    firstC.transform.forward = noneC.transform.forward;
                    firstC.transform.position = noneC.transform.position;
                    break;                    
            }
            pvType = PersonViewType.FirstPerson;
            firstC.Priority = 20;
            thirdC.Priority = 10;
            noneC.Priority = 10;
            followC.Priority = 10;
        }
        #endregion

        private void OnDestroy()
        {
            //�����ƶ�
            EventCenter.GetInstance().RemoveEventListener(KeyCode.LeftControl + "����", OnEState);
            // EventCenter.GetInstance().RemoveEventListener<float>("������", OnMouseScrollWheel);
            //��ת
            EventCenter.GetInstance().RemoveEventListener("����Ҽ�����", OnMouseRightDown);
            EventCenter.GetInstance().RemoveEventListener("����Ҽ�̧��", OnMouseRightUp);
            EventCenter.GetInstance().RemoveEventListener<Vector2>("��껬��", OnMouseSliding);
            EventCenter.GetInstance().RemoveEventListener(KeyCode.Space + "����", OnQState);
            EventCenter.GetInstance().RemoveEventListener<Vector2>("�ƶ�����", UpdateMovement);
        }
    }
}