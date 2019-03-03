using UnityEngine;

namespace Logic
{
    public class Monster
    {
        private static int s_IDGenerater = 0;

        public int ID
        {
            get;
            private set;
        }

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

        public MonsterData Data
        {
            get;
            private set;
        }

        public bool IsDead
        {
            get
            {
                return Data.CurrentHealth <= 0;
            }
        }

        public bool CanDestroy
        {
            get
            {
                return IsDead && m_Logic.CanDestroy;
            }
        }

        private MonsterLogic m_Logic;

        public Monster(string prefabPath, Vector3 position, float health, float attack, float defend, float attackRange)
        {
            ID = s_IDGenerater++;

            gameObject = Object.Instantiate(Resources.Load<GameObject>(prefabPath));
            gameObject.transform.position = position;
            Data = new MonsterData(health, attack, defend, attackRange);
            m_Logic = new MonsterLogic(this);
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
