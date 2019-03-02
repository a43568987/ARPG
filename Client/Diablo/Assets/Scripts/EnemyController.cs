using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController{

    //角色控制信息
    private GameObject m_Enemy = null;
    public GameObject GetEnemy()
    {
        return m_Enemy;
    }
    private Animator m_EnemyAnimator = null;
    private NavMeshAgent m_NavMeshAgent = null;
    private GameObject m_Player = null;

    private EnemyData m_EnemyData = null;
    public void SetData(EnemyData data)
    {
        m_EnemyData = data;
    }
    private CharacterController m_CharacterController = null;
    public void SetController(CharacterController controller)
    {
        m_CharacterController = controller;
    }

    private float m_TimerA = 0;
    private bool m_CDA = false;
    private float m_TimerB = 0;
    private bool m_CDB = false;

    // Use this for initialization
    public void OnInitialize()
    {
        m_Player = GameObject.FindWithTag("Player");
        m_Enemy = Object.Instantiate(Resources.Load<GameObject>("Prefab/Enemy/Enemy"));
        m_Enemy.transform.position = m_Player.transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        m_EnemyAnimator = m_Enemy.GetComponent<Animator>();
        m_NavMeshAgent = m_Enemy.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    public void OnUpdate(float deltaTime)
    {
        //走向玩家
        if (m_EnemyData.m_Health >= 0)
        {
            m_NavMeshAgent.SetDestination(m_Player.transform.position + (m_Enemy.transform.position - m_Player.transform.position).normalized * 5 / 4);
        }
        //判断距离并开始攻击
        if (Vector3.Distance(m_Enemy.transform.position, m_Player.transform.position) <= 1.8)
        {
            m_Enemy.transform.rotation = Quaternion.Lerp(m_Enemy.transform.rotation, Quaternion.LookRotation(m_Player.transform.position - m_Enemy.transform.position), Time.deltaTime);
            m_EnemyAnimator.SetBool("IsAttacking", true);
        }
        else
        {
            m_EnemyAnimator.SetBool("IsAttacking", false);
        }
        m_EnemyAnimator.SetFloat("MoveSpeed", m_NavMeshAgent.velocity.magnitude);

        AnimatorStateInfo animatorInfo;
        animatorInfo = m_EnemyAnimator.GetCurrentAnimatorStateInfo(0);
        if ((animatorInfo.normalizedTime >= 0.5f) && (animatorInfo.IsName("Attack_Style_A")))
        {
            if (m_CDA == false)
            {
                m_CharacterController.GetHurt(m_EnemyData.m_Attack,m_EnemyData.m_Defend);
                m_CDA = true;
            }
        }
        if(m_CDA == true)
        {
            m_TimerA += Time.deltaTime;
            if(m_TimerA >= 0.5)
            {
                m_CDA = false;
                m_TimerA = 0;
            }
        }
        if ((animatorInfo.normalizedTime >= 0.5f) && (animatorInfo.IsName("Attack_Style_B")))
        {
            if (m_CDB == false)
            {
                m_CharacterController.GetHurt(m_EnemyData.m_Attack,m_EnemyData.m_Defend);
                m_CDB = true;
            }
        }
        if (m_CDB == true)
        {
            m_TimerB += Time.deltaTime;
            if (m_TimerB >= 0.5)
            {
                m_CDB = false;
                m_TimerB = 0;
            }
        }
        if (m_EnemyData.m_Health <= 0)
        {
            m_EnemyAnimator.Play("Dead_Front");
            
        }
    }

    public void GetHurt(float attack, float defend)
    {
        m_EnemyData.m_Health -= attack * (1-defend/100);
    }
}
