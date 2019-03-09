using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Framework;
using Logic.UI;

namespace Logic
{
    public sealed class GameManager : Singleton<GameManager>
    {
        public Dictionary<GameObject, Enemy> EnemyDict
        {
            get;
            private set;
        }

        private bool m_IsFighting = false;
        private Character m_Player = new Character();
        private List<Enemy> m_Enemies = new List<Enemy>();
        private float m_EnemySpawnTimer = 7;

        public void OnInitialize()
        {
            EnemyDict = new Dictionary<GameObject, Enemy>();
            EnemyPreLoad(5);
            EventManager.Instance.AddListener(GameEvent.c_LaunchGame, OnLaunchGame);
            ObjectPool.Init();
            UIManager.Instance.RegisterAllUI();
            UIManager.Instance.OpenUI(EPanelID.LaunchPanel);

            
        }

        public void OnUpdate(float deltaTime)
        {
            if (!m_IsFighting)
            {
                return;
            }
            TimerManager.Instance.OnUpdate(deltaTime);
            m_Player.OnUpdate(deltaTime);

            EnemySpawn(deltaTime);

        }

        private void EnemySpawn(float deltaTime)
        {
            m_EnemySpawnTimer += deltaTime;
            if (m_EnemySpawnTimer >= 10)
            {
                m_EnemySpawnTimer = 0;
                Enemy m_Enemy = new Enemy();
                m_Enemy.GetController().SetEnemy(PoolManager.Instance.Get("Prefab/Enemy/", "Enemy"));
                m_Enemy.OnInitialize();
                m_Enemy.GetController().SetController(m_Player.GetController());
                m_Enemies.Add(m_Enemy);
                EnemyDict.Add(m_Enemy.GetController().GetEnemy(), m_Enemy);
            }
            for (int i = 0; i < m_Enemies.Count; i++)
            {
                if (m_Enemies[i].GetController().GetEnemy().activeInHierarchy)
                    m_Enemies[i].OnUpdate(deltaTime);
                else
                {
                    EnemyDict.Remove(m_Enemies[i].GetController().GetEnemy());
                    m_Enemies.Remove(m_Enemies[i]);
                }
            }
        }

        private void EnemyPreLoad(int count)
        {
            PoolManager.Instance.Preload("Prefab/Enemy/","Enemy",count);
        }

        private void OnLaunchGame(object param)
        {
            UIManager.Instance.CloseUI(EPanelID.LaunchPanel);
            CoroutineMgr.Instance.Start(SwitchScene(GameConstants.c_FightSceneName, () =>
            {
                m_IsFighting = true;
                m_Player = new Character();
                m_Player.OnInitialize();
            }));
        }

        private IEnumerator SwitchScene(string sceneName, Action action)
        {
            yield return null;
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return null;
            GC.Collect();
            yield return Resources.UnloadUnusedAssets();

            if (action != null)
            {
                action.Invoke();
            }
        }
    }
}
