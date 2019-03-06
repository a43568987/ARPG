using System;

namespace Framework
{
    public class EventManager : Singleton<EventManager>
    {
        public void AddListener(int eventID, Action<object> handler)
        {

        }

        public void RemoveListener(int eventID, Action<object> handler)
        {

        }

        public void FireEvent(int eventID, object param)
        {

        }
    }
}
