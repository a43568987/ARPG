using UnityEngine;

namespace Logic
{
    // 用于接收射线检测的组件
    public class RaycastReceiver : MonoBehaviour
    {
        public int ID;              // 该物体的ID
        public EActorType Type;     // 该物体的类别
    }
}
