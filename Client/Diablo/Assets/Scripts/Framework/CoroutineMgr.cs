using System.Collections;
using UnityEngine;

namespace Framework
{
    public class CoroutineMgr : Singleton<CoroutineMgr>
    {
        private CoroutineAgent m_Agent;

        protected override void OnCreate()
        {
            GameObject go = new GameObject("Coroutine");
            m_Agent = go.AddComponent<CoroutineAgent>();
            go.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            Object.DontDestroyOnLoad(go);
        }

        public Coroutine Start(IEnumerator enumerator)
        {
            return m_Agent.StartCoroutine(enumerator);
        }

        public void Stop(Coroutine coroutine)
        {
            m_Agent.StopCoroutine(coroutine);
        }
    }
}
