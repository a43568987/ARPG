using UnityEngine;
using System;

namespace Logic.UI
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

        public virtual void OnCreate()
        {

        }

        public virtual void OnOpen(object param)
        {

        }

        public virtual void OnUpdate(float deltaTime)
        {

        }

        public virtual void OnClose(object param)
        {

        }
    }
}
