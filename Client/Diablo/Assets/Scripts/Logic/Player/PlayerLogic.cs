using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace Logic
{
    public class PlayerLogic
    {
        // 动画常量
        private static readonly string s_IdleAnimation = "Idle";
        private static readonly string s_MoveAnimation = "Move";
        private static readonly string s_AttackAnimation = "Attack";
        private static readonly string s_DeadAnimation = "Dead";
        private static readonly string s_AttackIdleAnimation = "Attack_Idle";

        // 音效常量
        private static readonly string s_ReadySound = "Sound/FootmanReady";
        private static readonly string s_DeadSound = "Sound/FootmanDeath";
        private static readonly string s_AttackHitSound = "Sound/FootmanAttackHit";
        private static readonly List<string> s_MoveSoundList = new List<string>()
        {
            "Sound/FootmanYes1",
            "Sound/FootmanYes2",
            "Sound/FootmanYes3",
            "Sound/FootmanYes4",
        };
        private static readonly List<string> s_AttackSoundList = new List<string>()
        {
            "Sound/FootmanYesAttack1",
            "Sound/FootmanYesAttack2",
            "Sound/FootmanYesAttack3",
            "Sound/FootmanYesAttack4",
        };

        // 玩家状态
        private enum EPlayerState
        {
            Idle,       // 空闲状态
            Move,       // 普通移动
            Chase,      // 向敌人移动
            Attack,     // 攻击中
            Dead,       // 死亡
        }

        // 玩家音效
        private enum EPlayerSound
        {
            Ready,      // 出生
            Move,       // 移动
            AttackMove, // 攻击移动
            AttackHit,  // 攻击击中
            Dead,       // 死亡
        }

        private Player m_Owner;
        private Animator m_Animator;
        private NavMeshAgent m_NavAgent;
        private AudioSource m_SoundPlayer;
        private AudioSource m_AttackHitSoundPlayer;
        private Dictionary<string, AudioClip> m_SoundDict;
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
            // 添加声音控制器
            m_SoundPlayer = m_Owner.gameObject.AddComponent<AudioSource>();
            m_SoundPlayer.playOnAwake = false;
            m_AttackHitSoundPlayer = m_Owner.gameObject.AddComponent<AudioSource>();
            m_AttackHitSoundPlayer.playOnAwake = false;
            m_AttackHitSoundPlayer.volume = 0.4f;
            // 预加载音效
            m_SoundDict = new Dictionary<string, AudioClip>();
            foreach (var audioClip in s_MoveSoundList)
            {
                m_SoundDict.Add(audioClip, Resources.Load<AudioClip>(audioClip));
            }
            foreach (var audioClip in s_AttackSoundList)
            {
                m_SoundDict.Add(audioClip, Resources.Load<AudioClip>(audioClip));
            }
            m_SoundDict.Add(s_ReadySound, Resources.Load<AudioClip>(s_ReadySound));
            m_SoundDict.Add(s_DeadSound, Resources.Load<AudioClip>(s_DeadSound));
            m_SoundDict.Add(s_AttackHitSound, Resources.Load<AudioClip>(s_AttackHitSound));

            PlaySound(EPlayerSound.Ready);
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
                            m_Animator.CrossFade(s_MoveAnimation, 0.2f);
                        }
                        PlaySound(EPlayerSound.AttackMove);
                        break;
                    default:
                        m_State = EPlayerState.Move;
                        m_MoveTarget = inputData.worldPosition;
                        m_Animator.CrossFade(s_MoveAnimation, 0.05f);
                        PlaySound(EPlayerSound.Move);
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
                        m_Animator.CrossFade(s_IdleAnimation, 0.2f);
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
                PlaySound(EPlayerSound.Dead);
            }
        }

        private void OnAnimatorEvent(EAnimatorEventType type)
        {
            switch (type)
            {
                case EAnimatorEventType.Attack:
                    m_AttackTarget.BeDamaged(m_Owner.Data.Attack);
                    PlaySound(EPlayerSound.AttackHit);
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

        private void PlaySound(EPlayerSound soundType)
        {
            switch (soundType)
            {
                case EPlayerSound.Ready:
                    m_SoundPlayer.clip = m_SoundDict[s_ReadySound];
                    break;
                case EPlayerSound.Move:
                    m_SoundPlayer.clip = m_SoundDict[s_MoveSoundList[Random.Range(0, s_MoveSoundList.Count)]];
                    break;
                case EPlayerSound.AttackMove:
                    m_SoundPlayer.clip = m_SoundDict[s_AttackSoundList[Random.Range(0, s_AttackSoundList.Count)]];
                    break;
                case EPlayerSound.AttackHit:
                    m_AttackHitSoundPlayer.clip = m_SoundDict[s_AttackHitSound];
                    m_AttackHitSoundPlayer.Play();
                    return;
                case EPlayerSound.Dead:
                    m_SoundPlayer.clip = m_SoundDict[s_DeadSound];
                    break;
            }
            m_SoundPlayer.loop = false;
            m_SoundPlayer.Play();
        }
    }
}
