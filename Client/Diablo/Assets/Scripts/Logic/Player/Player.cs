using UnityEngine;

namespace Logic
{
    public class Player
    {
        public GameObject gameObject
        {
            get;
            private set;
        }

        public Vector3 Position
        {
            get
            {
                return gameObject.transform.position;
            }
            set
            {
                gameObject.transform.position = value;
            }
        }

        public PlayerData Data
        {
            get;
            private set;
        }

        private PlayerLogic m_Logic;

        public bool IsDead
        {
            get
            {
                return Data.CurrentHealth <= 0;
            }
        }

        public Player(string prefabPath, Vector3 position, float health, float attack, float defend, float attackRange)
        {
            // 加载GameObject
            gameObject = Object.Instantiate(Resources.Load<GameObject>(prefabPath));
            gameObject.transform.position = position;
            // 创建角色数据
            Data = new PlayerData(health, attack, defend, attackRange);
            // 创建角色控制器
            m_Logic = new PlayerLogic(this);
        }

        public void OnUpdate(float deltaTime)
        {
            m_Logic.OnUpdate(deltaTime);
        }

        public void BeDamaged(float damage)
        {
            m_Logic.BeDamaged(damage);
        }
    }
}
