namespace Logic
{
    public class MonsterData
    {
        public float Health
        {
            get;
            private set;
        }

        public float CurrentHealth
        {
            get;
            set;
        }

        public float Attack
        {
            get;
            private set;
        }

        public float Defend
        {
            get;
            private set;
        }

        public float AttackRange
        {
            get;
            private set;
        }

        public MonsterData(float health, float attack, float defend, float attackRange)
        {
            Health = health;
            Attack = attack;
            Defend = defend;
            AttackRange = attackRange;

            CurrentHealth = Health;
        }
    }
}
