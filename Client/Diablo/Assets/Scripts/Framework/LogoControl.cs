using UnityEngine;

/// <summary>
/// 这是控制logo渐变的代码，挂在Logo上，使用CanvasGroup实现渐变
/// </summary>
namespace Framework
{
    public class LogoControl : MonoBehaviour
    {
        private float c_Alpha = 1.0f;
        private float c_AlphaSpeed = 1.0f;

        private CanvasGroup c_Cg;

        public GameObject c_StartButton;//开始按钮

        void Start()
        {
            c_Cg = this.transform.GetComponent<CanvasGroup>();
            c_StartButton.SetActive(false);
            //Debug.Log(m_cg.alpha);
        }

        void Update()
        {
            if (c_Alpha != c_Cg.alpha)
            {
                c_Cg.alpha = Mathf.Lerp(c_Cg.alpha, c_Alpha, c_AlphaSpeed * Time.deltaTime);
                //static functionLerp (from : float, to : float, t : float) : float
                //基于浮点数t返回a到b之间的插值，t限制在0～1之间。当t = 0返回from，当t = 1 返回to。当t = 0.5 返回from和to的平均值。
                if (Mathf.Abs(c_Alpha - c_Cg.alpha) <= 0.01)//计算并返回指定参数绝对值。
                {
                    c_Cg.alpha = c_Alpha;//渐变结束
                    c_StartButton.SetActive(true);//激活开始按钮
                }
            }
        }
    }

}