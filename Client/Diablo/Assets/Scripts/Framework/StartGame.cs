using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 这是控制Start按钮的脚本，挂在GameEntry上
/// </summary>
namespace Framework
{
    public class StartGame
    {
        public Button c_StartButton;//按钮
        public CanvasGroup c_StartGame;//使用CanvasGroup实现按钮的呼吸效果
        private float c_FlashSpeed = 1.0f;//速度
        private bool m_Flash;


        void Start()
        {
            c_StartGame.alpha = 0;
            m_Flash = false;
        }

        void Update()
        {
            if (c_StartButton.IsActive())
            {
                ButttonFlash();
            }
        }

        public void StartButton()//场景跳转
        {
            SceneManager.LoadScene("Main");
        }

        public void ButttonFlash()//实现按钮呼吸效果
        {
            if (!m_Flash)
            {
                c_StartGame.alpha  += c_FlashSpeed * Time.deltaTime;
                if (c_StartGame.alpha == 1)
                {
                    m_Flash = true;
                }
            }
            else
            {
                c_StartGame.alpha -= c_FlashSpeed * Time.deltaTime;
                if (c_StartGame.alpha == 0)
                {
                    m_Flash = false;
                }
            }
        }
    }
}

