using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class GameManager : Singleton<GameManager>
    {
        public Dictionary<GameObject, Enemy> EnemyDic;

        private Character m_Charater = new Character();
        private List<Enemy> m_Enemies = new List<Enemy>();
        private float m_Timer = 7;
        private int m_SceneNum=0;
        private bool m_Scene1Loaded = false;

        // 游戏初始化
        public void OnInitialize()
        {
            Scene1_OnInitialize();
        }

        // 游戏每帧更新
        public void OnUpdate(float deltaTime)
        {
            InstantiateEnemy(deltaTime);
        }

        private void Scene1_OnInitialize()
        {
            if (m_SceneNum == 1)
            {
                m_Charater.OnInitialize();
                EnemyDic = new Dictionary<GameObject, Enemy>();
                m_Scene1Loaded = true;
            }
        }

        public Dictionary<GameObject, Enemy> GetDic()
        {
            return EnemyDic;
        }

        public void SceneLoad(int SceneNum)//场景跳转
        {
            SceneManager.LoadScene(SceneNum);
            m_SceneNum = SceneNum;
        }

        public void InstantiateEnemy(float deltaTime)
        {
            if (m_SceneNum == 1 && m_Scene1Loaded)
            {
                m_Charater.OnUpdate(deltaTime);
                m_Timer += deltaTime;
                //固定秒生成一个敌人
                if (m_Timer >= 10)
                {
                    m_Timer = 0;
                    Enemy m_Enemy = new Enemy();
                    m_Enemy.OnInitialize();
                    m_Enemy.GetController().SetController(m_Charater.GetController());
                    m_Enemies.Add(m_Enemy);

                    EnemyDic.Add(m_Enemy.GetController().GetEnemy(), m_Enemy);

                }
                //敌人update，检测死亡
                for (int i = 0; i < m_Enemies.Count; i++)
                {
                    m_Enemies[i].OnUpdate(deltaTime);
                    if (m_Enemies[i].GetData().m_IsDead == true)
                    {
                        Object.Destroy(m_Enemies[i].GetController().GetEnemy());
                        m_Enemies.Remove(m_Enemies[i]);
                    }

                }
            }
        }
    }
}
