using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Logic
{
    public class GameManager : Singleton<GameManager>
    {
        public Player Player
        {
            get;
            private set;
        }

        private List<Monster> m_EnemyList = new List<Monster>();
        private Dictionary<int, Monster> m_EnemyDict;
        private float m_EnemySpawnTimer = 7;

        // 游戏初始化
        public void OnInitialize()
        {
            m_EnemyList = new List<Monster>();
            m_EnemyDict = new Dictionary<int, Monster>();
            // 创建主角
            Player = new Player("Prefab/Player/Player", new Vector3(0, 0, 15), 100, 30, 10, 1.5f);
        }

        // 游戏每帧更新
        public void OnUpdate(float deltaTime)
        {
            InputManager.Instance.OnUpdate(deltaTime);

            Player.OnUpdate(deltaTime);

            //固定秒生成一个敌人
            m_EnemySpawnTimer += deltaTime;
            if (m_EnemySpawnTimer >= 10)
            {
                m_EnemySpawnTimer = 0;
                Vector3 spawnPosition = Player.Position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                Monster enemy = new Monster("Prefab/Monster/Monster", spawnPosition, 100, 10, 10, 1.5f);
                m_EnemyList.Add(enemy);
                m_EnemyDict.Add(enemy.ID, enemy);
            }

            // 敌人Update，检测死亡
            for (int i = 0; i < m_EnemyList.Count; i++)
            {
                var enemy = m_EnemyList[i];
                enemy.OnUpdate(deltaTime);
                if (enemy.IsDead && enemy.CanDestroy)
                {
                    Object.Destroy(m_EnemyList[i].gameObject);
                    m_EnemyList.RemoveAt(i);
                    m_EnemyDict.Remove(enemy.ID);
                    i--;
                }
            }
        }

        public Monster GetEnemyByID(int enemyID)
        {
            return m_EnemyDict[enemyID];
        }
    }
}
