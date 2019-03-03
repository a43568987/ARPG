using UnityEngine;

namespace Logic
{
    public class InputData
    {
        public Vector3 screenPosition;      // 屏幕坐标
        public Vector3 worldPosition;       // 世界坐标
        public int clickedObjectID = -1;    // 选中物体的ID
        public EActorType clickedType;      // 鼠标选中物体的类型

        public void Reset()
        {
            screenPosition = Vector3.zero;
            worldPosition = Vector3.zero;
            clickedObjectID = -1;
            clickedType = EActorType.Other;
        }
    }
}
