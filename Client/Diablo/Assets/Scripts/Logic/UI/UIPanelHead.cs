using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace Logic.UI
{
    [DisallowMultipleComponent]
    public class UIPanelHead : UIPanelBase
    {
        [SerializeField]
        private Image m_HeadPortrait;
        [SerializeField]
        private Image m_Box;
        [SerializeField]
        private Image m_Blood;

        private float m_MaxBlood;

        private CharacterData m_player;
        // Use this for initialization
        public override void OnCreate()
        {
            EventManager.Instance.AddListener(GameEvent.c_UpdateBloodUI, UpdateBloodUI);
        }
        public override void OnOpen(object param)
        {
            m_player = GameManager.Instance.GetPlayer().GetData();
            m_MaxBlood = m_player.GetHealth();
        }
        public void UpdateBloodUI(object param)
        {
            m_Blood.fillAmount = m_player.GetHealth() / m_MaxBlood;
        }
    }
}

