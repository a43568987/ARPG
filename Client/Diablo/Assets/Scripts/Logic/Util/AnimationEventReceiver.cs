using System;
using UnityEngine;

namespace Logic
{
    // 动画事件
    public enum EAnimatorEventType
    {
        Attack = 1,     // 攻击事件     
        FootStep,   // 脚步声
    }

    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    public class AnimatorEventReceiver : MonoBehaviour
    {
        public event Action<EAnimatorEventType> OnAnimatorEvent;

        private void OnEvent(int type)
        {
            OnAnimatorEvent((EAnimatorEventType)type);
        }
    }
}
