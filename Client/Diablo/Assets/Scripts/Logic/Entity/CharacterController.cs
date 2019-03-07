using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController{
    //角色控制信息
    private GameObject m_Player = null;
    private Animator m_PlayerAnimator = null;
    private NavMeshAgent m_NavMeshAgent = null;
    private GameObject m_SelectedEnemyGO = null;

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
        CharacterControl();
    }

    public void GetHurt(float attack, float defend)
    {
        m_CharacterData.m_Health -= attack * (1 - defend / 100);
    }

    private void CharacterControl()
    {
        CharacterMove();

        CharacterAttack();

        CharacterAttacked();

        CharacterDead();
    }

    private void MoveToTarget(Vector3 target)
    {
        m_NavMeshAgent.SetDestination(target);
    }

    private void InputManager(GameObject AimObject, Vector3 target)
    {
        //点击鼠标右键移动或锁定敌人
        if (Input.GetMouseButtonDown(1))
        {

            if (AimObject.tag == "Enemy")
            {
                m_SelectedEnemyGO = AimObject;
                if (this.GetDistance() >= 2.6)
                {
                    MoveToTarget(target + (m_Player.transform.position - m_SelectedEnemyGO.transform.position).normalized * 2.5f);
                }
            }
            else
            {
                m_SelectedEnemyGO = null;
                MoveToTarget(target);
            }
        }
    }

    private float GetDistance()
    {
        return Vector3.Distance(m_Player.transform.position, m_SelectedEnemyGO.transform.position);
    }

    private void CharacterMove()
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

        InputManager(m_AimObject, m_target);
    }

    private void CharacterAttack()
    {
        //判断距离并开始攻击
        if (m_SelectedEnemyGO != null)
        {
            if (this.GetDistance() <= 2.6)
            {
                this.TurnToEnemy();
                m_PlayerAnimator.SetBool("IsAttacking", true);
            }
            if (this.GetEnemy().GetData().m_IsDead)
            {
                m_SelectedEnemyGO = null;
            }
            if (this.GetEnemy().GetData().m_Health <= 0)
            {
                m_PlayerAnimator.SetBool("IsAttacking", false);
            }
        }
        else
        {
            m_PlayerAnimator.SetBool("IsAttacking", false);
        }
        m_PlayerAnimator.SetFloat("MoveSpeed", m_NavMeshAgent.velocity.magnitude);
    }
    
    private void TurnToEnemy()
    {
        m_Player.transform.rotation = Quaternion.Lerp(m_Player.transform.rotation, Quaternion.LookRotation(m_SelectedEnemyGO.transform.position - m_Player.transform.position), Time.deltaTime * 2);
    }

    private void CharacterAttacked()
    {
        //动画判定击中与扣血
        AnimatorStateInfo animatorInfo;
        animatorInfo = m_PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        if ((animatorInfo.normalizedTime >= 0.5f) && (animatorInfo.IsName("Attack_Style_A")))
        {
            if (m_CDA == false)
            {
                if (m_SelectedEnemyGO != null && !this.GetEnemy().GetData().m_IsDead)
                    this.GetEnemy().GetController().GetHurt(m_CharacterData.m_Attack, m_CharacterData.m_Defend);
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
                if (m_SelectedEnemyGO != null && !this.GetEnemy().GetData().m_IsDead)
                    this.GetEnemy().GetController().GetHurt(m_CharacterData.m_Attack, m_CharacterData.m_Defend);
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
    }

    private Enemy GetEnemy()
    {
        return Logic.GameManager.Instance.EnemyDict[m_SelectedEnemyGO];
    }

    private void CharacterDead()
    {;
        if (m_CharacterData.m_Health <= 0)
        {
            m_PlayerAnimator.Play("Dead_Front");
        }
    }

    
    
}
