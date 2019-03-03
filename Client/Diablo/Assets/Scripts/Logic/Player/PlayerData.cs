namespace Logic
{
    public class PlayerData
    {
        // 最大生命值
        public float MaxHealth
        {
            get;
            private set;
        }

        // 当前生命值
        public float CurrentHealth
        {
            get;
            set;
        }

        // 攻击力
        public float Attack
        {
            get;
            private set;
        }

        // 防御力
        public float Defend
        {
            get;
            private set;
        }

        // 攻击范围
        public float AttackRange
        {
            get;
            private set;
        }

        public PlayerData(float health, float attack, float defend, float attackRange)
        {
            MaxHealth = health;
            Attack = attack;
            Defend = defend;
            AttackRange = attackRange;

            CurrentHealth = MaxHealth;
        }
    }
}
