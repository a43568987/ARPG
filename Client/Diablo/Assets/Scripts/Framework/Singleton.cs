using System;

namespace Framework
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static T s_Instance = null;
        public static T Instance
        {
            get
            {
                if(s_Instance == null)
                {
                    s_Instance = Activator.CreateInstance<T>();
                    s_Instance.OnCreate();
                }
                return s_Instance;
            }
        }

        public static void DestroyInstance()
        {
            if(s_Instance != null)
            {
                s_Instance.OnDestroy();
                s_Instance = null;
            }
        }

        protected virtual void OnCreate()
        {
        }

        protected virtual void OnDestroy()
        {
        }
    }
}
