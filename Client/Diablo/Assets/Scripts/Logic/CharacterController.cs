using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController{
    //角色控制信息
    private GameObject m_Player = null;
    private Animator m_PlayerAnimator = null;
    private NavMeshAgent m_NavMeshAgent = null;
    private GameObject m_SelectedEnemy = null;

    private CharacterData m_CharacterData = null;
    public void SetData(CharacterData data)
    {
        m_CharacterData = data;
    }

    private float m_TimerA = 0;
    private bool m_CDA = false;
    private float m_TimerB = 0;
    private bool m_CDB = false;

    // Use this for initialization
    public void OnInitialize()
    {
        m_Player = Object.Instantiate(Resources.Load<GameObject>("Prefab/Player/Player"));
        m_Player.transform.position = new Vector3(0, 0, 15);
        m_PlayerAnimator = m_Player.GetComponent<Animator>();
        m_NavMeshAgent = m_Player.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    public void OnUpdate(float deltaTime)
    {
        //射线检测，鼠标点击移动
        RaycastHit m_hitt = new RaycastHit();
        GameObject m_AimObject = null;
        Vector3 m_target = Vector3.zero;
        Ray m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(m_ray, out m_hitt))
        {
            m_AimObject = m_hitt.collider.gameObject;
            m_target = m_hitt.point;
        }

        //点击鼠标右键移动或锁定敌人
        if (Input.GetMouseButtonDown(1))
        {
            
            if (m_AimObject.tag == "Enemy")
            {
                m_SelectedEnemy = m_AimObject;
                m_NavMeshAgent.SetDestination(m_target + (m_Player.transform.position-m_SelectedEnemy.transform.position).normalized * 4/3);
            }
            else
            {
                m_SelectedEnemy = null;
                m_NavMeshAgent.SetDestination(m_target);
            }
        }
        //判断距离并开始攻击
        if(m_SelectedEnemy != null)
        {
            if (Vector3.Distance(m_Player.transform.position, m_SelectedEnemy.transform.position) <= 1.8)
            {
                m_Player.transform.rotation = Quaternion.Lerp(m_Player.transform.rotation,Quaternion.LookRotation(m_SelectedEnemy.transform.position - m_Player.transform.position),Time.deltaTime);
                m_PlayerAnimator.SetBool("IsAttacking", true);
            }
            if (Logic.GameManager.Instance.EnemyDic[m_SelectedEnemy].GetData().m_IsDead)
            {
                m_SelectedEnemy = null;
            }
        }
        else
        {
            m_PlayerAnimator.SetBool("IsAttacking", false);
        }
        m_PlayerAnimator.SetFloat("MoveSpeed", m_NavMeshAgent.velocity.magnitude);

        //动画判定击中与扣血
        AnimatorStateInfo animatorInfo;
        animatorInfo = m_PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        if ((animatorInfo.normalizedTime >= 0.5f) && (animatorInfo.IsName("Attack_Style_A")))
        {
            if (m_CDA == false)
            {
                Logic.GameManager.Instance.EnemyDic[m_SelectedEnemy].GetController().GetHurt(m_CharacterData.m_Attack,m_CharacterData.m_Defend);
                m_CDA = true;
            }
        }
        if (m_CDA == true)
        {
            m_TimerA += Time.deltaTime;
            if (m_TimerA >= 0.5)
            {
                m_CDA = false;
                m_TimerA = 0;
            }
        }
        if ((animatorInfo.normalizedTime >= 0.5f) && (animatorInfo.IsName("Attack_Style_B")))
        {
            if (m_CDB == false)
            {
                Logic.GameManager.Instance.EnemyDic[m_SelectedEnemy].GetController().GetHurt(m_CharacterData.m_Attack,m_CharacterData.m_Defend);
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

        Debug.Log(m_CharacterData.m_Health);


        if (m_CharacterData.m_Health <= 0)
        {
            m_PlayerAnimator.Play("Dead_Front");
        }
        
    }


    public void GetHurt(float attack, float defend)
    {
        m_CharacterData.m_Health -= attack * (1-defend/100);
    }
    
}
