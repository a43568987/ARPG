using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class TimerManager : Singleton<TimerManager>
    {

        private List<Timer> m_Timers = new List<Timer>();

        private class Timer
        {
            public static int Count = 0;
            public int ID;
            public float DelayTime;
            public float TheTime;
            public bool IsOver;
            public Timer()
            {
                Count++;
                ID = Count;
                IsOver = false;
                TheTime = 0;
            }
            
        }

        // 添加一个定时器，返回定时器ID
        public int AddTimer(float delayTime, Action action)
        {
            
            Timer timer = new Timer();
            m_Timers.Add(timer);
            timer.DelayTime = delayTime;
            return timer.ID;
        }

        // 根据ID取消一个定时器
        public void RemoveTimer(int timerID)
        {
            for(int i=0;i < m_Timers.Count; i++)
            {
                if(m_Timers[i].ID == timerID)
                {
                    m_Timers.Remove(m_Timers[i]);
                }
            }
        }

        public bool IsOver(int timerID)
        {
            bool IsOver = false;
            for (int i = 0; i < m_Timers.Count; i++)
            {
                if (m_Timers[i].ID == timerID)
                {
                    if (m_Timers[i].IsOver)
                    {
                        IsOver = m_Timers[i].IsOver;
                        RemoveTimer(m_Timers[i].ID);
                    }
                    return IsOver;
                }
            }
            return false;
        }

        public void OnUpdate(float deltaTime)
        {

            for (int i = 0; i < m_Timers.Count; i++)
            {
                m_Timers[i].TheTime += deltaTime;
                if(m_Timers[i].TheTime >= m_Timers[i].DelayTime)
                {
                    m_Timers[i].IsOver = true;
                    
                }
            }
        }
    }
}
