using UnityEngine;
using UnityEngine.AI;
using Framework;

namespace Logic
{
    public class InputManager : Singleton<InputManager>
    {
        private const int c_MouseLeftButton = 0;
        private const int c_MouseRightButton = 1;
        private const int c_MouseMiddleButton = 2;

        public bool HasInput
        {
            get;
            private set;
        }

        public InputData Data
        {
            get;
            private set;
        }

        protected override void OnCreate()
        {
            Data = new InputData();
        }

        public void OnUpdate(float deltaTime)
        {
            // 每帧重置数据
            HasInput = false;
            Data.Reset();

            // 安卓手机点击移动，默认为鼠标左键
#if !UNITY_EDITOR && UNITY_ANDROID
            if (Input.GetMouseButtonDown(c_MouseLeftButton))
#else
            if (Input.GetMouseButtonDown(c_MouseRightButton))
#endif
            {
                HasInput = true;
                Data.screenPosition = Input.mousePosition;

                RaycastHit m_HitData = new RaycastHit();
                Ray m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(m_ray, out m_HitData))
                {
                    NavMeshHit navMeshHit;
                    NavMesh.SamplePosition(m_HitData.point, out navMeshHit, 10.0f, NavMesh.AllAreas);
                    Data.worldPosition = navMeshHit.position;
                    var gameObject = m_HitData.collider.gameObject;
                    RaycastReceiver receiver = gameObject.GetComponent<RaycastReceiver>();
                    if (receiver != null && receiver.enabled)
                    {
                        Data.clickedType = receiver.Type;
                        Data.clickedObjectID = receiver.ID;
                    }
                }
                else
                {
                    HasInput = false;
                }
            }
        }
    }
}
