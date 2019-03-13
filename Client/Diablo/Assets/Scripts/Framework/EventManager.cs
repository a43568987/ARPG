using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<int, LinkedList<Action<object>>> m_HandlerDict = new Dictionary<int, LinkedList<Action<object>>>();

        public void AddListener(int eventID, Action<object> handler)
        {
            LinkedList<Action<object>> handlerList;
            if (!m_HandlerDict.TryGetValue(eventID, out handlerList))
            {
                handlerList = new LinkedList<Action<object>>();
                m_HandlerDict.Add(eventID, handlerList);
            }
            if (!handlerList.Contains(handler))
            {
                handlerList.AddLast(handler);
            }
            else
            {
                Debug.LogError("[EventManager] 重复添加事件监听！");
            }
        }

        public void RemoveListener(int eventID, Action<object> handler)
        {
            LinkedList<Action<object>> handlerList;
            if (m_HandlerDict.TryGetValue(eventID, out handlerList))
            {
                handlerList.Remove(handler);
            }
            else
            {
                Debug.LogError("[EventManager] 移除的事件监听不存在！");
            }
        }

        public void FireEvent(int eventID, object param)
        {
            LinkedList<Action<object>> handlerList;
            if (m_HandlerDict.TryGetValue(eventID, out handlerList))
            {
                foreach (var entry in handlerList)
                {
                    entry.Invoke(param);
                }
            }
        }
    }
}
