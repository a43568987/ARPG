using UnityEngine;

namespace Logic
{
    // 逻辑放在GameManager里面，这个类只是驱动GameManager运行
    [DisallowMultipleComponent]
    public class GameEntry : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.Instance.OnInitialize();
        }

        private void Update()
        {
            // Time.deltaTime大于0.1就不准确了
            float deltaTime = Time.deltaTime;
            if(deltaTime > 0.1f)
            {
                deltaTime = 0.1f;
            }
            GameManager.Instance.OnUpdate(deltaTime);
        }
    }
}
