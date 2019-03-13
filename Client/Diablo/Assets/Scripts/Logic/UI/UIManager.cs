using UnityEngine;
using System.Collections.Generic;
using Framework;

namespace Logic.UI
{
    public enum EPanelID
    {
        LaunchPanel,
        HeadPanel,
    }

    public class UIManager : Singleton<UIManager>
    {
        private Dictionary<EPanelID, UIPanelBase> m_UIDict = new Dictionary<EPanelID, UIPanelBase>();

        public void RegisterAllUI()
        {
            m_UIDict.Clear();

            GameObject root = GameObject.Find("UIRoot");
            Object.DontDestroyOnLoad(root);
            var panels = root.GetComponentsInChildren<UIPanelBase>(true);
     
            foreach (var entry in panels)
            {
                entry.OnCreate();
                // 默认关闭面板
                entry.gameObject.SetActive(false);
                m_UIDict.Add(entry.ID, entry);
            }
        }

        public void OpenUI(EPanelID id, object param = null)
        {
            UIPanelBase panel;
            if (m_UIDict.TryGetValue(id, out panel))
            {
                panel.gameObject.SetActive(true);
                panel.OnOpen(param);
            }
            else
            {
                Debug.LogError("[UIManager] 尝试打开一个不存在的ID: " + id);
            }
        }

        public void CloseUI(EPanelID id, object param = null)
        {
            UIPanelBase panel;
            if (m_UIDict.TryGetValue(id, out panel))
            {
                panel.OnClose(param);
                panel.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("[UIManager] 尝试关闭一个不存在的ID: " + id);
            }
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entry in m_UIDict)
            {
                var panel = entry.Value;
                if (panel.gameObject.activeInHierarchy)
                {
                    panel.OnUpdate(deltaTime);
                }
            }
        }
    }
}
