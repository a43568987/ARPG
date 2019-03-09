using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class ObjectPool
    {

        private static ObjectPool instance;

        private Dictionary<string, List<GameObject>> pool;

        private Dictionary<string, GameObject> prefabs;

        private ObjectPool()
        {
            this.pool = new Dictionary<string, List<GameObject>>();
            this.prefabs = new Dictionary<string, GameObject>();
        }

        public static void Init()
        {
            bool flag = ObjectPool.instance != null;
            if (flag)
            {
                ObjectPool.instance = null;
            }
        }

        public static ObjectPool GetInstance()
        {
            bool flag = ObjectPool.instance == null;
            if (flag)
            {
                ObjectPool.instance = new ObjectPool();
            }
            return ObjectPool.instance;
        }

        public GameObject GetObj(string path, string objName)
        {
            bool flag = this.pool.ContainsKey(objName);
            GameObject gameObject;
            if (flag)
            {
                bool flag2 = this.pool[objName].Count > 0;
                if (flag2)
                {
                    gameObject = this.pool[objName][0];
                    gameObject.SetActive(true);
                    this.pool[objName].Remove(gameObject);
                    return gameObject;
                }
            }
            bool flag3 = this.prefabs.ContainsKey(objName);
            GameObject gameObject2;
            if (flag3)
            {
                gameObject2 = this.prefabs[objName];
            }
            else
            {
                gameObject2 = Resources.Load<GameObject>(path + objName);
                this.prefabs.Add(objName, gameObject2);
            }
            gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
            gameObject.name = objName;
            return gameObject;
        }

        public void RecycleObj(GameObject obj)
        {
            obj.SetActive(false);
            bool flag = this.pool.ContainsKey(obj.name);
            if (flag)
            {
                this.pool[obj.name].Add(obj);
            }
            else
            {
                this.pool.Add(obj.name, new List<GameObject>
                {
                    obj
                });
            }
        }


    }
}
