using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController{

    //敌人控制信息
    private GameObject m_Enemy = null;
    private Animator m_EnemyAnimator = null;
    private NavMeshAgent m_NavMeshAgent = null;
    private GameObject m_Player = null;
    private EnemyData m_EnemyData = null;
    private CharacterController m_CharacterController = null;

    private int m_TimerA = 0;
    private bool m_CDA = false;
    private int m_TimerB = 0;
    private bool m_CDB = false;

    public GameObject GetEnemy()
    {
        return m_Enemy;
    }
    public void SetEnemy(GameObject enemy)
    {
        m_Enemy = enemy;
    }
    public void SetData(EnemyData data)
    {
        m_EnemyData = data;
    }
    public void SetController(CharacterController controller)
    {
        m_CharacterController = controller;
    }

    

    // Use this for initialization
    public void OnInitialize()
    {
        m_Player = GameObject.FindWithTag("Player");
        //m_Enemy = Object.Instantiate(Resources.Load<GameObject>("Prefab/Enemy/Enemy"));
        //m_Enemy = Framework.PoolManager.Instance.Get("Prefab/Enemy/", "Enemy");
        m_Enemy.transform.position = m_Player.transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        m_EnemyAnimator = m_Enemy.GetComponent<Animator>();
        m_NavMeshAgent = m_Enemy.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    public void OnUpdate(float deltaTime)
    {
        EnemyControl();
    }

    private void EnemyControl()
    {
        EnemyMove();

        EnemyAttack();

        EnemyAttacked();

        EnemyDead();
    }

    private void EnemyMove()
    {
        //走向玩家
        if (m_EnemyData.m_Health >= 0)
        {
            this.MoveToTarget(m_Player.transform.position + (m_Enemy.transform.position - m_Player.transform.position).normalized * 5 / 4);
        }
        m_EnemyAnimator.SetFloat("MoveSpeed", m_NavMeshAgent.velocity.magnitude);
    }

    private void MoveToTarget(Vector3 target)
    {
        m_NavMeshAgent.SetDestination(target);
    }

    private void EnemyAttack()
    {
        //判断距离并开始攻击
        if (this.GetDistance() <= 1.8)
        {
            TurnToPlayer();
            m_EnemyAnimator.SetBool("IsAttacking", true);
        }
        else
        {
            m_EnemyAnimator.SetBool("IsAttacking", false);
        }
    }

    private float GetDistance()
    {
        return Vector3.Distance(m_Enemy.transform.position, m_Player.transform.position);
    }

    private void TurnToPlayer()
    {
        m_Enemy.transform.rotation = Quaternion.Lerp(m_Enemy.transform.rotation, Quaternion.LookRotation(m_Player.transform.position - m_Enemy.transform.position), Time.deltaTime * 2);
    }

    private void EnemyAttacked()
    {
        AnimatorStateInfo animatorInfo;
        animatorInfo = m_EnemyAnimator.GetCurrentAnimatorStateInfo(0);
        if ((animatorInfo.normalizedTime >= 0.5f) && (animatorInfo.IsName("Attack_Style_A")))
        {
            if (m_CDA == false)
            {
                m_CharacterController.GetHurt(m_EnemyData.m_Attack, m_EnemyData.m_Defend);
                m_CDA = true;
                m_TimerA = SetTimer(0.5f);
            }
        }
        if (Framework.TimerManager.Instance.IsOver(m_TimerA))
        {
            m_CDA = false;
        }
        if ((animatorInfo.normalizedTime >= 0.5f) && (animatorInfo.IsName("Attack_Style_B")))
        {
            if (m_CDB == false)
            {
                m_CharacterController.GetHurt(m_EnemyData.m_Attack, m_EnemyData.m_Defend);
                m_CDB = true;
                m_TimerB = SetTimer(0.5f);
            }
        }
        if (Framework.TimerManager.Instance.IsOver(m_TimerB))
        {
            m_CDB = false;
        }
    }

    private int SetTimer(float delay)
    {
        return Framework.TimerManager.Instance.AddTimer(delay, null);
    }

    private void EnemyDead()
    {
        if (m_EnemyData.m_Health <= 0)
        {
            m_EnemyAnimator.Play("Dead_Front");
        }
        if (m_EnemyData.m_IsDead)
        {
            Framework.PoolManager.Instance.Recycle(m_Enemy);
        }
    }

    public void GetHurt(float attack, float defend)
    {
        m_EnemyData.m_Health -= attack * (1-defend/100);
    }
}
