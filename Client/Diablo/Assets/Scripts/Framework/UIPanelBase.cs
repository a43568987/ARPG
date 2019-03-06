using UnityEngine;
using System;

namespace Framework
{
    // 所有UI面板继承该类，不能使用Unity自带的Awake、Start、Update
    public abstract class UIPanelBase : MonoBehaviour
    {
        [SerializeField]
        private EPanelID m_ID;
        public EPanelID ID
        {
            get
            {
                return m_ID;
            }
        }

        protected virtual void OnCreate(object param)
        {

        }

        protected virtual void OnOpen(object param)
        {

        }

        protected virtual void OnUpdate(float deltaTime)
        {

        }

        protected virtual void OnClose(object param)
        {

        }
    }
}
