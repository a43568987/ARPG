using UnityEngine;

namespace Logic
{
    // 逻辑放在GameManager里面，这个类只是驱动GameManager运行
    [DisallowMultipleComponent]
    public class GameEntry : MonoBehaviour
    {
        private void Awake()
        {
            // 初始化Unity设置: 锁帧、后台运行、高质量骨骼动画
            Application.runInBackground = true;
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            QualitySettings.blendWeights = BlendWeights.FourBones;

            GameManager.Instance.OnInitialize();
        }

        private void Update()
        {
            // Time.deltaTime大于0.1就不准确了
            float deltaTime = Time.deltaTime;
            if (deltaTime > 0.1f)
            {
                deltaTime = 0.1f;
            }
            GameManager.Instance.OnUpdate(deltaTime);
        }
    }
}
