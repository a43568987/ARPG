using UnityEngine;
using UnityEngine.AI;

namespace Logic
{
    public class PlayerLogic
    {
        private static readonly string s_IdleAnimation = "Idle";
        private static readonly string s_MoveAnimation = "Move";
        private static readonly string s_AttackAnimation = "Attack";
        private static readonly string s_DeadAnimation = "Dead";
        private static readonly string s_AttackIdleAnimation = "Attack_Idle";
        private static readonly float s_CrossFadeTime = 0.2f;

        private enum EPlayerState
        {
            Idle,       // 空闲状态
            Move,       // 普通移动
            Chase,      // 向敌人移动
            Attack,     // 攻击中
            Dead,       // 死亡
        }

        private Player m_Owner;
        private Animator m_Animator;
        private NavMeshAgent m_NavAgent;
        private EPlayerState m_State;
        private Vector3 m_MoveTarget;
        private Monster m_AttackTarget;
        private float m_AttackTimer;

        // 把Character注入进来，就不需要去外面找了
        public PlayerLogic(Player player)
        {
            m_Owner = player;
            // 获取Unity提供的控制器
            m_Animator = m_Owner.gameObject.GetComponent<Animator>();
            m_NavAgent = m_Owner.gameObject.GetComponent<NavMeshAgent>();
            // 初始化参数
            m_State = EPlayerState.Idle;
            m_AttackTimer = 0;
            // 添加动画事件接收器
            AnimatorEventReceiver eventReceiver = m_Owner.gameObject.AddComponent<AnimatorEventReceiver>();
            eventReceiver.OnAnimatorEvent += OnAnimatorEvent;
        }

        public void OnUpdate(float deltaTime)
        {
            if (m_State == EPlayerState.Dead)
            {
                return;
            }

            if (m_AttackTimer > 0)
            {
                m_AttackTimer -= deltaTime;
            }

            if (InputManager.Instance.HasInput)
            {
                var inputData = InputManager.Instance.Data;
                switch (inputData.clickedType)
                {
                    case EActorType.Monster:
                        m_AttackTarget = GameManager.Instance.GetEnemyByID(inputData.clickedObjectID);
                        if (CanAttackEnemy())
                        {
                            m_State = EPlayerState.Attack;
                            m_Animator.CrossFade(s_AttackIdleAnimation, 0.1f);
                        }
                        else
                        {
                            m_State = EPlayerState.Chase;
                            m_Animator.CrossFade(s_MoveAnimation, s_CrossFadeTime);
                        }
                        break;
                    default:
                        m_State = EPlayerState.Move;
                        m_MoveTarget = inputData.worldPosition;
                        m_Animator.CrossFade(s_MoveAnimation, 0.05f);
                        break;
                }
            }
            switch (m_State)
            {
                case EPlayerState.Idle:
                    if (!m_NavAgent.isStopped)
                    {
                        m_NavAgent.isStopped = true;
                    }
                    var animatorState = m_Animator.GetCurrentAnimatorStateInfo(0);
                    if (!animatorState.IsName(s_IdleAnimation) && animatorState.normalizedTime >= 0.9f)
                    {
                        m_Animator.CrossFade(s_IdleAnimation, 0.1f);
                    }
                    break;
                case EPlayerState.Move:
                    if (Vector3.Distance(m_Owner.Position, m_MoveTarget) < 0.1f)
                    {
                        m_Owner.Position = m_MoveTarget;
                        m_NavAgent.isStopped = true;
                        m_State = EPlayerState.Idle;
                        m_Animator.CrossFade(s_IdleAnimation, s_CrossFadeTime);
                    }
                    if (m_NavAgent.isStopped)
                    {
                        m_NavAgent.isStopped = false;
                    }
                    m_NavAgent.SetDestination(m_MoveTarget);
                    break;
                case EPlayerState.Chase:
                    if (CanAttackEnemy())
                    {
                        m_State = EPlayerState.Attack;
                        m_Animator.CrossFade(s_AttackIdleAnimation, 0.1f);
                        if (!m_NavAgent.isStopped)
                        {
                            m_NavAgent.isStopped = true;
                        }
                        return;
                    }
                    if (m_NavAgent.isStopped)
                    {
                        m_NavAgent.isStopped = false;
                    }
                    m_NavAgent.SetDestination(m_AttackTarget.Position);
                    break;
                case EPlayerState.Attack:
                    if (!m_AttackTarget.IsDead)
                    {
                        m_Owner.gameObject.transform.LookAt(m_AttackTarget.gameObject.transform);
                        if (m_AttackTimer <= 0)
                        {
                            m_Animator.CrossFade(s_AttackAnimation, 0.05f);
                            m_AttackTimer = 1.5f;
                        }
                    }
                    else
                    {
                        m_State = EPlayerState.Idle;
                    }
                    break;
            }
        }

        public void BeDamaged(float damage)
        {
            m_Owner.Data.CurrentHealth -= damage * (1 - m_Owner.Data.Defend / 100);
            if (m_Owner.Data.CurrentHealth <= 0)
            {
                m_Owner.Data.CurrentHealth = 0;
                m_State = EPlayerState.Dead;
                m_Animator.CrossFade(s_DeadAnimation, 0.2f);
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
                    // TODO:
                    break;
            }
        }

        private bool CanAttackEnemy()
        {
            if (m_AttackTarget == null || m_AttackTarget.IsDead)
            {
                return false;
            }
            return Vector3.Distance(m_Owner.Position, m_AttackTarget.Position) < m_Owner.Data.AttackRange;
        }
    }
}
