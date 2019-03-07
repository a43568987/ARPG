using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework;

namespace Logic.UI
{
    [DisallowMultipleComponent]
    public class UILaunchPanel : UIPanelBase
    {
        [SerializeField]
        private Image m_LogoImage;
        [SerializeField]
        private Button m_EnterButton;

        public override void OnCreate()
        {
            m_EnterButton.onClick.AddListener(() =>
            {
                EventManager.Instance.FireEvent(GameEvent.c_LaunchGame, null);
            });
        }

        public override void OnOpen(object param)
        {
            m_LogoImage.gameObject.SetActive(true);
            m_EnterButton.gameObject.SetActive(false);

            Color srcColor = m_LogoImage.color;
            m_LogoImage.color = new Color(srcColor.r, srcColor.g, srcColor.b, 0.0f);
            var tweener = m_LogoImage.DOFade(1.0f, 2.0f);
            tweener.OnComplete(() =>
            {
                m_EnterButton.gameObject.SetActive(true);
            });
        }
    }
}
