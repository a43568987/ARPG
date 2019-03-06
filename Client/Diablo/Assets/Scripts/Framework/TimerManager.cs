using System;

namespace Framework
{
    public class TimerManager : Singleton<TimerManager>
    {
        private class Timer
        {
            public int ID;
        }

        // 添加一个定时器，返回定时器ID
        public int AddTimer(float delayTime, Action action)
        {
            return -1;
        }

        // 根据ID取消一个定时器
        public void RemoveTimer(int timerID)
        {

        }

        public void OnUpdate(float deltaTime)
        {

        }
    }
}
