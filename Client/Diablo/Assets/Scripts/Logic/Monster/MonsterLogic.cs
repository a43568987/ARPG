using UnityEngine;
using UnityEngine.AI;

namespace Logic
{
    public class MonsterLogic
    {
        private static readonly float s_DelayDestroyTime = 3.0f;

        private static readonly string s_IdleAnimation = "Idle";
        private static readonly string s_MoveAnimation = "Move";
        private static readonly string s_AttackAnimation = "Attack";
        private static readonly string s_AttackIdleAnimation = "Attack_Idle";
        private static readonly string s_DeadAnimation = "Dead";

        private enum EEnemyState
        {
            Idle,
            Chase,
            Attack,
            Dead,
        }

        public bool CanDestroy
        {
            get
            {
                return m_DestroyTimer <= 0;
            }
        }

        private Monster m_Owner;
        private Player m_AttackTarget;
        private Animator m_Animator;
        private NavMeshAgent m_NavAgent;
        private EEnemyState m_State;
        private float m_AttackTimer;
        private float m_DestroyTimer;


        public MonsterLogic(Monster owner)
        {
            m_Owner = owner;
            m_State = EEnemyState.Idle;
            m_AttackTarget = GameManager.Instance.Player;

            m_Animator = m_Owner.gameObject.GetComponent<Animator>();
            m_NavAgent = m_Owner.gameObject.GetComponent<NavMeshAgent>();
            // 添加射线检测接收器
            RaycastReceiver raycaster = m_Owner.gameObject.AddComponent<RaycastReceiver>();
            raycaster.ID = m_Owner.ID;
            raycaster.Type = EActorType.Monster;
            // 添加动画事件接收器
            AnimatorEventReceiver eventReceiver = m_Owner.gameObject.AddComponent<AnimatorEventReceiver>();
            eventReceiver.OnAnimatorEvent += OnAnimatorEvent;
        }

        public void OnUpdate(float deltaTime)
        {
            if (m_AttackTimer > 0)
            {
                m_AttackTimer -= deltaTime;
            }
            switch (m_State)
            {
                case EEnemyState.Idle:
                    if (!m_NavAgent.isStopped)
                    {
                        m_NavAgent.isStopped = true;
                    }
                    if (m_AttackTarget.IsDead)
                    {
                        return;
                    }
                    else if (CanAttackPlayer())
                    {
                        m_State = EEnemyState.Attack;
                        m_Animator.CrossFade(s_AttackIdleAnimation, 0.2f);
                    }
                    else
                    {
                        m_State = EEnemyState.Chase;
                        m_Animator.CrossFade(s_MoveAnimation, 0.05f);
                    }
                    break;
                case EEnemyState.Chase:
                    if (CanAttackPlayer())
                    {
                        m_State = EEnemyState.Attack;
                        m_Animator.CrossFade(s_AttackIdleAnimation, 0.2f);
                    }
                    else
                    {
                        MoveToPlayer();
                    }
                    break;
                case EEnemyState.Attack:
                    if (m_AttackTarget.IsDead)
                    {
                        m_State = EEnemyState.Idle;
                        m_Animator.CrossFade(s_IdleAnimation, 0.2f);
                    }
                    else if (!CanAttackPlayer())
                    {
                        m_State = EEnemyState.Chase;
                        m_Animator.CrossFade(s_MoveAnimation, 0.05f);
                    }
                    else
                    {
                        if (!m_NavAgent.isStopped)
                        {
                            m_NavAgent.isStopped = true;
                        }
                        if (m_AttackTimer <= 0)
                        {
                            m_AttackTimer = 2.0f;
                            m_Owner.gameObject.transform.LookAt(m_AttackTarget.gameObject.transform);
                            m_Animator.CrossFade(s_AttackAnimation, 0.05f);
                        }
                    }
                    break;
                case EEnemyState.Dead:
                    if (m_DestroyTimer > 0)
                    {
                        m_DestroyTimer -= deltaTime;
                    }
                    break;
            }
        }

        private void OnAnimatorEvent(EAnimatorEventType type)
        {
            switch (type)
            {
                case EAnimatorEventType.Attack:
                    m_AttackTarget.BeDamaged(m_Owner.Data.Attack);
                    break;
                case EAnimatorEventType.FootStep:
                    break;
            }
        }

        private bool CanAttackPlayer()
        {
            // 非死亡状态且玩家在攻击范围内
            return m_State != EEnemyState.Dead && Vector3.Distance(m_Owner.Position, m_AttackTarget.Position) < m_Owner.Data.AttackRange;
        }

        public void MoveToPlayer()
        {
            if (m_NavAgent.isStopped)
            {
                m_NavAgent.isStopped = false;
            }
            m_NavAgent.SetDestination(m_AttackTarget.Position);
        }

        public void BeDamaged(float damage)
        {
            m_Owner.Data.CurrentHealth -= damage * (1 - m_Owner.Data.Defend / 100);
            if (m_Owner.Data.CurrentHealth <= 0)
            {
                m_Owner.Data.CurrentHealth = 0;
                m_DestroyTimer = s_DelayDestroyTime;
                m_State = EEnemyState.Dead;
                m_NavAgent.enabled = false;
                m_Animator.CrossFade(s_DeadAnimation, 0.1f);

                m_Owner.gameObject.GetComponent<RaycastReceiver>().enabled = false;
                m_Owner.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }
}
